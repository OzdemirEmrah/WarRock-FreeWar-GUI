using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_CUSTOM : Packet
    {
        public PACKET_CUSTOM(params object[] Params)
        {
            int type = Convert.ToInt32(Params[1]);
            newPacket(type);
            for (int i = 2; i < Params.Length; i++)
            {
                addBlock(Params[i]);
            }
        }
    }
}
