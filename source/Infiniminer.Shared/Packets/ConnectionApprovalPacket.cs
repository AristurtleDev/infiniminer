using LiteNetLib.Utils;

namespace Infiniminer.Packets;

public class ConnectionApprovalPacket : INetSerializable
{
    public string? PlayerHandle { get; set; } = null;

    public ConnectionApprovalPacket() { }

    public ConnectionApprovalPacket(string playerHandle) => PlayerHandle = playerHandle;

    public void Deserialize(NetDataReader reader)
    {
        PlayerHandle = reader.GetString();
    }

    public void Serialize(NetDataWriter writer)
    {
        writer.Put(PlayerHandle);
    }
}