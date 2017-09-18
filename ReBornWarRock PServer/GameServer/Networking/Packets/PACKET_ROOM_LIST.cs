using System.Collections;

using ReBornWarRock_PServer.GameServer.Managers;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.Room;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_ROOM_LIST : Packet
    {
        public PACKET_ROOM_LIST(ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser User, int Page)
        {
            newPacket(29184);

            ArrayList Rooms = RoomManager.getRoomsInChannel(User.Channel, Page);

            addBlock(Rooms.Count); //Rooms Count
            addBlock(Page); // Room Page
            addBlock(0);

            foreach (virtualRoom Room in Rooms)
            {
                addRoomInfo(Room);
            }
        }
    }
}
