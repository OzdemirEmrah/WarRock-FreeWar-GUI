using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_MESSAGE_BOX : Packet
    {
        public PACKET_MESSAGE_BOX(string Message)
        {
            newPacket(1337);
            addBlock(10);
            addBlock(1);
            addBlock(0);
            addBlock(Message);
        }
    }
}
