using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;
namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_JOIN_ROOM : Packet
    {
        public enum ErrorCodes
        {
            GenericError = 94010,
            InvalidPassword = 94030,
            BadLevel = 94300,
            OnlyPremium = 94301
        }

        

        public PACKET_JOIN_ROOM(ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser User, Virtual_Objects.Room.virtualRoom Room)
        {
            newPacket(29456);
            addBlock(1);
            addBlock(User.RoomSlot);
            addRoomInfo(Room);
        }

        public PACKET_JOIN_ROOM(ErrorCodes ErrCode)
        {
            newPacket(29456);
            addBlock((int)ErrCode);
        }
    }
   
}
