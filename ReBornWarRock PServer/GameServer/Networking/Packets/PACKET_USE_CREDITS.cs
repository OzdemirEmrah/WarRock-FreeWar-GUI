using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_USE_CREDITS : Packet
    {
        public PACKET_USE_CREDITS(ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser User, string ItemCode)
        {
            if (ItemCode == "CB03") // Kill/Death Reset
            {
                //30720 1111 1 CB03 DB33-3-0-13070522-0,CB08-2-0-13052022-4,CC02-3-0-13070522-0,^,CA01-3-0-13071223-0,CD01-3-0-13070522-0,CD02-3-0-13070522-0,DJ09-1-0-13062000-0,DN03-1-0-13062000-0,DZ01-3-0-13062000-0,DT01-1-0-13071700-0,DG08-1-0-13062001-0,DH01-1-0-13071921-0,DI01-1-0-13062921-0,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^ T,F,F,F 
                User.rKills = User.rDeaths = 0;
                newPacket(30720);
                addBlock(1111);
                addBlock(1);
                addBlock("CB03");
                addBlock(User.rebuildWeaponList());
                addBlock(User.getSlots());
            }
            else if (ItemCode == "CB09") // Golden Key
            {
                //30720 1111 1 CB09 DB33-3-0-13070522-0,CB08-2-0-13052022-4,CC02-3-0-13070522-0,^,CA01-3-0-13071223-0,CD01-3-0-13070522-0,CD02-3-0-13070522-0,DJ09-1-0-13062000-0,DN03-1-0-13062000-0,DZ01-3
                newPacket(30720);
                addBlock(1111);
                addBlock(1);
                addBlock("CB09");
                addBlock(User.rebuildWeaponList());
            }
        }
    }
}
