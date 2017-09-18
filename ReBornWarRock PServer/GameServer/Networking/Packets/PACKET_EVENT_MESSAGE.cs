using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_EVENT_MESSAGE : Packet
    {
        public enum EventCodes
        {
            EXP_Activate = 810,
            EXP_Deactivate = 820
        }
        public PACKET_EVENT_MESSAGE(EventCodes eCode)
        {
            newPacket(25344);
            addBlock((int)eCode);
            addBlock(0);
        }
    }
}
