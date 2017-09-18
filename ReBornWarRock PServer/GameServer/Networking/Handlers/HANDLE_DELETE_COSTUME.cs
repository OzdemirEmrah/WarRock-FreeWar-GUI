using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReBornWarRock_PServer.GameServer.Virtual_Objects;

using ReBornWarRock_PServer.GameServer.Managers;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.User.Inventory;

namespace ReBornWarRock_PServer.GameServer.Networking.Handlers
{
    class HANDLE_DELETE_COSTUME : PacketHandler
    {
        public override void Handle(ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser User)
        {
                string RemoveItem = getBlock(0);

                if (User.hasCostume(RemoveItem))
                { 
                        DB.runQuery("DELETE FROM inventory_costume WHERE ownerid='" + User.UserID + "' AND itemcode='" + RemoveItem + "'");
                        User.Costume = new CostumeItem[105];
                        User.LoadItems();
                        User.send(new Packets.PACKET_DELETE_COSTUME(User, RemoveItem));
                        User.reloadCash();
                }
                else
                    User.disconnect();
        }
    }
}