using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_UNKNOW : Packet
    {
        public PACKET_UNKNOW(int packetId, params object[] par)
        {
            newPacket(packetId);
            foreach (var p in par)
            {
                addBlock(p);
            }
        }
    }
}
