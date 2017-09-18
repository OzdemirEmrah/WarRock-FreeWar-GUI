using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_COSTUME_EQUIPMENT : Packet
    {
        public PACKET_COSTUME_EQUIPMENT(ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser User, int Class, string Code)
        {
            //29971 0 3 B 0 BA04 0
            newPacket(29971);
            addBlock(1);
            addBlock(Class);
            // Costume Inventory //
            addBlock(Code);
        }
    }
}
