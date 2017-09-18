using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_USED_NICKNAME : Packet
    {
        public PACKET_USED_NICKNAME()
        {
            newPacket(4352);
            addBlock(74070);
        }
    }
}
