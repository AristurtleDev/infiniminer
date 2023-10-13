using System.Net;
using System.Net.Sockets;
using Infiniminer.Packets;
using LiteNetLib;
using LiteNetLib.Utils;

namespace Infiniminer;

public class InfiniminerServerListener : INetEventListener
{
    private readonly NetPacketProcessor _netPacketProcessor;
    private readonly NetManager _server;

    public InfiniminerServerListener(NetManager server)
    {
        _server = server;
        _netPacketProcessor = new NetPacketProcessor();


        _netPacketProcessor.SubscribeReusable<ConnectionApprovalPacket, NetPeer>((packet, peer) =>
        {

        });
    }

    public void OnConnectionRequest(ConnectionRequest request)
    {
        throw new NotImplementedException();
    }

    public void OnNetworkError(IPEndPoint endPoint, SocketError socketError)
    {
        throw new NotImplementedException();
    }

    public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
    {
        throw new NotImplementedException();
    }

    public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, byte channelNumber, DeliveryMethod deliveryMethod)
    {
        throw new NotImplementedException();
    }

    public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
    {
        throw new NotImplementedException();
    }

    public void OnPeerConnected(NetPeer peer)
    {
        throw new NotImplementedException();
    }

    public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
    {
        throw new NotImplementedException();
    }
}
