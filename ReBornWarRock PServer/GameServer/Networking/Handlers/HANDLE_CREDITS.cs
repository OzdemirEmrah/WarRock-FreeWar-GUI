using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ReBornWarRock_PServer.GameServer.Managers;
using ReBornWarRock_PServer.GameServer.Networking.Packets;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.User.Inventory;

namespace ReBornWarRock_PServer.GameServer.Networking.Handlers
{
    class HANDLE_CREDITS : PacketHandler
    {
        public override void Handle(ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser User)
        {
            try
            {
                int OPCode = Convert.ToInt32(getNextBlock());
                if (OPCode == 1113) // Item Shop Open
                {
                    User.reloadCash();
                    User.reloadOutBox();
                    User.reloadEquipment();
                    User.rebuildCostumeList(); 
                }
                else if (OPCode == 1111)
                {
                    string ItemCode = getBlock(4);
                    if (!User.hasItem(ItemCode)) User.disconnect();
                    if (ItemCode == "CB01")// Change Nick
                    {
                        string NewNickname = getBlock(5);
                        string[] checkUsedNick = DB.runReadRow("SELECT * FROM users WHERE nickname='" + NewNickname + "'");
                        if (checkUsedNick.Length == 0)
                        {
                            switch (User.Rank)
                            {
                                case 2: NewNickname = "[D]" + NewNickname; break;
                                case 3: NewNickname = "[MOD]" + NewNickname; break;
                                case 4: NewNickname = "[MOD]" + NewNickname; break;
                                case 5: NewNickname = "[GM]" + NewNickname; break;
                                case 6: NewNickname = "[GM]" + NewNickname; break;
                            }
                            Log.AppendText("---" + User.Nickname + " is now known as: " + NewNickname + "---");
                            User.Nickname = NewNickname;
                            User.send(new PACKET_CHANGE_NICKNAME(User, User.Nickname));
                            DB.runQuery("DELETE FROM inventory WHERE ownerid = '" + User.UserID + "' AND itemcode = 'CB01'");
                            DB.runQuery("UPDATE users SET nickname='" + NewNickname + "' WHERE id='" + User.UserID + "'");
                            User.Inventory = new InventoryItem[105];
                            User.send(new PACKET_DELETE_WEAPON(User, "CB01"));
                            User.LoadItems();
                            User.reloadCash();
                        }
                        else
                            User.send(new PACKET_CHANGE_NICKNAME());
                    }
                    else if (ItemCode == "CB03")
                    {
                        User.Kills = 0;
                        User.Deaths = 0;
                        DB.runQuery("UPDATE users SET kills='0', deaths='0' WHERE id='" + User.UserID + "'");
                        DB.runQuery("DELETE FROM inventory WHERE ownerid='" + User.UserID + "' AND itemcode='CB03'");
                        User.Inventory = new InventoryItem[105];
                        User.LoadItems();
                        User.send(new PACKET_KILLDEATH_USE(User));
                    }
                    else if (ItemCode == "CZ99" || ItemCode == "CB09" || ItemCode == "CB27" || ItemCode == "CC36" || ItemCode == "CC37" || ItemCode == "CC56" || ItemCode == "CC57")
                    {
                        //if (ItemCode == "CB09" && User.hasItem("CB08") == false) return;
                        DB.runQuery("DELETE FROM inventory WHERE ownerid = '" + User.UserID + "' AND itemcode = '" + ItemCode + "'");
                        int Rand = new Random().Next(0, 5);
                        string Code = null;
                        int Days = 1;

                        if (ItemCode == "CB09")
                        {
                            if (Rand == 0) { Code = "DU04"; Days = 7; }
                            else if (Rand == 1) { Code = "DF48"; Days = 15; }
                            else if (Rand == 2) { Code = "CD02"; Days = 15; }
                            else if (Rand == 3) { Code = "CD01"; Days = 15; }
                            else if (Rand == 4) { Code = "DF35"; Days = 30; }
                            else if (Rand == 5) { Code = "DC33"; Days = 30; }
                        }
                        else if (ItemCode == "CB27")
                        {
                            if (Rand == 0) { Code = "DF06"; Days = 15; }
                            else if (Rand == 1) { Code = "DF07"; Days = 15; }
                            else if (Rand == 2) { Code = "DG08"; Days = 15; }
                            else if (Rand == 3) { Code = "DC04"; Days = 15; }
                            else if (Rand == 4) { Code = "DT02"; Days = 15; }
                            else if (Rand == 5) { Code = "DG01"; Days = 15; }
                        }
                        else if (ItemCode == "CC36")
                        {
                            if (Rand == 0) { Code = "DU04"; Days = 7; }
                            else if (Rand == 1) { Code = "DF48"; Days = 15; }
                            else if (Rand == 2) { Code = "CD02"; Days = 15; }
                            else if (Rand == 3) { Code = "CD01"; Days = 15; }
                            else if (Rand == 4) { Code = "DF35"; Days = 30; }
                            else if (Rand == 5) { Code = "DC33"; Days = 30; }
                        }
                        else if (ItemCode == "CC37")
                        {
                            if (Rand == 0) { Code = "DU04"; Days = 7; }
                            else if (Rand == 1) { Code = "DF48"; Days = 15; }
                            else if (Rand == 2) { Code = "CD02"; Days = 15; }
                            else if (Rand == 3) { Code = "CD01"; Days = 15; }
                            else if (Rand == 4) { Code = "DF35"; Days = 30; }
                            else if (Rand == 5) { Code = "DC33"; Days = 30; }
                        }
                        else if (ItemCode == "CZ99")
                        {
                            int[] tableIDs = DB.runReadColumn("SELECT id FROM random_box_items;", 0, null);
                            int id = new Random().Next(1, tableIDs.Length); 
                            string[] Data = DB.runReadRow("SELECT * FROM `random_box_items` WHERE id=" + tableIDs[id]);
                            Code = Data[1];
                            Days = int.Parse(Data[2]);
                        }
                        else if (ItemCode == "CC56")
                        {
                            if (Rand == 0) { Code = "DU04"; Days = 7; }
                            else if (Rand == 1) { Code = "DF48"; Days = 15; }
                            else if (Rand == 2) { Code = "CD02"; Days = 15; }
                            else if (Rand == 3) { Code = "CD01"; Days = 15; }
                            else if (Rand == 4) { Code = "DF35"; Days = 30; }
                            else if (Rand == 5) { Code = "DC33"; Days = 30; }
                        }
                        else if (ItemCode == "CC57")
                        {
                            if (Rand == 0) { Code = "DU04"; Days = 7; }
                            else if (Rand == 1) { Code = "DF48"; Days = 7; }
                            else if (Rand == 2) { Code = "CD02"; Days = 15; }
                            else if (Rand == 3) { Code = "CD01"; Days = 15; }
                            else if (Rand == 4) { Code = "DF35"; Days = 30; }
                            else if (Rand == 5) { Code = "DC33"; Days = 30; }
                        }
                        User.AddOutBoxItem(Code, Days, 1);
                        User.Inventory = new InventoryItem[105];
                        User.LoadItems();
                        User.send(new PACKET_RANDOM_BOX(User, Code, Days));
                    }
                    else
                    {
                        Log.AppendError(" - Unknown ItemCode, blocks: " + string.Join(" ", getAllBlocks()));
                    }
                }
                else if (OPCode == 1110) // Buy with cash
                {
                    //1110 1110 2 3 0 750 DA03 0
                    int Period = Convert.ToInt32(getBlock(3));
                    int CashToPay = Convert.ToInt32(getBlock(5));
                    string ItemCode = getBlock(6);
                    int EACount = 0;
                    int[] convertDays = new int[6] { 3, 7, 15, 30, 1, -1 };

                    Item Item = ItemManager.getItem(ItemCode);
                    if (Item != null)
                    {
                        if (Item.Code == "CZ83" || Item.Code == "CZ84" || Item.Code == "CZ85")
                        {
                            if (Period == 0) EACount = 1;
                            if (Period == 1) EACount = 10;
                            if (Period == 2) EACount = 30;
                        }
                        int Price = Item.getCashPrice(Period);
                        if (User.Cash < Price)
                        {
                            User.send(new PACKET_ITEMSHOP(PACKET_ITEMSHOP.ErrorCodes.NotEnoughDinar, "NULL"));
                        }
                        else if (LevelCalculator.getLevelforExp(User.Exp) < Item.Level && User.Rank < 2)
                        {
                            User.send(new PACKET_ITEMSHOP(PACKET_ITEMSHOP.ErrorCodes.LevelLow, "NULL"));
                        }
                        else if (User.Cash > Price && Item.Buyable == true && Price >= 0)
                        {
                            int InventorySlot = User.InventorySlots;
                            if (InventorySlot > 0)
                            {
                                #region military
                                string[] military = { "CR39", "CR09", "CR11" };
                                foreach (string x in military)
                                {
                                    if (ItemCode == x)
                                    { User.AddOutBoxItem(ItemCode, -1, 1); break; };
                                }
                                #endregion

                                #region SendItem
                                switch (ItemCode)
                                {
                                    case "CB01":
                                        {
                                            User.AddOutBoxItem(ItemCode, -1, 1);
                                            break;
                                        }
                                    case "CB03":
                                        {
                                            User.AddOutBoxItem(ItemCode, -1, 1);
                                            break;
                                        }
                                    case "CB09":
                                        {
                                            User.AddOutBoxItem(ItemCode, -1, 1);
                                            break;
                                        }
                                    case "CZ83":
                                        {
                                            User.AddOutBoxItem(ItemCode, -1, EACount);
                                            break;
                                        }
                                    case "CZ84":
                                        {
                                            User.AddOutBoxItem(ItemCode, -1, EACount);
                                            break;
                                        }
                                    case "CZ85":
                                        {
                                            User.AddOutBoxItem(ItemCode, -1, EACount);
                                            break;
                                        }
                                    default:
                                        {
                                            User.AddOutBoxItem(ItemCode, convertDays[Period], EACount);
                                            //User.AddItem(ItemCode, convertDays[Period]);
                                            break;
                                        }
                                }
                                #endregion
                                User.Cash -= Price;
                                User.Inventory = new InventoryItem[105];
                                DB.runQuery("UPDATE users SET cash='" + User.Cash + "' WHERE id='" + User.UserID + "'");
                                User.LoadItems();
                                User.send(new PACKET_OUTBOX_SEND(User));
                                Log.AppendText(User.Nickname + " has bought [" + Item.Code.ToUpper() + "-" + Item.Name + "] for " + convertDays[Period] + "days.");
                            }
                            else
                            {
                                User.send(new PACKET_ITEMSHOP(PACKET_ITEMSHOP.ErrorCodes.InventoryFull, "NULL"));
                            }
                        }
                        else
                        {
                            User.send(new PACKET_ITEMSHOP(PACKET_ITEMSHOP.ErrorCodes.CannotBeBougth, "NULL"));
                        }
                    }
                    else
                    {
                        if (ItemCode == "CB02" || ItemCode == "CB53" || ItemCode == "CB54")
                        {
                            User.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Whisper, "SYSTEM >> This item is available only on Webshop!", User.SessionID, User.Nickname));
                        }
                        Log.AppendError(User.Nickname + " tried to buy: " + ItemCode + " but is not available yet.");
                        User.send(new PACKET_ITEMSHOP(PACKET_ITEMSHOP.ErrorCodes.CannotBeBougth, "NULL"));
                    }
                }
                else
                {
                    Log.AppendText("New Cash ErrCode: " + OPCode);
                    Log.AppendText(string.Join(" ", getAllBlocks()));
                }
            }
            catch (Exception ex)
            {
                Log.AppendError("MySQL Error: " + ex.Message);
            }
        }
    }
}

