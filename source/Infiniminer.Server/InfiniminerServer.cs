/* ----------------------------------------------------------------------------
MIT License

Copyright (c) 2009 Zach Barth
Copyright (c) 2023 Christopher Whitley

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
---------------------------------------------------------------------------- */

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Net;
using Infiniminer.IO;
using Infiniminer.Packets;
using Infiniminer.Server;
using LiteNetLib;
using LiteNetLib.Utils;
using Microsoft.Xna.Framework;

namespace Infiniminer
{
    public class InfiniminerServer
    {
        private BlockType[,,]? _blockList;   //  In game coordinates, where Y points up
        private PlayerTeam[,,]? _blockCreatorTeam;
        // private ServerConfig? _config;
        private HashSet<IPAddress>? _banList;

        //  Tracking
        private int _lavaBlockCount;
        private PlayerTeam _winningTeam;
        DateTime _restartTime;
        private bool _restartTriggered;
        private uint _duplicateNameCount;

        //  Server
        private NetManager? _server;
        private EventBasedNetListener? _serverListener;
        private NetPacketProcessor? _packetProcessor;
        private readonly InfiniminerServerConsole _console;



        public InfiniminerServer()
        {
            _lavaBlockCount = 0;
            _winningTeam = PlayerTeam.None;
            _restartTime = DateTime.Now;
            _restartTriggered = false;
            _duplicateNameCount = 0;
        }

        public bool Start()
        {
            ServerConfig config = LoadServerConfig();
            HashSet<IPAddress> banlist = LoadBanList();
            GenerateWorld(config.MapSize, config.IncludeLava, config.OreFactor, out _blockList, out _blockCreatorTeam);
            InitializeLava(config.MapSize);
        }

        private static ServerConfig LoadServerConfig()
        {
            ServerConfig config = new ServerConfig();
            using ConfigurationFileReader reader = new ConfigurationFileReader("server.config.txt");

            ConfigurationItem? item = null;
            while ((item = reader.ReadLine()) is not null)
            {
                switch (item.Key)
                {
                    case nameof(ServerConfig.ServerName):
                        config.ServerName = item.Value;
                        break;

                    case nameof(ServerConfig.MaxPlayers):
                        config.MaxPlayers = uint.Parse(item.Value, System.Globalization.CultureInfo.InvariantCulture);
                        break;

                    case nameof(ServerConfig.Port):
                        config.Port = int.Parse(item.Value, System.Globalization.CultureInfo.InvariantCulture);
                        break;

                    case nameof(ServerConfig.IsPublic):
                        config.IsPublic = bool.Parse(item.Value);
                        break;

                    case nameof(ServerConfig.PublicHost):
                        config.PublicHost = item.Value;
                        break;

                    case nameof(ServerConfig.MapSize):
                        config.MapSize = int.Parse(item.Value, System.Globalization.CultureInfo.InvariantCulture);
                        break;

                    case nameof(ServerConfig.OreFactor):
                        config.OreFactor = uint.Parse(item.Value, System.Globalization.CultureInfo.InvariantCulture);
                        break;

                    case nameof(ServerConfig.IncludeLava):
                        config.IncludeLava = bool.Parse(item.Value);
                        break;

                    case nameof(ServerConfig.SandboxMode):
                        config.SandboxMode = bool.Parse(item.Value);
                        break;

                    default: continue;
                }
            }

            return config;
        }

        private HashSet<IPAddress> LoadBanList()
        {
            HashSet<IPAddress> banList = new HashSet<IPAddress>();

            FileStream? stream = null;
            StreamReader? reader = null;

            try
            {
                stream = File.OpenRead("banlist.txt");
                reader = new StreamReader(stream);

                string? line = null;
                while ((line = reader.ReadLine()) is not null)
                {
                    try
                    {
                        IPAddress bannedAddress = IPAddress.Parse(line);
                        if (!banList.Add(bannedAddress))
                        {
                            _console.WriteWarningLine($"Duplicate IP Address in banlist.txt: '{line}'");
                        }

                    }
                    catch
                    {
                        //  If the line could not be parsed as an IPAddress, then 
                        //  we ignore it, log it, and continue to the next line
                        _console.WriteErrorLine($"'{line}' is not a valid IP Address");
                        continue;
                    }
                }
            }
            catch { }
            finally
            {
                stream?.Close();
                stream = null;
                reader?.Close();
                reader = null;
            }

            return banList;

        }

