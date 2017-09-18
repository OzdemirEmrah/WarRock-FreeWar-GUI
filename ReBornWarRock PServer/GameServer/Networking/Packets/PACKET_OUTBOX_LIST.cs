using ReBornWarRock_PServer.GameServer.Virtual_Objects.User.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_OUTBOX_LIST : Packet
    {
        public PACKET_OUTBOX_LIST(GameServer.Virtual_Objects.User.virtualUser User)
        {
            newPacket(30752);
            addBlock(1117);
            addBlock(1);
            addBlock(User.Dinar);
            addBlock(-1);
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
            addBlock(0);
            addBlock(0);
            addBlock(0);
        }
    }
}
