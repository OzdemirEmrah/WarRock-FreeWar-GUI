
using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.Room;


namespace ReBornWarRock_PServer.GameServer.Networking.Handlers
{
    class HANDLE_LEAVE_VEHICLE : PacketHandler
    {
        public override void Handle(virtualUser User)
        {

            {
                User.Room.send(new Packets.PACKET_JOIN_VEHICLE(getAllBlocks()));
            }
        }
    }
    class SP_Unknown : Packet
    {
        public SP_Unknown(int packetId, params object[] par)
        {
            newPacket(packetId);
            foreach (var p in par)
            {
                addBlock(p);
            }
        }
    }
    class HANDLE_MULTIPLAYER_ZOMBIE : PacketHandler
    {
        public override void Handle(Virtual_Objects.User.virtualUser User)
        {
            {
                virtualRoom Room = User.Room;
                if (Room != null)
                {
                    if (Room.Players.Count > 1)
                    {
                        Room.send(new SP_Unknown(31490, getAllBlocks()));
                    }
                }
            }
        }
    }
}


