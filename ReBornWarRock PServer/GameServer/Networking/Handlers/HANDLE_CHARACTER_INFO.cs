using System;
using ReBornWarRock_PServer.GameServer.Networking.Packets;
using ReBornWarRock_PServer.GameServer.Managers;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.User.Inventory;

namespace ReBornWarRock_PServer.GameServer.Networking.Handlers
{
    class HANDLE_CHARACTER_INFO : PacketHandler
    {
        public override void Handle(ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser User)
        {
            int UserID = Convert.ToInt32(getBlock(1));
            if (BanManager.isBlocked(UserID) == false)
            {
                string LoginName = getBlock(3);
                int AccessLevel = Convert.ToInt32(getBlock(8));
                string[] UserData = DB.runReadRow("SELECT id, username, nickname, exp, dinar, kills, deaths, premium, premiumExpire, cash, rank, coupons, todaycoupon, pc, clanID, clanrank, loginevent, logineventcheck, online, password, salt, country, infinity FROM users WHERE id='" + UserID + "' AND username='" + LoginName + "'");
                string[] Event = DB.runReadRow("SELECT userid, eventid FROM users_events WHERE userid='" + UserID + "'");

                if (UserData.Length > 0)
                {
                    if(Event.Length > 0)
                    if (Event[1] == "5") User.ReceivedRandomBox = true;
                    User.UserID = Convert.ToInt32(UserData[0]);
                    User.Username = UserData[1];
                    User.Nickname = UserData[2];
                    User.Exp = Convert.ToInt32(UserData[3]);
                    User.Dinar = Convert.ToInt32(UserData[4]);
                    User.Kills = Convert.ToInt32(UserData[5]);
                    User.Deaths = Convert.ToInt32(UserData[6]);
                    User.Premium = Convert.ToInt32(UserData[7]);
                    User.PremiumExpire = long.Parse(UserData[8]);
                    User.Cash = Convert.ToInt32(UserData[9]);
                    User.Rank = Convert.ToInt32(UserData[10]);
                    User.Coupons = Convert.ToInt32(UserData[11]);
                    User.TodayCoupon = Convert.ToInt32(UserData[12]);
                    User.PCItem = (Convert.ToInt32(UserData[13]) == 1) ? true : false;
                    User.PCItem1 = (Convert.ToInt32(UserData[13]) == 2) ? true : false;
                    User.PCItem2 = (Convert.ToInt32(UserData[13]) == 3) ? true : false;
                    User.ClanID = Convert.ToInt32(UserData[14]);
                    User.Country = UserData[21];
                    User.InfinityPremium = int.Parse(UserData[22]);

                    User.LoginCountry = Structure.LookupModule.getCountry(User.IPAddr);

                    if (User.Country == "")
                    {
                        DB.runQuery("UPDATE users SET country='" + User.LoginCountry.getCode() + "' WHERE id='" + User.UserID + "'");

                        DB.runQuery("UPDATE countrys SET count=count+1 WHERE code='" + User.LoginCountry.getCode() + "'");
                    }

                    string[] HeadshotData = DB.runReadRow("SELECT headshots FROM users WHERE id='" + User.UserID + "'");

                    User.Headshots = Convert.ToInt32(HeadshotData[0]);

                    if (User.Rank > 4) User.Nickname = "[GM]" + User.Nickname;
                    else if (User.Rank > 2) User.Nickname = "[MOD]" + User.Nickname;
                    else if (User.Rank == 2) User.Nickname = "[ESL]" + User.Nickname;

                    DB.runQuery("UPDATE users SET lasthwid='" + User.HWID + "' WHERE id='" + User.UserID + "'");
                    if (User.ClanID != -1)
                    {
                        User.ClanRank = Convert.ToInt32(UserData[15]);
                    }
                    User.LoginEvent = Convert.ToInt32(UserData[16]);
                    User.LoginEventCheck = Convert.ToInt32(UserData[17]);
                    if (Convert.ToInt32(UserData[18]) == 1)
                    {
                        Log.AppendError(User.Nickname + " tried to double login!");
                        DB.runQuery("UPDATE users SET online='0' WHERE id='" + User.UserID + "'");
                        User.disconnect();
                    }

                    for (int I = 0; I < User.EndGameWord.Length; I++)
                    {
                        User.EndGameWord[I] = "/";
                    }

                    if (User.Rank > 1) User.MaxSlots = 80;

                    if (Structure.Debug > 1 && User.Rank < 3)
                    {
                        User.send(new PACKET_CHAT(User, PACKET_CHAT.ChatType.Notice1, "NOTA: Stiamo facendo modifiche al server, attendere prego", User.SessionID, User.Nickname));
                        return;
                    }

                    User.uniqID = Convert.ToInt32(getBlock(5)); // 1
                    User.uniqID2 = Convert.ToInt32(getBlock(6)); // 0
                    User.uniqIDisCRC = Convert.ToInt32(getBlock(7)); // 910
                    string LastJoin = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    DB.runQuery("UPDATE users SET lasthwid='" + User.HWID + "', lastjoin='" + LastJoin + "' WHERE id='" + User.UserID + "'");



                    DateTime current = DateTime.Now;
                    int StartTime = Convert.ToInt32(String.Format("{0:yyMMddHH}", current));

                    string[] Skin = DB.runReadRow("SELECT class_0,class_1,class_2,class_3,class_4 FROM users_costumes WHERE ownerid='" + User.UserID + "'");
                    if (Skin.Length > 0)
                    {
                        User.CostumeE = Skin[0];
                        User.CostumeM = Skin[1];
                        User.CostumeS = Skin[2];
                        User.CostumeA = Skin[3];
                        User.CostumeH = Skin[4];
                    }
                    else
                        DB.runQuery("INSERT INTO users_costumes(ownerid) VALUES ('" + User.UserID + "')");

                    if (User.ClanID != -1)
                    {
                        string[] Clan = DB.runReadRow("SELECT clanname, iconid FROM clans WHERE id='" + User.ClanID + "'");
                        if (Clan.Length > 0)
                        {
                            User.Clan = ClanManager.getClan(User.ClanID);
                            User.ClanName = Clan[0];
                            User.ClanIconID = Convert.ToInt32(Clan[1]);
                        }
                        string[] checkClanUser = DB.runReadRow("SELECT * FROM users WHERE clanrank='2' AND clanid='" + User.ClanID + "'");
                        if (checkClanUser.Length == 0)
                        {
                            DB.runQuery("UPDATE users SET clanrank='-1', clanid='-1' WHERE clanid='" + User.ClanID + "'");
                            DB.runQuery("DELETE FROM clans WHERE id='" + User.ClanID + "'");
                        }
                    }
                    else
                    {
                        DB.runQuery("UPDATE users SET clanID='-1', clanRank='1' WHERE id='" + User.UserID + "'");
                    }

                    bool isEventMessage = false;

                    for (int i = 0; i < 5; i++)
                    {
                        User.cCodes6th[i] = "^";
                    }

                    User.LoadItems();

                    int[] checkItemIDs = DB.runReadColumn("SELECT id FROM inventory WHERE ownerid='" + UserID.ToString() + "' AND deleted='0'", 0, null);
                    for (int I = 0; I < checkItemIDs.Length; I++)
                    {
                        string[] itemData = DB.runReadRow("SELECT expiredate, itemcode FROM inventory WHERE id=" + checkItemIDs[I].ToString());
                        if (Convert.ToInt32(itemData[0]) < StartTime)
                        {
                            int InvItems = 0;
                            foreach (InventoryItem _InvItem in User.Inventory)
                            {
                                if (_InvItem != null) InvItems++;
                            }
                            if (InvItems > User.MaxSlots)
                            {
                                DB.runQuery("DELETE FROM inventory WHERE ownerid='" + UserID.ToString() + "'");
                                User.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> Your inventory's was resetted due too many items!", 999, "NULL"));
                                User.Inventory = new InventoryItem[105];
                            }

                            string itemID = User.getInventoryID(itemData[1]);

                            DB.runQuery("DELETE FROM inventory WHERE ownerid='" + UserID.ToString() + "' AND itemcode='" + itemData[1] + "'");
                            User.LeftItems.Add(itemData[1]);
                            User.ExpiredWeapon = true;
                        }
                    }

                    int[] checkCostumeIDs = DB.runReadColumn("SELECT id FROM inventory_costume WHERE ownerid='" + UserID.ToString() + "' AND deleted='0'", 0, null);
                    for (int I = 0; I < checkCostumeIDs.Length; I++)
                    {
                        string[] itemData = DB.runReadRow("SELECT expiredate, itemcode FROM inventory_costume WHERE id=" + checkCostumeIDs[I].ToString());
                        if (Convert.ToInt32(itemData[0]) < StartTime)
                        {
                            int InvItems = 0;
                            foreach (CostumeItem _InvItem in User.Costume)
                            {
                                if (_InvItem != null) InvItems++;
                            }
                            if (InvItems > User.MaxSlots)
                            {
                                DB.runQuery("DELETE FROM inventory_costume WHERE ownerid='" + UserID.ToString() + "'");
                                User.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> Your costume's was resetted due too many items!", 999, "NULL"));
                                User.Costume = new CostumeItem[105];
                            }
                            DB.runQuery("DELETE FROM inventory_costume WHERE ownerid='" + UserID.ToString() + "' AND itemcode='" + itemData[1] + "'");
                            User.LeftItems.Add(itemData[1]);
                            User.ExpiredWeapon = true;
                        }
                    }


                    // Check for expired weapons

                    if (User.ExpiredWeapon == true)
                    {
                        User.LoadEquipment();
                        int ItemCount = User.LeftItems.Count;
                        foreach (string sItem in User.LeftItems)
                        {
                            string inventoryID = User.getInventoryID(sItem);
                            for (int Class = 0; Class < 5; Class++)
                            {
                                for (int Slot = 0; Slot < 8; Slot++)
                                {
                                    if (User.Equipment[Class, Slot].Contains(inventoryID))
                                        User.Equipment[Class, Slot] = "^";
                                }
                            }
                        }
                        User.SaveEquipment();
                        User.send(new PACKET_EXPIRE_ITEM(User));
                        User.reloadEquipment(ItemCount);
                        User.Inventory = new InventoryItem[105];
                        User.LoadItems();
                    }

                    User.CheckForFirstLogin();

                    /*Login Event*/
                    int EventID = 6;
                    bool EventEnabled = false;
                    if (User.CheckForEvent(EventID) == false && EventEnabled)
                    {
                        int InvItems = User.InventorySlots;

                        if (InvItems > 0)
                        {
                            User.AddOutBoxItem("CB09", -1, 1);
                            DB.runQuery("INSERT INTO users_events (eventid, userid) VALUES ('" + EventID + "','" + User.UserID + "')");
                        }
                        else
                            User.send(new PACKET_ITEMSHOP(PACKET_ITEMSHOP.ErrorCodes.InventoryFull, "NULL"));
                    }
                    User.PremiumTimeLeft();
                    User.LoadRetails();
                    User.KillEventLoad2();

                    User.send(new PACKET_CLIENT_PACKET(0)); // Welcome sound
                    UserManager.addUser(User);
                    if (isEventMessage == true)
                    {
                        User.send(new PACKET_MESSAGE_BOX("Check_your_inventory!_:)"));
                    }
                    User.send(new PACKET_CREDITS(User));
                    User.send(new PACKET_CHARACTER_INFO(User));
                    User.send(new PACKET_PING(User));
                    User.reloadCash();
                }
            }
            else
            {
                User.send(new PACKET_CHARACTER_INFO(73030));
                User.disconnect();
            }
        }
    }
}