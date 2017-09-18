using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_SPECTATE_ROOM : Packet
    {
        public PACKET_SPECTATE_ROOM() // Leave
        {
            newPacket(29488);
            addBlock(0);
        }

        public PACKET_SPECTATE_ROOM(ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser User, Virtual_Objects.Room.virtualRoom Room) // Join
        {
            newPacket(29488);
            addBlock(1);
            addBlock(1);
            addBlock(32 + User.SpectatorID);
            addRoomInfo(Room);
        }
    }
}
