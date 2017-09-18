using ReBornWarRock_PServer.GameServer.Virtual_Objects.User.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_OUTBOX_SEND : Packet
    {
        public PACKET_OUTBOX_SEND(GameServer.Virtual_Objects.User.virtualUser User)
        {
            //30752 1117 1 46628 0 500 LIST 5 4386 14297567 CB09 0 2013-05-15 vitostrong vitostrong 0 4386 14297565 CB09 0 2013-05-15 vitostrong vitostrong 0 4386 14297564 CB09 0 2013-05-15 vitostrong vitostrong 0 4386 14297522 CB09 0 2013-05-15 vitostrong vitostrong 0 4386 14297519 CB09 0 2013-05-15 vitostrong vitostrong 0
            //1 DB33-3-0-13080422-0,DA03-1-1-13071419-0,CC02-3-0-13080422-0,DS01-3-0-13080903-0,CA01-3-0-13081400-0,CD01-3-0-13080422-0,CD02-3-0-13080422-0,DB04-1-0-13070914-0,^,DV01-3-0-13080620-0,DT01-1-0-13071700-0,DI05-1-0-13071720-0,DH01-1-0-13071921-0,DF12-3-0-13071420-0,CF02-3-0-13072602-0,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^ T,T,F,F BA10-3-0-14032700-0-0,BA08-3-0-14070720-0-0,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^ 0 1 
            newPacket(30752);
            addBlock(1117);
            addBlock(1);
            addBlock(User.Dinar);
            addBlock(0);
            addBlock(User.Cash);
            addBlock("LIST");
            addBlock(User.OutBoxItems.Count);
            foreach (OutboxItem Item in User.OutBoxItems)
            {
                addBlock(Item.ID);
                addBlock(User.UserID);
                addBlock(Item.ItemCode);
                addBlock((Item.Count > 1 ? Item.Count : Item.Days));
                addBlock("NULL");
                addBlock("NULL");
                addBlock(User.Nickname);
                addBlock(0);
            }
            addBlock(1);
            addBlock(User.rebuildWeaponList());
            addBlock(User.getSlots()); //Slots Enabled
            addBlock(User.rebuildCostumeList());
            addBlock(0);
            addBlock(1);
        }
    }
}
