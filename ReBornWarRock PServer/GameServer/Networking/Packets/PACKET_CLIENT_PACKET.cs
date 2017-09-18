using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_CLIENT_PACKET : Packet
    {
        public PACKET_CLIENT_PACKET(int Operation)
        {
            newPacket(1337);
            addBlock(Operation);
            addBlock(1);
            addBlock(0);
        }
    }
}
