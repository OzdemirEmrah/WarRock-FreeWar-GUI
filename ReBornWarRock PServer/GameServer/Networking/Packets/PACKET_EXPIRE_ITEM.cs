using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ReBornWarRock_PServer.GameServer.Managers;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_EXPIRE_ITEM : Packet
    {
        public PACKET_EXPIRE_ITEM(ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser User)
        {
            newPacket(30976);
            addBlock(1);
            addBlock(User.getSlots()); //Slots Enable
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
            // Build Inventory //
            addBlock(User.rebuildWeaponList());
            addBlock(User.LeftItems.Count);
            foreach (string Item in User.LeftItems)
            {
                addBlock(Item);
            }
            User.LeftItems.Clear();
        }
    }
}
