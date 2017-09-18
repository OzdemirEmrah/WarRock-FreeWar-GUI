using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_CLAN_CHANGE : Packet
    {

        public PACKET_CLAN_CHANGE()
        {
            newPacket(26384);
            addBlock(11);
            addBlock(1);
        }

        public PACKET_CLAN_CHANGE(int SubType, int UserID)
        {
            //26384 10 1 2 23343041
            newPacket(26384);
            addBlock(10);
            addBlock(1);
            addBlock(SubType);
            addBlock(UserID);
        }

        public PACKET_CLAN_CHANGE(Virtual_Objects.User.virtualUser User, bool isNick)
        {
            //26384 12 1 DB33-3-0-13080422-0,CB08-2-0-13052022-4,CC02-3-0-13080422-0,DS01-3-0-13080903-0,CA01-3-0-13081400-0,CD01-3-0-13080422-0,CD02-3-0-13080422-0,DB04-1-0-13070914-0,DA09-1-0-13070215-0,DF03-1-0-13070214-0,DT01-1-0-13071700-0,^,DH01-1-0-13071921-0,DI01-1-0-13062921-0,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^ 
            newPacket(26384);
            if (isNick)
                addBlock(12);
            else
                addBlock(14);
            addBlock(1);
            addBlock(User.rebuildWeaponList());
        }
    }
}
