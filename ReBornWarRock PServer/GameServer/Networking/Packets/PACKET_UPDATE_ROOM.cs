using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_UPDATE_ROOM : Packet
    {
        public PACKET_UPDATE_ROOM(Virtual_Objects.Room.virtualRoom Room)
        {
            newPacket(29200);
            addBlock(Room.ID);
            addBlock(1);
            addRoomInfo(Room);
        }
    }
}
