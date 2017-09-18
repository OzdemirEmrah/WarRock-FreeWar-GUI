using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReBornWarRock_PServer.GameServer.Managers;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_SUICIDE : Packet
    {
        internal enum SuicideType
        {
            Suicide = 0,
            KilledByNotHavinHealTreatment = 1,

        }
        public PACKET_SUICIDE(int slotId, SuicideType type = SuicideType.Suicide, bool outofworld = false)
        {
            newPacket(30000);
            addBlock(1);
            addBlock(slotId);
            addBlock(-1);
            addBlock(2);
            addBlock(157);
            addBlock(0);
            addBlock((int)type);
            addBlock((outofworld ? 2 : slotId));
            Fill(0, 7);
        }
    }
}
