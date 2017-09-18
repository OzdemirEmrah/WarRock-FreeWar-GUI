using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_DELETE_WEAPON : Packet
    {
        public PACKET_DELETE_WEAPON(ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser User, string itemCode)
        {
            newPacket(30224);
            addBlock(1);
            addBlock(itemCode);
            // Build Inventory //
            addBlock(User.rebuildWeaponList());
            addBlock(User.getSlots()); //Slots Enabled
            // Player Equipment //
            for (int Class = 0; Class < 5; Class++)
            {
                StringBuilder ClassBuilder = new StringBuilder();

                for (int Slot = 0; Slot < 8; Slot++)
                {
                    ClassBuilder.Append(User.Equipment[Class, Slot]);
                    if (Slot != 7) ClassBuilder.Append(",");

                }
                addBlock(ClassBuilder.ToString());
            }
        }
    }
}
