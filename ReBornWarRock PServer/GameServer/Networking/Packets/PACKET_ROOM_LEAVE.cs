using ReBornWarRock_PServer.GameServer.Virtual_Objects.Room;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_LEAVE_ROOM : Packet
    {
        public PACKET_LEAVE_ROOM(ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser User, virtualRoom Room, int oldPlace, int newMaster)
        {
            newPacket(29504);
            addBlock(1);
            addBlock(User.SessionID); // SessionID
            addBlock(oldPlace); // Position in currentRoom
            addBlock(0); // ?
            addBlock(newMaster); // Master Slot
            addBlock(User.Exp); // Exp
            addBlock(User.Dinar); // Dinar
        }
    }
}
