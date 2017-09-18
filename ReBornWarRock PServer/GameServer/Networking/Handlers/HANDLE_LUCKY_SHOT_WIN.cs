using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using ReBornWarRock_PServer.GameServer.Networking.Packets;

namespace ReBornWarRock_PServer.GameServer.Networking.Handlers
{
    class HANDLE_LUCKY_SHOT_WIN : PacketHandler
    {
        public string[] WeaponsOnLoose = new string[] { "DV01", "DS10", "DB18", "DA04", "DA09" };
        public override void Handle(ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser User)
        {
            try
            {
                //3777520 30258 0 DA08 3000 3 
                bool isDinar = (getBlock(0) == "0" ? true : false);
                string Item = getBlock(1);
                int PriceToPay = Convert.ToInt32(getBlock(2));
                int Bets = Convert.ToInt32(getBlock(3));
                int ItemWon = 1;
                if (isDinar && User.Dinar - PriceToPay >= 0 || isDinar == false && User.Cash - PriceToPay >= 0)
                {
                    string[] amountData = DB.runReadRow("SELECT amount FROM luckyshot WHERE itemcode='" + Item + "'");
                    string[] isInDatabase = DB.runReadRow("SELECT * FROM luckyshot WHERE itemcode='" + Item + "'");
                    if (isInDatabase.Length <= 0) User.disconnect();
                    int ItemAmount = Convert.ToInt32(amountData[0]);
                    int PercentageToWin = Bets;
                    if (amountData[0] == "0") return;
                    Random random = new Random();
                    switch (Bets)
                    {
                        case 1: Bets = 1; PercentageToWin = 10; break;
                        case 2: Bets = 1; PercentageToWin = 10; break;
                        case 3: Bets = 3; PercentageToWin = 9; break;
                        case 4: Bets = 3; PercentageToWin = 9; break;
                        case 5: Bets = 7; PercentageToWin = 8; break;
                        case 6: Bets = 7; PercentageToWin = 8; break;
                        case 7: Bets = 15; PercentageToWin = 7; break;
                        case 8: Bets = 15; PercentageToWin = 7; break;
                        case 9: Bets = 30; PercentageToWin = 6; break;
                        case 10: Bets = 30; PercentageToWin = 5; break;
                    }
                    int win = random.Next(0, PercentageToWin);
                    if (win > 1)
                    {
                        int weapon2 = random.Next(5);
                        Item = WeaponsOnLoose[weapon2];
                        ItemWon = 0;
                    }
                    if (ItemWon == 1)
                    {
                        ItemAmount -= 1;
                        foreach (ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser _Players in Managers.UserManager.getAllUsers())
                            _Players.send(new PACKET_LUCKY_SHOT(Item, ItemAmount));
                        DB.runQuery("UPDATE luckyshot SET amount=" + ItemAmount + " WHERE itemcode='" + Item + "'");
                    }
                    int InventorySlot = User.InventorySlots;
                    if (InventorySlot > 0)
                    {
                        User.AddItem(Item, Bets, 1);

                        if (isDinar)
                        {
                            User.Dinar -= PriceToPay;
                            DB.runQuery("UPDATE users SET dinar='" + User.Dinar + "' WHERE id='" + User.UserID + "'");
                        }
                        else
                        {
                            User.Cash -= PriceToPay;
                            User.reloadCash();
                            DB.runQuery("UPDATE users SET cash='" + User.Cash + "' WHERE id='" + User.UserID + "'");
                        }
                        User.LoadItems();
                        User.send(new Packets.PACKET_LUCKY_SHOT(User, Bets, Item, ItemWon, isDinar));
                    }
                    else
                    {
                        User.send(new PACKET_ITEMSHOP(PACKET_ITEMSHOP.ErrorCodes.InventoryFull, "NULL"));
                    }
                }
                else
                {
                    User.send(new PACKET_LUCKY_SHOT(PACKET_LUCKY_SHOT.ErrorCode.NotEnoughDinar));
                }
            }
            catch (Exception ex)
            {
                Log.AppendError(ex.Message);
            }
        }
    }
}