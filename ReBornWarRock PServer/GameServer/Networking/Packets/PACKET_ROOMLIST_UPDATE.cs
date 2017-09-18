using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_ROOMLIST_UPDATE : Packet
    {
        public PACKET_ROOMLIST_UPDATE(Virtual_Objects.Room.virtualRoom Room, int currentRoomExist = 1)
        {
            //29200 76 1 76 2 2 0 Aufgeht's! 0 8 2 43 0 0 2 2 3 1 0 1 0 0 0 1 0 30 0 -1
            newPacket(29200);
            addBlock(Room.ID);
            addBlock(currentRoomExist);
            if (currentRoomExist != 2)
            {
                addRoomInfo(Room);
            }
        }
    }
}
