using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReBornWarRock_PServer.GameServer.Virtual_Objects;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.User.Inventory;

namespace ReBornWarRock_PServer.GameServer.Networking.Handlers
{
    class HANDLE_DELETE_WEAPON : PacketHandler
    {
        public override void Handle(ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser User)
        {
            string RemoveItem = getNextBlock();
            int InvID = Convert.ToInt32(getNextBlock());

            if (User.hasItem(RemoveItem))
            {
                string inventoryID = User.getInventoryID(RemoveItem);
                for (int I = 0; I < 5; I++)
                {
                    for (int J = 0; J < 8; J++)
                    {
                        if (User.Equipment[I, J].Contains(inventoryID))
                            User.Equipment[I, J] = "^";
                    }
                }
                User.SaveEquipment();
                User.LoadEquipment();
                User.reloadEquipment();
                DB.runQuery("DELETE FROM inventory WHERE ownerid='" + User.UserID + "' AND itemcode='" + RemoveItem + "'");
                User.Inventory = new InventoryItem[105];
                User.LoadItems();
                User.send(new Packets.PACKET_DELETE_WEAPON(User, RemoveItem));
            }
            else
                User.disconnect();
        }
    }
}