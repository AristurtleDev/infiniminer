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

using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Infiniminer
{
    public class Player
    {
        public bool Kicked = false; // set to true when a player is kicked to let other clients know they were kicked
        public string Handle = "";
        public uint OreMax = 0;
        public uint WeightMax = 0;
        public uint Ore = 0;
        public uint Weight = 0;
        public uint Cash = 0;
        public bool Alive = false;
        public List<Vector3> ExplosiveList = new List<Vector3>();
        public uint ID;
        public Vector3 Heading = Vector3.Zero;
        public NetConnection NetConn;
        public float TimeIdle = 0;
        public uint Score = 0;
        public float Ping = 0;
        public string IP = "";

        // This is used to force an update that says the player is not using their tool, thus causing a break
        // in their tool usage animation.
        public bool QueueAnimationBreak = false;

        // Things that affect animation.
        public SpriteModel? SpriteModel;
        private Game? gameInstance;

        private bool idleAnimation = false;
        public bool IdleAnimation
        {
            get { return idleAnimation; }
            set
            {
                if (idleAnimation != value)
                {
                    idleAnimation = value;
                    if (gameInstance != null)
                    {
                        if (idleAnimation)
                            SpriteModel?.SetPassiveAnimation("1,0.2");
                        else
                            SpriteModel?.SetPassiveAnimation("0,0.2;1,0.2;2,0.2;1,0.2");
                    }
                }
            }
        }

        private Vector3 position = Vector3.Zero;
        public Vector3 Position
        {
            get { return position; }
            set
            {
                if (position != value)
                {
                    TimeIdle = 0;
                    IdleAnimation = false;
                    position = value;
                }
            }
        }

        private struct InterpolationPacket
        {
            public Vector3 position;
            public double gameTime;

            public InterpolationPacket(Vector3 position, double gameTime)
            {
                this.position = position;
                this.gameTime = gameTime;
            }
        }

        private List<InterpolationPacket> interpList = new List<InterpolationPacket>();

        public void UpdatePosition(Vector3 position, double gameTime)
        {
            interpList.Add(new InterpolationPacket(position, gameTime));

            // If we have less than 10 packets, go ahead and set the position directly.
            if (interpList.Count < 10)
                Position = position;

            // If we have more than 10 packets, remove the oldest.
            if (interpList.Count > 10)
                interpList.RemoveAt(0);
        }

        public void StepInterpolation(double gameTime)
        {
            // We have 10 packets, so interpolate from the second to last to the last.
            if (interpList.Count == 10)
            {
                Vector3 a = interpList[8].position, b = interpList[9].position;
                double ta = interpList[8].gameTime, tb = interpList[9].gameTime;
                Vector3 d = b - a;
                double timeScale = (interpList[9].gameTime - interpList[0].gameTime) / 9;
                double timeAmount = Math.Min((gameTime - ta) / timeScale, 1);
                Position = a + d * (float)timeAmount;
            }
        }

        private PlayerTeam team = PlayerTeam.None;
        public PlayerTeam Team
        {
            get { return team; }
            set
            {
                if (value != team)
                {
                    team = value;
                    UpdateSpriteTexture();
                }
            }
        }
        private PlayerTools tool = PlayerTools.Pickaxe;
        public PlayerTools Tool
        {
            get { return tool; }
            set
            {
                if (value != tool)
                {
                    tool = value;
                    UpdateSpriteTexture();
                }
            }
        }
        private bool usingTool = false;
        public bool UsingTool
        {
            get { return usingTool; }
            set
            {
                if (value != usingTool)
                {
                    usingTool = value;
                    if (usingTool == true && gameInstance != null)
                        SpriteModel?.StartActiveAnimation("3,0.15");
                }
            }
        }

        public Player(NetConnection netConn, Game? gameInstance)
        {
            this.gameInstance = gameInstance;
            this.NetConn = netConn;
            this.ID = Player.GetUniqueId();

            if (netConn != null)
                this.IP = netConn.RemoteEndpoint.Address.ToString();

            if (gameInstance != null)
            {
                Texture2D tex = gameInstance.Content.Load<Texture2D>(GenerateTextureName());
                this.SpriteModel = new SpriteModel(gameInstance, 4, tex);
                this.IdleAnimation = true;
            }
        }

        private string GenerateTextureName()
        {
            string name = "sprites/tex_sprite_";

            if (team == PlayerTeam.Red)
            {
                name += "red_";
            }
            else
            {
                name += "blue_";
            }

            switch (tool)
            {
                case PlayerTools.ConstructionGun:
                case PlayerTools.DeconstructionGun:
                    name += "construction";
                    break;
                case PlayerTools.Detonator:
                    name += "detonator";
                    break;
                case PlayerTools.Pickaxe:
                    name += "pickaxe";
                    break;
                case PlayerTools.ProspectingRadar:
                    name += "radar";
                    break;
                default:
                    name += "pickaxe";
                    break;
            }

            return name;
        }

        private void UpdateSpriteTexture()
        {
            if (gameInstance == null)
                return;

            string contentPath = GenerateTextureName();
            Texture2D texture = gameInstance.Content.Load<Texture2D>(contentPath);
            SpriteModel?.SetSpriteTexture(texture);
        }

        static uint uniqueId = 0;
        public static uint GetUniqueId()
        {
            uniqueId += 1;
            return uniqueId;
        }
    }
}
