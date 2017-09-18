using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ReBornWarRock_PServer.GameServer.Managers;
using ReBornWarRock_PServer.GameServer.Networking.Packets;

namespace ReBornWarRock_PServer.GameServer.Networking.Handlers
{
    class HANDLE_COSTUME_BUY : PacketHandler
    {
        public override void Handle(ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser User)
        {
            try
            {
                int OPCode = Convert.ToInt32(getBlock(0));
                int Period = Convert.ToInt32(getBlock(4));
                string Code = getBlock(1);
                int[] convertDays = new int[6] { 3, 7, 15, 30, 1, -1};
                 Item Item = ItemManager.getItem(Code);
                if (Item != null)
                {
                    int Price = Item.getCashPrice(Period);
                    if (User.Cash - Price < 1)
                    {
                        User.send(new PACKET_ITEMSHOP(PACKET_ITEMSHOP.ErrorCodes.NotEnoughDinar, "NULL"));
                    }
                    else if (LevelCalculator.getLevelforExp(User.Exp) < Item.Level && User.Rank < 3)
                    {
                        User.send(new PACKET_ITEMSHOP(PACKET_ITEMSHOP.ErrorCodes.LevelLow, "NULL"));
                    }
                    else
                    {
                        int InventorySlot = User.InventorySlots;
                        if (InventorySlot >= 0)
                        {
                            User.Cash -= Price;
                            DB.runQuery("UPDATE users SET cash='" + User.Cash + "' WHERE id='" + User.UserID + "'");
                            User.LoadItems();
                            User.AddOutBoxItem(Code, convertDays[Period], 1);
                            User.send(new PACKET_OUTBOX_SEND(User));
                            Log.AppendText(User.Nickname + " has bought [" + Item.Code.ToUpper() + "-" + Item.Name + "] for " + convertDays/*[Period]*/ + "days.");
                        }
                        else
                        {
                            User.send(new PACKET_ITEMSHOP(PACKET_ITEMSHOP.ErrorCodes.InventoryFull, "NULL"));
                        }
                    }
                }
                else
                {
                    User.send(new Packets.PACKET_ITEMSHOP(Packets.PACKET_ITEMSHOP.ErrorCodes.CannotBeBougth, "NULL"));
                }
            }
            catch (Exception ex)
            {
                Log.AppendError(ex.Message);
            }
        }
    }
}
