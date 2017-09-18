using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_LOGIN_EVENT : Packet
    {
        public PACKET_LOGIN_EVENT()
        {
            newPacket(30993);
            addBlock(-1);
        }
        public PACKET_LOGIN_EVENT(ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser User, string Weapon)
        {
            //30993 1 1 1 DA09 3 ^,^,DA09-1-0-13050402-0-0-0-0-0,DC03-1-3-13050613-0-0-0-0-0,^,^,^,DJ09-1-0-13062000-0-0-0-0-0,DN03-1-0-13062000-0-0-0-0-0,DZ01-3-0-13062000-0-0-0-0-0-9999-9999,DS01-3-0-13050600-0-0-0-0-0-9999-9999,DG08-1-0-13062001-0-0-0-0-0,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^ F,T,F,F 7807 
            newPacket(30993);
            addBlock(1);
            addBlock(User.Dinar);
            addBlock(User.LoginEvent);
            addBlock(Weapon);
            addBlock(3);
            addBlock(User.rebuildWeaponList());
            addBlock(User.getSlots()); //Slots Enabled
            addBlock(User.rebuildCostumeList());
            addBlock(User.Dinar);
        }
    }
}
