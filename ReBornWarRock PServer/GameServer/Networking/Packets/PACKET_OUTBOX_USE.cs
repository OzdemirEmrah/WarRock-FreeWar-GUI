using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_OUTBOX_USE : Packet
    {
        public PACKET_OUTBOX_USE(Virtual_Objects.User.virtualUser User, string itemCode)
        {
            //30752 1118 1 46628 0 0 CB09 0 1 DB33-3-0-13080422-0,DA03-1-1-13071419-0,CC02-3-0-13080422-0,DS01-3-0-13080903-0,CA01-3-0-13081400-0,CD01-3-0-13080422-0,CD02-3-0-13080422-0,DB04-1-0-13070914-0,CB09-2-2-13070719-6,DV01-3-0-13080620-0,DT01-1-0-13071700-0,DI05-1-0-13071720-0,DH01-1-0-13071921-0,DF12-3-0-13071420-0,CF02-3-0-13072602-0,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^ T,T,F,F BA10-3-0-14032700-0-0,BA08-3-0-14070720-0-0,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^ 1 0 
            newPacket(30752);
            addBlock(1118);
            addBlock(1);
            addBlock(User.Dinar);
            addBlock(0);
            addBlock(User.Cash);
            addBlock(itemCode);
            addBlock(0);
            addBlock(1);
            addBlock(User.rebuildWeaponList());
            addBlock(User.getSlots()); //Slots Enabled
            addBlock(User.rebuildCostumeList());
            addBlock(1);
            addBlock(0);
        }
    }
}
