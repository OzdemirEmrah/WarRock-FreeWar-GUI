using System;
using System.Text;

using ReBornWarRock_PServer.GameServer.Managers;
using ReBornWarRock_PServer.GameServer.Networking.Packets;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.User.Inventory;

namespace ReBornWarRock_PServer.GameServer.Networking.Handlers
{
    class HANDLE_ITEMSHOP : PacketHandler
    {
        public override void Handle(ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser User)
        {
            string unknownObj = getNextBlock();
            string WeaponID = getBlock(1);
            int Period = Convert.ToInt32(getBlock(4));

            int[] convertDays = new int[6] { 3, 7, 15, 30, 1, -1 };

            Item Item = ItemManager.getItem(WeaponID);
            if (Item != null)
            {
                if (Item.Buyable && Item.getPrice(Period) >= 0)
                {
                    if (Item.Premium && User.Premium <= 0)
                    {
                        User.send(new PACKET_ITEMSHOP(PACKET_ITEMSHOP.ErrorCodes.PremiumOnly, unknownObj));
                    }
                    else if (LevelCalculator.getLevelforExp(User.Exp) < Item.Level && User.Rank < 2)
                    {
                        User.send(new PACKET_ITEMSHOP(PACKET_ITEMSHOP.ErrorCodes.LevelLow, unknownObj));
                    }
                    else
                    {
                        int DinarResult = User.Dinar - Item.getPrice(Period);
                        if (DinarResult >= 0)
                        {
                            int InventorySlot = User.InventorySlots;
                            if (InventorySlot > 0)
                            {
                                User.AddItem(WeaponID, convertDays[Period], 1);
                                User.Dinar = DinarResult;
                                DB.runQuery("UPDATE users SET dinar='" + DinarResult + "' WHERE id=" + User.UserID);
                                User.Inventory = new InventoryItem[105];
                                User.LoadItems();
                                User.send(new PACKET_ITEMSHOP(User));
                                Log.AppendText(User.Nickname + " has bought [" + Item.Code.ToUpper() + "-" + Item.Name + "] for " + convertDays[Period] + "days.");
                            }
                            else
                            {
                                User.send(new PACKET_ITEMSHOP(PACKET_ITEMSHOP.ErrorCodes.InventoryFull, unknownObj));
                            }
                        }
                        else
                        {
                            User.send(new PACKET_ITEMSHOP(PACKET_ITEMSHOP.ErrorCodes.NotEnoughDinar, unknownObj));
                        }
                    }
                }
                else
                {
                    User.send(new PACKET_ITEMSHOP(PACKET_ITEMSHOP.ErrorCodes.CannotBeBougth, unknownObj));
                }
            }
            else
            {
                User.send(new PACKET_ITEMSHOP(PACKET_ITEMSHOP.ErrorCodes.CannotBeBougth, unknownObj));
            }
        }
    }
}