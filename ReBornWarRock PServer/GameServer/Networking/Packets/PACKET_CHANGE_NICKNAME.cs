using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_CHANGE_NICKNAME : Packet
    {
        public PACKET_CHANGE_NICKNAME()
        {
            newPacket(4352);
            addBlock(74070);
        }
        public PACKET_CHANGE_NICKNAME(ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser User, string NewNickname)
        {
            //30720 1111 1 CB01 ^,^,DA09-1-0-13050402-0-0-0-0-0,DC03-1-3-13050613-0-0-0-0-0,^,^,^,DJ09-1-0-13062000-0-0-0-0-0,DN03-1-0-13062000-0-0-0-0-0,DZ01-3-0-13062000-0-0-0-0-0-9999-9999,DS01-3-0-13050600-0-0-0-0-0-9999-9999,DG08-1-0-13062001-0-0-0-0-0,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^ F,T,F,F ToXiiC 
            newPacket(30720);
            addBlock(1111);
            addBlock(1);
            addBlock("CB01");
            addBlock(User.rebuildWeaponList());
            addBlock(User.getSlots()); // Slot Enabled//Slots Enabled
            addBlock(NewNickname);
        }
    }
}
