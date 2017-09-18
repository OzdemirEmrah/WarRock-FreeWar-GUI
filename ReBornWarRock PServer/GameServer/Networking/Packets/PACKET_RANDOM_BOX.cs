using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_RANDOM_BOX : Packet
    {
        public PACKET_RANDOM_BOX(Virtual_Objects.User.virtualUser User, string ItemCode, int Days)
        {
            //30720 1111 1 CB09 DB33-3-0-13080422-0,CB08-2-0-13052022-3,CC02-3-0-13080422-0,DS01-3-0-13080903-0,CA01-3-0-13081400-0,CD01-3-0-13080422-0,CD02-3-0-13080422-0,DB04-1-0-13070914-0,DA09-1-0-13070215-0,DF03-1-0-13070214-0,DT01-1-0-13071700-0,^,DH01-1-0-13071921-0,DI01-1-0-13062921-0,CF02-3-0-13072602-0,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^ T,T,F,F CF02 30 47728 
            newPacket(30720);
            addBlock(1111);
            addBlock(1);
            addBlock("CB09");
            addBlock(User.rebuildWeaponList());
            //T,T,F,F CF02 30 47728
            addBlock(User.getSlots());
            addBlock(ItemCode);
            addBlock(Days);
            addBlock(User.Dinar);
        }
    }
}