        private void InitializeLava(int mapSize)
        {
            _console.WriteLine("Calculating Initial Lava Flows");

            for (int i = 0; i < mapSize * 2; i++)
            {
                DoLavaStuff(mapSize);
            }

            _console.WriteLine($"Total Lava Blocks = {_lavaBlockCount}");
        }

        private void DoLavaStuff(int mapSize)
        {
            bool[,,] flowSleep = new bool[mapSize, mapSize, mapSize];   //  if true, do not calculate this turn

            for (ushort i = 0; i < mapSize; i++)
            {
                for (ushort j = 0; j < mapSize; j++)
                {
                    for (ushort k = 0; k < mapSize; k++)
                    {
                        flowSleep[i, j, k] = false;
                    }
                }
            }

            

            for (ushort i = 0; i < mapSize; i++)
            {
                for (ushort j = 0; j < mapSize; j++)
                {
                    for (ushort k = 0; k < mapSize; k++)
                    {
                        if (_blockList[i, j, k] == BlockType.Lava && !flowSleep[i, j, k])
                        {
                            // RULES FOR LAVA EXPANSION:
                            // if the block below is lava, do nothing
                            // if the block below is empty space, add lava there
                            // if the block below is something solid, add lava to the sides
                            BlockType typeBelow = (j == 0) ? BlockType.Lava : blockList[i, j - 1, k];
                            if (typeBelow == BlockType.None)
                            {
                                if (j > 0)
                                {
                                    SetBlock(i, (ushort)(j - 1), k, BlockType.Lava, PlayerTeam.None);
                                    flowSleep[i, j - 1, k] = true;
                                }
                            }
                            else if (typeBelow != BlockType.Lava)
                            {
                                if (i > 0 && blockList[i - 1, j, k] == BlockType.None)
                                {
                                    SetBlock((ushort)(i - 1), j, k, BlockType.Lava, PlayerTeam.None);
                                    flowSleep[i - 1, j, k] = true;
                                }
                                if (k > 0 && blockList[i, j, k - 1] == BlockType.None)
                                {
                                    SetBlock(i, j, (ushort)(k - 1), BlockType.Lava, PlayerTeam.None);
                                    flowSleep[i, j, k - 1] = true;
                                }
                                if (i < config.MapSize - 1 && blockList[i + 1, j, k] == BlockType.None)
                                {
                                    SetBlock((ushort)(i + 1), j, k, BlockType.Lava, PlayerTeam.None);
                                    flowSleep[i + 1, j, k] = true;
                                }
                                if (k < config.MapSize - 1 && blockList[i, j, k + 1] == BlockType.None)
                                {
                                    SetBlock(i, j, (ushort)(k + 1), BlockType.Lava, PlayerTeam.None);
                                    flowSleep[i, j, k + 1] = true;
                                }
                            }
                        }
                    }
                }
            }


        }


        private static void GenerateWorld(int mapSize, bool includeLava, uint oreFactor, [NotNull] out BlockType[,,]? blockList, [NotNull] out PlayerTeam[,,]? blockCreatorTeam)
        {
            //  Create our block world, translating the coordinates out of the cave generator (where Z points down)
            BlockType[,,] worldData = CaveGenerator.GenerateCaveSystem(mapSize, includeLava, oreFactor);

            blockList = new BlockType[mapSize, mapSize, mapSize];
            blockCreatorTeam = new PlayerTeam[mapSize, mapSize, mapSize];

            for (ushort i = 0; i < mapSize; i++)
            {
                for (ushort j = 0; j < mapSize; j++)
                {
                    for (ushort k = 0; k < mapSize; k++)
                    {
                        blockList[i, (ushort)(mapSize - 1 - k), j] = worldData[i, j, k];
                        blockCreatorTeam[i, j, k] = PlayerTeam.None;
                    }
                }
            }
        }
    }
}
