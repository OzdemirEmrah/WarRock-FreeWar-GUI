using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_LEVEL_UP : Packet
    {
        public PACKET_LEVEL_UP(ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser User, int Dinar)
        {
            //31008 1 0 1 2336 2500
            //31008 2 2340 0 10000 T,F,F,F CA01-3-0-13071814-0,DA03-1-0-13071813-0,DB08-1-0-13071813-0,DC06-1-0-13071813-0,DF04-1-0-13071813-0,CB08-2-0-13071114-1,DC03-1-1-13071815-0,DJ03-1-1-13071815-0,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^ ^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^ 0
            newPacket(31008);
            addBlock(User.RoomSlot);
            addBlock(User.Exp);
            addBlock(0);//item.count
            //foreach()
            //{
            //    addBlock("DF05");//code
            //    addBlock(7);//day
            //}
            addBlock(Dinar);
            addBlock(User.getSlots());
            addBlock(User.rebuildWeaponList());
            addBlock(User.rebuildCostumeList());
            addBlock(0);
        }
    }
}
