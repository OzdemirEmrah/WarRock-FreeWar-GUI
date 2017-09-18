using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_PARTY_PROGRAM_DETECTED : Packet
    {
        public PACKET_PARTY_PROGRAM_DETECTED()
        {
            /*24576 666 IsWeaponIntegrity↔:↔impossibleitem--1*/
            newPacket(24576);
            addBlock(666);
        }
    }
}
