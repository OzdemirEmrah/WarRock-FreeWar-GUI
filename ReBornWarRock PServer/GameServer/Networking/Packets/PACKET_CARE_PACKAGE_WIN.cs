using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_CARE_PACKAGE_WIN : Packet
    {
        public PACKET_CARE_PACKAGE_WIN(Virtual_Objects.User.virtualUser User, string ItemCode, int Days, bool Dinar, bool Win)
        {
            //30273 1 0 0 DS01 15 DB33-3-0-13070522-0,CB08-2-0-13052022-4,CC02-3-0-13070522-0,DS01-3-0-13071001-0,CA01-3-0-13071500-0,CD01-3-0-13070522-0,CD02-3-0-13070522-0,^,^,^,DT01-1-0-13071700-0,^,DH01-1-0-13071921-0,DI01-1-0-13062921-0,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^ 30228 
            newPacket(30273);
            addBlock(1);
            addBlock(Dinar ? 0 : 1);
            addBlock(Win ? 1 : 0);
            addBlock(ItemCode);
            addBlock(1);
            addBlock(User.rebuildWeaponList());
            if (Dinar)
                addBlock(User.Dinar);
            else
                addBlock(User.Cash);
            addBlock(User.getSlots()); //Slots Enable
        }
    }
}
