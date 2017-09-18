using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_SKILL_POINT_KILL : Packet
    {
        public PACKET_SKILL_POINT_KILL(params object[] Params)
        {
            newPacket(31492);
            foreach (object Parameter in Params)
            {
                addBlock(Parameter);
            }
        }
    }
}
