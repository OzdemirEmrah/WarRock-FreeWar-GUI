using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_TO_CLIENT : Packet
    {
        public PACKET_TO_CLIENT(int Subtype)
        {
            newPacket(46723);
            addBlock(Subtype);
            addBlock(0);
        }
    }
}
