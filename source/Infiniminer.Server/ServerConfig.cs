namespace Infiniminer;

public class ServerConfig
{
    /// <summary>
    ///     The display name to show as the server name in the server list.
    /// </summary>
    public string ServerName { get; set; } = "Unnamed Server";

    /// <summary>
    ///     The maximum number of player connections allowed on this server.
    /// </summary>
    public uint MaxPlayers { get; set; } = 16;

    /// <summary>
    ///     The port number that is used by this server.
    /// </summary>
    public int Port { get; set; } = 5565;

    /// <summary>
    ///     <see langword="true"/> if the server should be listed in the public server list; otherwise,
    ///     <see langword="false"/>.
    /// </summary>
    public bool IsPublic { get; set; } = false;

    /// <summary>
    ///     The hostname or IPv4 address of the public server that your local sever should be listed on.
    /// </summary>
    public string PublicHost { get; set; } = string.Empty;

    /// <summary>
    ///     The total number of blocks for the width, height, and depth of the map.
    /// </summary>
    public int MapSize { get; set; } = 64;

    /// <summary>
    ///     The number of ore veins generated in the world.
    /// </summary>
    public uint OreFactor { get; set; } = 10;

    /// <summary>
    ///     When <see langword="true"/>, indicates that lava blocks will be generated in the world.
    /// </summary>
    public bool IncludeLava { get; set; } = true;

    /// <summary>
    ///     When <see langword="true"/>, all blocks will be free and the game will be unwinnable.
    /// </summary>
    public bool SandboxMode { get; set; } = false;


}