using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Data;
using System.Drawing;

using ReBornWarRock_PServer.GameServer.Managers;
using ReBornWarRock_PServer.GameServer.Networking;
using ReBornWarRock_PServer.GameServer.Networking.Packets;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.User.Inventory;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.Room;
using ReBornWarRock_PServer.GameServer.Networking.Handlers;

namespace ReBornWarRock_PServer.GameServer.Virtual_Objects.User
{
    class virtualMessenger
    {
        public int ID;
        public string Nickname;
        public int Status = 0;
        public int RequesterID = 0;
        public bool isOnline = false;

        public virtualMessenger(int ID, string Nickname, int Status, int RequesterID)
        {
            this.ID = ID;
            this.Nickname = Nickname;
            this.Status = Status;
            this.RequesterID = RequesterID;
            this.isOnline = false;

            virtualUser u = UserManager.getUser(this.ID);

            if (u != null)
            {
                this.isOnline = true;
            }
        }
    }
    class virtualUser
    {
        /*[BINGO]*/
        public int BingoNumber = -1;
        /*FreeWar*/
        public bool BackedToRoom = false;

        #region Escape
        public bool IsEscapeZombie = false;
        #endregion

        public bool IsPowerUp = false;
        public int LastRepairTick = 0;
        public int TotalWarSupport = 0;
        private bool KeepAliveThread = true;
        public bool InGame = false;
        public int UserID;
        public int SessionID;
        public int Rank;
        public Country LoginCountry;
        public int PersonalKill = 0;
        public int DoorDamageTime = 0;
        public int LastSuicideTick = 0;
        public int BossDamage = 0;
        public int SupplyTemp = -1;
        public bool ReceivedRandomBox = false;
        public int InfinityPremium = 0;
        public Color ColorChat = Color.Empty;

        public int preselected = -1;
        public bool BodyDefence = false;//test bodydefence

        public int RoomIDU;
        public int LastDieTick;
        public bool breaked = false;
        public int Headshots = 0;
        public int SpectatorID = -1;
        public int connectionId;
        public bool isSpectating = false;
        public bool sendPing = false;

        public int sessionStart { get; set; }
        public int heartBeatTime = -1;

        public string Nickname;
        public string Username;
        public string Country;
        public int Channel = 1;
        public int Page = 0;
        public int ClanID = -1;
        public long ClanIconID = -1;
        public int ClanRank = -1;
        public string ClanName = "NULL";
        public int VehNum = -1;
        public int ZombieHealth = 0;
        public int lastKillUser = -1;

        //test fix spawn//
        public Dictionary<String, int> Spawned =
                new Dictionary<String, int>();
        //-------------//

        public int Premium = 0;
        public long PremiumExpire = -1;

        public bool F5 = false;
        public int Cash = 0;
        public int Exp = 0;
        public int ExpUP = 0;
        public int DinarUP = 0;
        public int Dinar = 0;
        public int Health = 1000;
        public int Kills = 0;
        public int Plantings = 0;
        public int Rockets = 0;
        public bool Alive = false;
        public bool isSpawned = false;
        public int Granade = 0;
        public int killFromSpawn = 0;
        public int Deaths = 0;
        public int Class = 1;
        public string ClassCode = "-1";
        public int Weapon = 1;
        public int Coupons = 0;
        public int CountEvent = 0;
        public int TodayCoupon = 0;
        public int droppedFlash = 0;
        public int droppedM14 = 0;
        public int droppedAmmo = 0;
        public int playedsEventMap = 0;
        public int MaxSlots = 40;
        public bool AuthCheck = true;
        public bool GMMode = false;
        public bool PCItem = false;
        public bool PCItem1 = false;
        public bool PCItem2 = false;
        public long LastTimeStamp = 0;
        public int userListPage = 0;
        public int InvitationBy = -1;
        public int LoginEvent = -1;
        public int LoginEventCheck = 0;
        public long MutedTime = 0;
        public long LastChatTick = 0;
        public int ChatWarnings = 0;
        //godmode
        public int TheoreticalDeaths = 0;
        public bool GodMode = false;
        //
        public int InRadio = 0;
        //powcamp radio
        public int hackingBase = 0;
        public bool isHacking = false;
        public int hackTick = 0;
        public int hackPercentage = 0;
        public int LastHackBase = 0;
        public int LastHackTick = 0;
        public bool hasC4 = false;
        //
        //KillEvent
        int CurrentKills = 0;
        //EndeKillEvent

        public bool ExpiredWeapon = false;
        public string[] EndGameWord = new string[7];
        public DateTime pingServer = DateTime.Now;
        public string HWID = null;
        //Engineer
        public string CostumeE = "BA01,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^";
        //Medic
        public string CostumeM = "BA02,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^";
        //Sniper
        public string CostumeS = "BA03,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^";
        //Assault
        public string CostumeA = "BA04,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^";
        //Heavy
        public string CostumeH = "BA05,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^";

        public int SpawnProtection = 0;
        public Vehicle currentVehicle = null;
        public VehicleSeat currentSeat = null;
        public IPEndPoint RemoteNetwork = null;

        public ArrayList OutBoxItems = new ArrayList();
        public ArrayList LeftItems = new ArrayList();
        public Dictionary<int, virtualMessenger> Friends = new Dictionary<int, virtualMessenger>(); // now using dictionary
        public bool isMuted = false;

        ~virtualUser()
        {
            GC.Collect();
        }

        public string[] cCodes6th = new string[6];

        public void AddCostume(string Item, int Days)
        {
            try
            {
                if (Days == -1) Days = 99999;
                int InventorySlot = InventorySlots;
                if (InventorySlot > 0)
                {
                    if (hasCostume(Item))
                    {
                        //if user have item
                        CostumeItem bItem = null;
                        foreach (CostumeItem II in Costume)
                        {
                            if (II != null) { if (II._Code.ToLower() == Item.ToLower()) { bItem = II; } }
                        }

                        if (bItem != null)
                        {
                            DateTime ItemTime = DateTime.ParseExact(bItem._StartTime.ToString(), "yyMMddHH", null);
                            ItemTime = ItemTime.AddDays(Days);
                            long StartTime = long.Parse(String.Format("{0:yyMMddHH}", ItemTime));
                            bItem._StartTime = StartTime;
                            DB.runQuery("UPDATE inventory_costume SET expiredate='" + bItem._StartTime + "' WHERE ownerid='" + UserID + "' AND itemcode='" + Item + "'");

                        }
                    }
                    else
                    {
                        DateTime current = DateTime.Now;
                        current = current.AddDays(Days);
                        current = current.AddHours(-1);
                        int StartTime = Convert.ToInt32(String.Format("{0:yyMMddHH}", current));
                        DB.runQuery("INSERT INTO inventory_costume (`ownerid`, `expiredate`, `itemcode`, `deleted`) VALUES ('" + UserID.ToString() + "', '" + StartTime.ToString() + "', '" + Item + "', '0')");
                        Costume[InventorySlot] = new CostumeItem(StartTime, Item);
                    }
                }
                else
                    send(new PACKET_ITEMSHOP(PACKET_ITEMSHOP.ErrorCodes.InventoryFull, "null"));
            }
            catch (Exception ex)
            {
                Log.WriteDebug(ex.Message);
            }
        }

        /*FreeWar : SuicideTest*/
        public void Die()
        {
            Health = 0;
            isSpawned = false;

            if (Room.getSide(this) == 0)
            {
                Room.KillsDeberanLeft--;
            }
            else
            {
                Room.KillsNIULeft--;
            }

            /* if (Room.heromode != null)
             {
                 if (room.derbHeroUsr == roomslot)
                 {
                     room.derbHeroUsr = -1;
                     room.derbHeroKill--;
                 }
                 else if (room.niuHeroUsr == roomslot)
                 {
                     room.niuHeroUsr = -1;
                     room.niuHeroKill--;
                 }
                 room.heromode.CheckForNewRound();
             }*/

            if (isHacking)
            {
                isHacking = false;
                Room.send(new SP_RoomHackMission1(RoomSlot, (hackingBase == 0 ? Room.HackPercentageA : Room.HackPercentageB), 3, hackingBase));
            }

            if (hasC4)
            {
                Room.send(new SP_Unknown(29985, 0, 0, 1, 0, 0, 0, 0, 0)); // Remove C4 from the user 
                Room.PickuppedC4 = false;
                hasC4 = false;
                Room.send(new SP_Unknown(29985, 0, -1, 1, 5, -1, 0, -1, 0)); // Spawn the C4
            }

            /*if(room.HasChristmasMap)
            {
                send(new SP_KillCount(SP_KillCount.ActionType.Hide));
            }*/

            rDeaths++;
            rPoints++;
            LastDieTick = Structure.timestamp + 1;
            ClassCode = "-1";
        }
        /**/

        public void AddOutBoxItem(string ItemCode, int Days, int Count)
        {
            DB.runQuery("INSERT INTO outbox (ownerid, itemcode, days, count) VALUES ('" + UserID + "', '" + ItemCode + "', '" + Days + "', '" + Count + "')");
            OutBoxItems.Clear();
            int[] OutBoxIDs = DB.runReadColumn("SELECT id FROM outbox WHERE ownerid='" + UserID.ToString() + "'", 0, null);
            for (int I = 0; I < OutBoxIDs.Length; I++)
            {
                string[] itemData = DB.runReadRow("SELECT days, itemcode, count FROM outbox WHERE id=" + OutBoxIDs[I].ToString());
                OutboxItem Item = new OutboxItem();
                Item.ID = OutBoxIDs[I];
                Item.Days = Convert.ToInt32(itemData[0]);
                Item.ItemCode = itemData[1];
                Item.Count = int.Parse(itemData[2]);
                OutBoxItems.Add(Item);
            }
        }

        public int hasSmileBadge
        {
            get
            {
                return (hasItem("CK01") ? 0 : 1);
            }
        }


        public void KillEventLoad2()
        {
            string[] CheckEvent = DB.runReadRow("SELECT userid, kills FROM users_kill_events WHERE userid='" + UserID + "'");
            if (CheckEvent.Length > 0)
            {
                CurrentKills = Convert.ToInt32(CheckEvent[1]);
            }
            else // doesnt exists
            {
                DB.runQuery("INSERT INTO users_kill_events (userid, kills) values ('" + UserID + "', '0')");
            }

        }


        //MESSENGER
        public void LoadFriends()
        {
            Friends.Clear();
            try
            {
                int[] theFriends = DB.runReadColumn("SELECT * FROM friends WHERE id1='" + UserID + "' OR id2='" + UserID + "'", 0, null);
                for (int I = 0; I < theFriends.Length; I++)
                {
                    int FriendID = -1;
                    string[] DBFriend = DB.runReadRow("SELECT id1, id2, status, requesterid FROM friends WHERE id='" + theFriends[I] + "'");
                    if (DBFriend[0] == UserID.ToString())
                        FriendID = Convert.ToInt32(DBFriend[1]);
                    else
                        FriendID = Convert.ToInt32(DBFriend[0]);
                    string[] FriendData = DB.runReadRow("SELECT nickname FROM users WHERE id='" + FriendID + "'");
                    virtualMessenger MessengerUser = new virtualMessenger(theFriends[I], FriendData[0], Convert.ToInt32(DBFriend[2]), Convert.ToInt32(DBFriend[3]));
                    Friends.Add(FriendID, MessengerUser);
                }
            }
            catch { }
        }

        public void AddFriend(int uid, int requester, string FriendName)
        {
            try
            {
                if (!Friends.ContainsKey(uid) && uid != UserID)
                {
                    virtualMessenger MessengerUser = new virtualMessenger(uid, FriendName, 5, requester);
                    MessengerUser.isOnline = false;
                    Friends.Add(uid, MessengerUser);
                }
                else
                {
                    Log.AppendError(Nickname + " -> tried to add himself :|");
                }
            }
            catch { }
        }

        public void RemoveFriend(int uid)
        {
            if (Friends.ContainsKey(uid))
            {
                Friends.Remove(uid);
            }
        }

        public virtualMessenger getFriend(int id)
        {
            if (Friends.ContainsKey(id))
                return (virtualMessenger)Friends[id];
            return null;
        }

        public virtualMessenger getFriend(string nick)
        {
            foreach (virtualMessenger u in Friends.Values)
            {
                if (u.Nickname.ToLower() == nick.ToLower())
                    return u;
            }
            return null;
        }

        public void AddItem(string Item, int Days, int Count)
        {
            if (Days == -1)
                Days = 99999;
            int inventorySlots = this.InventorySlots;
            int num1 = 0;
            foreach (InventoryItem inventoryItem in this.Inventory)
            {
                if (inventoryItem != null)
                    ++num1;
            }
            int index = num1 + 1;
            this.LoadInventory();
            if (inventorySlots > 0)
            {
                if (this.hasItem(Item))
                {
                    InventoryItem inventoryItem1 = (InventoryItem)null;
                    foreach (InventoryItem inventoryItem2 in this.Inventory)
                    {
                        if (inventoryItem2 != null && inventoryItem2._Code.ToLower() == Item.ToLower())
                            inventoryItem1 = inventoryItem2;
                    }
                    if (inventoryItem1 != null)
                    {
                        if (ItemManager.getItem(Item).BuyType == 2)
                        {
                            string[] OldCount = DB.runReadRow("SELECT count FROM inventory WHERE ownerid='" + UserID + "' AND itemcode= '" + Item + "'");
                            int TotalCount = int.Parse(OldCount[0]) + Count;
                            DB.runQuery("UPDATE inventory SET count=" + TotalCount + " WHERE ownerid='" + this.UserID + "' AND itemcode='" + Item + "'");
                        }
                        else
                        {
                            DateTime dateTime = DateTime.ParseExact(inventoryItem1._StartTime.ToString(), "yyMMddHH", (IFormatProvider)null);
                            dateTime = dateTime.AddDays((double)Days);
                            long num2 = long.Parse(string.Format("{0:yyMMddHH}", dateTime));
                            inventoryItem1._StartTime = num2;
                            DB.runQuery("UPDATE inventory SET expiredate='" + inventoryItem1._StartTime + "' WHERE ownerid='" + this.UserID + "' AND itemcode='" + Item + "'");
                        }
                    }
                }
                else
                {
                    DateTime dateTime = DateTime.Now;
                    dateTime = dateTime.AddDays((double)Days);
                    dateTime = dateTime.AddHours(-1.0);
                    int num2 = int.Parse(string.Format("{0:yyMMddHH}", dateTime));
                    if (ItemManager.getItem(Item).BuyType == 2)
                        DB.runQuery("INSERT INTO inventory (`ownerid`, `expiredate`, `itemcode`, `deleted`, `count`) VALUES ('" + this.UserID.ToString() + "', '" + num2.ToString() + "', '" + Item + "', '0','" + Count + "')");
                    else
                        DB.runQuery("INSERT INTO inventory (`ownerid`, `expiredate`, `itemcode`, `deleted`, `count`) VALUES ('" + this.UserID.ToString() + "', '" + num2.ToString() + "', '" + Item + "', '0','" + Count + "')");
                    string[] OldCount = DB.runReadRow("SELECT count FROM inventory WHERE ownerid='" + UserID + "' AND itemcode= '" + Item + "'");
                    this.Inventory[index] = new InventoryItem((long)num2, Item, int.Parse(OldCount[0]) + Count);
                }
                this.Inventory = new InventoryItem[105];
                this.LoadItems();
                this.LoadInventory();
                this.reloadOutBox();
            }
            else
                this.send((Packet)new PACKET_ITEMSHOP(PACKET_ITEMSHOP.ErrorCodes.InventoryFull, "null"));
        }

        public void RemoveOutBoxItem(int ID)
        {
            try
            {
                foreach (OutboxItem Item in OutBoxItems)
                {
                    if (Item.ID == ID)
                    {
                        OutBoxItems.Remove(Item);
                        //this.OutBoxItems.Remove(ID);
                        //DB.runQuery("DELETE FROM outbox WHERE id='" + ID + "' AND ownerid='" + this.UserID + "'");
                    }
                    else return;
                }
            }
            catch (Exception ex)
            {
            }
        }

        public string getSlots()
        {
            string UserSlots = "F,F,F,F";
            string[] Slots = UserSlots.Split(new char[] { ',' });
            if (hasItem("CA01") || Premium >= 3)
                Slots[0] = "T";
            if (hasItem("DS05") || hasItem("DU04") || hasItem("DS10") || hasItem("DV01") || hasItem("DS01") || hasItem("DU05") || hasItem("DU01") || hasItem("DU02") || hasItem("DS03"))
                Slots[1] = "T";
            if (hasItem("CA03"))
                Slots[2] = "T";
            if (PCItem || hasItem("CA04"))
                Slots[3] = "T";
            UserSlots = string.Join(",", Slots);
            return UserSlots;
        }

        public void reloadOutBox()
        {
            OutBoxItems.Clear();

            int[] OutBoxIDs = DB.runReadColumn("SELECT id FROM outbox WHERE ownerid='" + UserID.ToString() + "'", 0, null);
            for (int I = 0; I < OutBoxIDs.Length; I++)
            {
                string[] itemData = DB.runReadRow("SELECT days, itemcode, count FROM outbox WHERE id=" + OutBoxIDs[I].ToString());
                OutboxItem Item = new OutboxItem();
                Item.ID = OutBoxIDs[I];
                Item.Days = Convert.ToInt32(itemData[0]);
                Item.ItemCode = itemData[1];
                Item.Count = int.Parse(itemData[2]);
                OutBoxItems.Add(Item);
            }
            send(new PACKET_OUTBOX_LIST(this));
        }

        public void reloadCash()
        {
            string[] _Cash = DB.runReadRow("SELECT cash FROM users WHERE id='" + UserID + "'");
            Cash = Convert.ToInt32(_Cash[0]);
            send(new PACKET_CREDITS(this));
        }

        public bool CheckForEvent(int EventID)
        {
            int EventCheck = DB.runRead("SELECT * FROM users_events WHERE userid='" + UserID + "' AND eventid='" + EventID + "'", null);
            if (EventCheck == 0)
            {
                return false;
            }
            return true;
        }

        public void reloadEquipment(int ItemCount = 1)
        {
            for (int I = 0; I < 5; I++)
            {
                for (int J = 0; J < 8; J++)
                {
                    string Code = Equipment[I, J];
                    if (Code.StartsWith("I") && Code != "^")
                    {
                        string AddPart = null;
                        if (Code.Contains("-") == false)
                        {
                            string[] SplitTheCode = Code.Split(Convert.ToChar("I"));
                            int InventoryID = Convert.ToInt32(SplitTheCode[1]);

                            if (InventoryID >= 100)
                                AddPart = "I";
                            else if (InventoryID >= 10)
                                AddPart = "I0";
                            else
                                AddPart = "I00";

                            InventoryID -= ItemCount;
                            if (InventoryID > 0)
                            {
                                if (InventoryID.ToString().Contains("-"))
                                    InventoryID = Convert.ToInt32(InventoryID.ToString().Replace("-", ""));
                                AddPart = AddPart + InventoryID;
                            }
                            else
                                AddPart = "^";
                        }
                        else
                        {
                            string[] SplitItems = Code.Split(Convert.ToChar("-"));
                            string[] SplitTheCodes1 = SplitItems[0].Split(Convert.ToChar("I"));
                            string[] SplitTheCodes2 = SplitItems[1].Split(Convert.ToChar("I"));

                            int InventoryID1 = Convert.ToInt32(SplitTheCodes1[1]);
                            int InventoryID2 = Convert.ToInt32(SplitTheCodes2[1]);

                            string Part1 = null;
                            string Part2 = null;

                            if (InventoryID1 >= 100)
                                Part1 = "I";
                            else if (InventoryID1 >= 10)
                                Part1 = "I0";
                            else
                                Part1 = "I00";


                            if (InventoryID2 >= 100)
                                Part2 = "I";
                            else if (InventoryID2 >= 10)
                                Part2 = "I0";
                            else
                                Part2 = "I00";

                            InventoryID1 -= ItemCount;
                            InventoryID2 -= ItemCount;
                            AddPart = Part1 + InventoryID1 + "-" + Part2 + InventoryID2;
                        }

                        Equipment[I, J] = AddPart;
                    }
                }
            }
        }

        public string rebuildCostumeList()
        {
            StringBuilder Costumes = new StringBuilder();
            int CostumeCount = 0;
            for (int I = 0; I < MaxSlots; I++)
            {
                if (Costume[I] != null)
                {
                    CostumeItem _Inv = Costume[I];
                    TimeSpan ExpireTime = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(_Inv._StartTime);

                    Costumes.Append(_Inv._Code.ToUpper() + "-3-3-" + _Inv._StartTime + "-0-0-0-0-0-9999-9999,");/*+ ExpireTime.TotalSeconds +*/
                    CostumeCount++;
                }
            }

            for (int I = 0; I < (MaxSlots - CostumeCount); I++)
            {
                Costumes.Append("^,");
            }
            return Costumes.ToString().Remove(Costumes.ToString().Length - 1);
        }

        public int CostumeSlots
        {
            get
            {
                int InvItems = 0;
                foreach (CostumeItem _InvItem in Costume)
                {
                    if (_InvItem != null) InvItems++;
                }
                int FinalResult = MaxSlots - InvItems;
                if (FinalResult <= 0) FinalResult = 0;
                return FinalResult;
            }
        }

        public int InventorySlots
        {
            get
            {
                int InvItems = 0;
                foreach (InventoryItem _InvItem in Inventory)
                {
                    if (_InvItem != null) InvItems++;
                }
                int FinalResult = MaxSlots - InvItems;
                if (FinalResult <= 0) FinalResult = 0;
                return FinalResult;
            }
        }

        public StringBuilder rebuildWeaponList()
        {
            int num = 0;
            StringBuilder stringBuilder = new StringBuilder();
            for (int index = 0; index < this.MaxSlots; ++index)
            {
                if (this.Inventory[index] != null)
                {
                    InventoryItem inventoryItem = this.Inventory[index];
                    TimeSpan timeSpan = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds((double)inventoryItem._StartTime);
                    if (ItemManager.getItem((inventoryItem._Code.ToUpper()).ToString()).BuyType == 2)
                    {
                        stringBuilder.Append(inventoryItem._Code.ToUpper() + "-3-0-" + inventoryItem._StartTime + "-" + inventoryItem._Count.ToString() + "-0-0-0-0-9999-9999,");
                        ++num;
                    }
                    else
                    {
                        stringBuilder.Append(string.Concat(new object[4]
                        {
             inventoryItem._Code.ToUpper(),
             "-3-0-",
             inventoryItem._StartTime,
             "-0-0-0-0-0-9999-9999,"
                        }));
                        ++num;
                    }
                }
            }
            for (int index = 0; index < this.MaxSlots - num; ++index)
                stringBuilder.Append("^,");
            return stringBuilder;
        }



        public void SaveEquipment()
        {
            for (int j = 0; j < 5; j++)
            {
                StringBuilder Items = new StringBuilder();
                for (int I = 0; I < 8; I++)
                {
                    Items.Append(Equipment[j, I] + ",");
                }
                DB.runQuery("UPDATE equipment SET class" + j + "='" + Items.ToString().Remove(Items.ToString().Length - 1) + "' WHERE ownerID=" + UserID.ToString());
            }
        }

        public void CheckForFirstLogin()
        {
            string[] checkFirstLogin = DB.runReadRow("SELECT firstlogin FROM users WHERE id='" + UserID.ToString() + "'");
            if (checkFirstLogin[0].Equals("2"))
            {
                AddItem("DB33", 7, 1);
                AddItem("DF05", 7, 1);
                AddItem("CN01", 7, 1);
                AddItem("DC03", 7, 1);
                AddItem("DG01", 7, 1);
                AddItem("DJ03", 7, 1);
                AddItem("CA01", 7, 1);
                send(new PACKET_SIGN_UP(this));
                DB.runQuery("UPDATE users SET firstlogin='1' WHERE id='" + UserID + "'");
            }
        }

        public void LoadRetails()
        {
            if (PCItem)
            {
                if (Equipment[0, 7] == "^")
                    Equipment[0, 7] = "D501";
                if (Equipment[1, 7] == "^")
                    Equipment[1, 7] = "D602";
                if (Equipment[2, 7] == "^")
                    Equipment[2, 7] = "DG13";
                if (Equipment[3, 7] == "^")
                    Equipment[3, 7] = "D801";
                if (Equipment[4, 7] == "^")
                    Equipment[4, 7] = "D902";
            }
            else
            {
                if (Equipment[0, 7] == "^")
                    Equipment[0, 7] = "DF02"; //Engineer - p90
                if (Equipment[1, 7] == "^")
                    Equipment[1, 7] = "DQ02"; //Medic - medic2
                if (Equipment[2, 7] == "^")
                    Equipment[2, 7] = "DB03"; //Sniper - mp5k
                if (Equipment[3, 7] == "^")
                    Equipment[3, 7] = "DO01"; //Assault - smoke
                if (Equipment[4, 7] == "^")
                    Equipment[4, 7] = "DB03"; //Heavy - mp5k
            }
        }

        public void LoadEquipment()
        {
            bool forcesave = false;
            string[] loadEquipment = DB.runReadRow("SELECT class0, class1, class2, class3, class4 FROM equipment WHERE ownerid='" + UserID + "'");
            for (int I = 0; I < 5; I++)
            {
                string[] slotItem = loadEquipment[I].Split(Convert.ToChar(","));
                for (int X = 0; X < slotItem.Length; X++)
                {
                    string inventoryIDCode = slotItem[X];

                    bool Checked = false;

                    if (inventoryIDCode.StartsWith("I"))
                    {
                        if (inventoryIDCode.Contains("-"))
                        {
                            string[] SplitItems = inventoryIDCode.Split(Convert.ToChar("-"));
                            string Code1 = getInventoryCode(SplitItems[0]);
                            string Code2 = getInventoryCode(SplitItems[1]);
                            if (hasItem(Code1) && hasItem(Code2))
                                Checked = true;
                        }
                        else
                            inventoryIDCode = getInventoryCode(slotItem[X]);
                    }

                    if (isDefaultWeapon(inventoryIDCode))
                        Equipment[I, X] = inventoryIDCode;
                    else if (Checked)
                        Equipment[I, X] = inventoryIDCode;
                    else if (hasItem(inventoryIDCode))
                        Equipment[I, X] = inventoryIDCode;
                    else
                    {
                        Equipment[I, X] = "^";
                        forcesave = true;
                    }
                }
            }

            if (forcesave)
                SaveEquipment();
        }

        public void LoadInventory()
        {
            for (int I = 0; I < Inventory.Length; I++)
                Inventory[I] = null;
            for (int I = 0; I < Costume.Length; I++)
                Costume[I] = null;

            int[] costumeIDs = DB.runReadColumn("SELECT id FROM inventory_costume WHERE ownerid ='" + UserID.ToString() + "' AND deleted='0'", 0, null);
            for (int I = 0; I < costumeIDs.Length; I++)
            {
                string[] itemData = DB.runReadRow("SELECT expiredate, itemcode FROM inventory_costume WHERE id=" + costumeIDs[I].ToString());
                Costume[I] = new CostumeItem(Convert.ToInt32(itemData[0]), itemData[1].ToLower());
            }

            int[] itemIDs = DB.runReadColumn("SELECT id FROM inventory WHERE ownerid ='" + UserID.ToString() + "' AND deleted='0'", 0, null);
            for (int I = 0; I < itemIDs.Length; I++)
            {
                string[] itemData = DB.runReadRow("SELECT expiredate, itemcode, count FROM inventory WHERE id=" + itemIDs[I].ToString());
                Inventory[I] = new InventoryItem(Convert.ToInt32(itemData[0]), itemData[1].ToLower(), int.Parse(itemData[2]));
            }
        }

        public void LoadItems()
        {
            try
            {
                DateTime current = DateTime.Now;
                int StartTime = Convert.ToInt32(String.Format("{0:yyMMddHH}", current));

                string[] Skin = DB.runReadRow("SELECT class_0,class_1,class_2,class_3,class_4 FROM users_costumes WHERE ownerid='" + UserID + "'");
                if (Skin.Length > 0)
                {
                    CostumeE = Skin[0];
                    CostumeM = Skin[1];
                    CostumeS = Skin[2];
                    CostumeA = Skin[3];
                    CostumeH = Skin[4];
                }
                else
                {
                    DB.runQuery("INSERT INTO users_costumes (ownerid) VALUES ('" + UserID + "')");
                }

                LoadInventory();

                string[] _CostumeE = CostumeE.Split(new char[] { ',' });
                string[] _CostumeM = CostumeM.Split(new char[] { ',' });
                string[] _CostumeS = CostumeS.Split(new char[] { ',' });
                string[] _CostumeA = CostumeA.Split(new char[] { ',' });
                string[] _CostumeH = CostumeH.Split(new char[] { ',' });

                if (hasCostume(_CostumeE[0]) == false) { CostumeE = "BA01,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^"; DB.runQuery("UPDATE users_costumes SET class_0='" + CostumeE + "' WHERE ownerid='" + UserID + "'"); }
                if (hasCostume(_CostumeM[0]) == false) { CostumeM = "BA02,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^"; DB.runQuery("UPDATE users_costumes SET class_1='" + CostumeM + "' WHERE ownerid='" + UserID + "'"); }
                if (hasCostume(_CostumeS[0]) == false) { CostumeS = "BA03,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^"; DB.runQuery("UPDATE users_costumes SET class_2='" + CostumeS + "' WHERE ownerid='" + UserID + "'"); }
                if (hasCostume(_CostumeA[0]) == false) { CostumeA = "BA04,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^"; DB.runQuery("UPDATE users_costumes SET class_3='" + CostumeA + "' WHERE ownerid='" + UserID + "'"); }
                if (hasCostume(_CostumeH[0]) == false) { CostumeH = "BA05,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^"; DB.runQuery("UPDATE users_costumes SET class_4='" + CostumeH + "' WHERE ownerid='" + UserID + "'"); }

                LoadEquipment();

                LoadRetails();

                int RetailID = DB.runRead("SELECT id FROM users_retails WHERE userid='" + UserID + "'", null);
                if (RetailID > 0)
                {
                    string[] RetailData = DB.runReadRow("SELECT class, code FROM users_retails WHERE id='" + RetailID.ToString() + "'");
                    if (Equipment[1, 7] == "^")
                    {
                        Equipment[1, 7] = "DQ02";
                    }
                    RetailClass = Convert.ToInt32(RetailData[0]);
                    Retail = RetailData[1];
                    Equipment[RetailClass, 7] = RetailData[1];
                }
            }
            catch (Exception)
            {
                //Log.AppendError("Error: " + ex.Message + " while loading item!");
            }
        }

        #region Equipment & Inventory
        public string[,] Equipment = new String[5, 8];
        public InventoryItem[] Inventory = new InventoryItem[100];
        public CostumeItem[] Costume = new CostumeItem[100];
        public string Retail = "";
        public int RetailClass = -1;
        public bool hasRetail() { return (Retail.Length > 0); }
        public bool hasRetail(string ItemCode) { return ItemCode.ToLower().Equals(Retail.ToLower()); }
        #endregion

        #region Room Information

        public virtualRoom Room = null;
        public virtualClan Clan = null;
        public int RoomSlot = 0;
        public bool isReady = false;
        public bool isReadyForNextRound = false;
        public int rKills = 0;
        public int rPoints = 0;
        public int rSkillPoints = 0;
        public int rDeaths = 0;
        public int rFlags = 0;
        public int ExpEarned = 0;
        public int TotalWarPoint = 0;
        public int DinarEarned = 0;
        public int EarnedPoints = 0;

        #endregion

        #region Peer2Peer
        private IPEndPoint _RemoteEndPoint;
        private IPEndPoint _LocalEndPoint;

        public long uniqID = 0L;
        public long uniqID2 = 0L;
        public int uniqIDisCRC = 0;

        public int iLocalPort = 0;
        public int iNetworkPort = 0;
        public long lLocalIP = 0;
        public long lNetworkIP = 0;

        #endregion

        public long PremiumTimeLeft()
        {
            if (InfinityPremium == 1) return 1;
            if (PremiumExpire > Structure.currTimeStamp)
            {
                return PremiumExpire - Structure.currTimeStamp;
            }
            else if (Premium > 0)
            {
                DB.runQuery("UPDATE users SET premium =  '0', premiumExpire =  '-1' WHERE id=" + UserID);
                Premium = 0;

                return -1;
            }
            return -1;
        }

        public void CouponLoop()
        {
            while (InGame && KeepAliveThread)
            {
                Thread.Sleep(1800000);
                if (TodayCoupon < 5)
                {
                    TodayCoupon++;
                    Coupons++;
                    DB.runQuery("UPDATE users SET coupons='" + Coupons + "', todaycoupon='" + TodayCoupon + "' WHERE id='" + UserID + "'");
                    send(new PACKET_COUPON_EVENT(TodayCoupon, Coupons));
                }
                if (TodayCoupon >= 5) KeepAliveThread = false;
            }
        }

        public void AuthorityCheck()
        {
            Thread.Sleep(45000);
            if (AuthCheck == false)
                disconnect();
        }

        public virtualUser(int SessionID, int SocketID, Socket uSocket)
        {
            try
            {
                this.SocketID = SocketID;
                this.uSocket = uSocket;
                this.SessionID = SessionID;

                //DB.runQuery("INSERT INTO log_connections (`timestamp`, `server`, `status`, `ip`, `host`) VALUES ('" + Program.currTimeStamp + "', '"+Config.SERVER_ID.ToString()+"', '0', '" + this.IPAddr + "', '" + this.Hostname + "');");

                #region Inventory System
                for (int I = 0; I < Inventory.Length; I++)
                {
                    Inventory[I] = null;
                }
                #endregion

                #region Equipment Reset System
                for (int Class = 0; Class < 5; Class++)
                {
                    for (int Slot = 0; Slot < 8; Slot++)
                    {
                        if (Slot == (int)Slots.Hands)
                            Equipment[Class, Slot] = "DA02"; //Knuckle
                        else if (Slot == (int)Slots.HandGun)
                            Equipment[Class, Slot] = "DB01"; //Colt
                        else if (Slot == (int)Slots.Weapon1)
                        {
                            switch (Class)
                            {
                                case (int)Classes.Engeneer:
                                case (int)Classes.Medic:
                                    Equipment[Class, Slot] = "DF01"; // MP7
                                    break;
                                case (int)Classes.Sniper:
                                    Equipment[Class, Slot] = "DG05";
                                    break;
                                case (int)Classes.Assault:
                                    Equipment[Class, Slot] = "DC02"; // K2
                                    break;
                                case (int)Classes.Heavy:
                                    Equipment[Class, Slot] = "DJ01";
                                    break;
                            }
                        }
                        else if (Slot == (int)Slots.Equipment)
                        {
                            switch (Class)
                            {
                                case (int)Classes.Engeneer:
                                    Equipment[Class, Slot] = "DR01"; // Spanner
                                    break;
                                case (int)Classes.Medic:
                                    Equipment[Class, Slot] = "DQ01"; // Medic Kit 1
                                    break;
                                case (int)Classes.Sniper:
                                case (int)Classes.Assault:
                                    Equipment[Class, Slot] = "DN01"; // Grenade
                                    break;
                                case (int)Classes.Heavy:
                                    Equipment[Class, Slot] = "DL01"; // Mine
                                    break;
                            }
                        }
                        else
                            Equipment[Class, Slot] = "^";
                    }
                }
                #endregion

                send(new PACKET_CONNECT());
                uSocket.BeginReceive(dataBuffer, 0, dataBuffer.Length, SocketFlags.None, new AsyncCallback(arrivedData), null);
                Thread _CouponThread = new Thread(CouponLoop);
                _CouponThread.Priority = ThreadPriority.AboveNormal;
                _CouponThread.Start();
                //Thread _LoginEventThread = new Thread(LoginThread);
                //_LoginEventThread.Priority = ThreadPriority.AboveNormal;
                //_LoginEventThread.Start();
                Thread _CheckThread = new Thread(AuthorityCheck);
                _CheckThread.Priority = ThreadPriority.AboveNormal;
                _CheckThread.Start();

            }
            catch (Exception ex)
            {
                Log.AppendError("Error setting up a new virtualUser: " + ex.Message);
            }
        }

        public bool hasCostume(string strCode)
        {
            try
            {
                foreach (CostumeItem II in Costume)
                {
                    if (strCode == "BA01" || strCode == "BA02" || strCode == "BA03" || strCode == "BA04" || strCode == "BA05") return true;
                    if (II != null) { if (II._Code.ToLower() == strCode.ToLower()) { return true; } }
                }
                return false;
            }
            catch { return false; }
        }

        public bool hasItem(string strCode)
        {
            if (strCode.StartsWith("I"))
            {
                for (int I = 0; I < Inventory.Length; I++)
                {
                    int inventoryID = -1;
                    if (strCode.StartsWith("I00"))
                        inventoryID = Convert.ToInt32(strCode.Substring(3));
                    else if (strCode.StartsWith("I0"))
                        inventoryID = Convert.ToInt32(strCode.Substring(2));
                    else
                        inventoryID = Convert.ToInt32(strCode.Substring(1));
                    InventoryItem Item = Inventory[I];
                    if (inventoryID == I && Item != null)
                    {
                        return true;
                    }
                }
                return false; ;
            }
            else
            {
                foreach (InventoryItem II in Inventory)
                {
                    if (II != null)
                    {
                        if (II._Code.ToLower() == strCode.ToLower()) return true;
                    }
                }
                return false;
            }        
        }

        #region Commands
        public bool isCommand(string Command)
        {
            try
            {
                string[] args = Command.Split(Convert.ToChar(0x20));

                switch (args[0].Substring(1).ToLower())
                {
                    case " ": { break; }
                    case "cmd":
                        {
                            // User CMD
                            send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> User CMD!", 999, "NULL"));
                            send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> /myinfo <USERNAME> (check your stats and kill events)", 999, "NULL"));

                            if (Rank == 5)
                            {
                                send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> Moderator CMD!", 999, "NULL"));
                                send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> /ban <USERNAME>", 999, "NULL"));
                                send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> /mute <USERNAME>", 999, "NULL"));
                                send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> /unmute <USERNAME>", 999, "NULL"));
                                send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> /map <NUMBER>", 999, "NULL"));
                                send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> /roominfo <ROOM NUMBER>", 999, "NULL"));
                                send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> /userinfo <USERNAME>", 999, "NULL"));
                                send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> /kickr <ROOM NUMER>", 999, "NULL"));
                                send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> /kick <USERNAME>", 999, "NULL"));
                            }
                            else if (Rank == 6)
                            {
                                send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> Administrator CMD!", 999, "NULL"));
                                send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> /ban <USERNAME>", 999, "NULL"));
                                send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> /mute <USERNAME>", 999, "NULL"));
                                send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> /unmute <USERNAME>", 999, "NULL"));
                                send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> /map <NUMBER>", 999, "NULL"));
                                send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> /roominfo <ROOM NUMBER>", 999, "NULL"));
                                send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> /userinfo <USERNAME>", 999, "NULL"));
                                send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> /kickr <ROOM NUMER>", 999, "NULL"));
                                send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> /kick <USERNAME>", 999, "NULL"));
                                send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> /kickall", 999, "NULL"));
                                send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> /clean", 999, "NULL"));
                                send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> /notice <MESSAGE>", 999, "NULL"));
                                send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> /extendtime", 999, "NULL"));
                                send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> /reload", 999, "NULL"));
                                send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> /endgame", 999, "NULL"));
                                send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> /uptime", 999, "NULL"));
                                send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> /gmmode", 999, "NULL"));
                                send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> /hwban <USERNAME>", 999, "NULL"));
                                send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> /hwid <USERNAME>", 999, "NULL"));
                                send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> /givecoupon <USERNAME> <AMOUNT>", 999, "NULL"));
                                send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> /givecash <USERNAME> <AMOUNT>", 999, "NULL"));
                                send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> /givecostume <USERNAME> <CODE> <DAYS>", 999, "NULL"));
                                send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> /giveitem <USERNAME> <CODE> <DAYS>", 999, "NULL"));
                                send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> /rdis <ROOM NUMBER>", 999, "NULL"));
                                send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> /setlevel <USERNAME> <LEVEL>", 999, "NULL"));
                                send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> /stop", 999, "NULL"));
                                send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> /event <MINUTES> <EXP>", 999, "NULL"));
                            }
                            return true;
                        }
                    case "myinfo":
                        {
                            foreach (virtualUser Client in UserManager.getAllUsers())
                            {
                                if (Client == null) continue;
                                if (Client.Nickname.ToLower().Equals(args[1].ToLower()) || Client.Username.ToLower().Equals(args[1].ToLower()))
                                {
                                    string rPermissions = "User";
                                    string rPremium = "Free2Play";
                                    string rChannel = "NONE";

                                    switch (Client.Rank)
                                    {
                                        case 0: rPermissions = "Banned"; break;
                                        case 1: rPermissions = "User"; break;
                                        case 2: rPermissions = "User"; break;
                                        case 3: rPermissions = "Donator"; break;
                                        case 4: rPermissions = "NONE"; break;
                                        case 5: rPermissions = "Moderator"; break;
                                        case 6: rPermissions = "Administrator"; break;
                                    }

                                    switch (Client.Channel)
                                    {
                                        case 1: rChannel = "Close Quarter Combat"; break;
                                        case 2: rChannel = "Battle Group"; break;
                                        case 3: rChannel = "A.I Mode"; break;
                                    }

                                    switch (Client.Premium)
                                    {
                                        case 0: rPremium = "Free2Play"; break;
                                        case 1: rPremium = "Bronze"; break;
                                        case 2: rPremium = "Silver"; break;
                                        case 3: rPremium = "Gold"; break;
                                        case 4: rPremium = "Platinum"; break;
                                    }
                                    send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> Hello " + Client.Nickname + ", here is all the information about you.", 999, "NULL"));

                                    send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> Kills: " + Client.Kills + ", Deaths: " + Client.Deaths, 999, "NULL"));
                                    send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> Dinars: " + Client.Dinar + ", Cash: " + Client.Cash, 999, "NULL"));
                                    send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> Level: " + LevelCalculator.getLevelforExp(Client.Exp) + " Premium: " + rPremium, 999, "NULL"));
                                    send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> Rank: " + rPermissions + " (" + Client.Rank + ")", 999, "NULL"));

                                    if (ConfigServer.KillEvent == 1)
                                    {
                                        send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> *********************", 999, "NULL"));
                                        send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >>            KILL EVENT", 999, "NULL"));
                                        send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> *********************", 999, "NULL"));
                                        send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> Count: " + CurrentKills + " / 10", 999, "NULL"));
                                    }
                                }
                            }
                            return true;
                        }
                    case "god":
                        {
                            if (Rank < 6) return true;
                            if (GodMode == false)
                            {
                                GodMode = true;
                                send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Whisper, "SYSTEM >> GodMode ON", SessionID, Nickname));

                            }
                            else
                            {
                                GodMode = false;
                                send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Whisper, "SYSTEM >> GodMode OFF", SessionID, Nickname));
                            }
                            return true;
                        }
                    case "powerup":
                        {
                            if (Rank < 6) return true;
                            if (IsPowerUp == false)
                            {
                                IsPowerUp = true;
                                send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Whisper, "SYSTEM >> IsPowerUp ON", SessionID, Nickname));

                            }
                            else
                            {
                                IsPowerUp = false;
                                send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Whisper, "SYSTEM >> IsPowerUp OFF", SessionID, Nickname));
                            }
                            return true;
                        }

                    case "kickall":
                        {
                            if (Rank < 6) return true;
                            foreach (virtualUser Players in UserManager.getAllUsers())
                            {
                                Players.disconnect();
                            }
                            return true;
                        }
                    case "break":
                        {
                            if (Rank < 6) return true;
                            this.Room.send(new PACKET_ZOMBIE_SPAWN(0, 0, int.Parse(args[1]), 0, 50000));
                            return true;
                        }
                    case "clean":
                        {
                            if (Rank < 6) return true;
                            Room.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> Room Removed!", 999, "NULL"));
                            Room.remove();
                            return true;
                        }
                    case "notice":
                        {
                            if (Rank < 3) return true;

                            foreach (virtualUser Player in UserManager.getAllUsers())
                            {
                                Player.send(new PACKET_CHAT(Player, PACKET_CHAT.ChatType.Notice1, " NOTICE: " + Command.Substring(7), 100, "Server"));
                            }

                            return true;
                        }
                    case "extendtime":
                        {
                            if (Rank < 3) return true;

                            if (Room != null)
                            {
                                if (Room.RoomStatus != 1)
                                {
                                    Room.RoundTimeLeft += Convert.ToInt32(args[1]) * 60000;
                                    Room.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> Round Time extended with " + Convert.ToInt32(args[1]).ToString() + " minutes!", 999, "NULL"));
                                }
                            }
                            return true;
                        }
                    case "reload":
                        {
                            if (Rank < 5) return true;
                            ItemManager.DecryptBinFile("item.bin");
                            ItemManager.LoadItems();
                            send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> Item Manager is been reloaded!", 999, "NULL"));
                            virtualMapData.Load();
                            send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> MapVehicle is been reloaded!", 999, "NULL"));
                            return true;
                        }
                    case "endgame":
                        {
                            if (Rank < 3) return true;

                            if (Room != null)
                            {
                                if (Room.RoomStatus != 1)
                                {
                                    Room.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> Ending Game!", 999, "NULL"));
                                    System.Threading.Thread.Sleep(2000);
                                    Room.endGame();
                                }
                            }
                            break;
                        }
                    case "uptime":
                        {
                            TimeSpan _StartTime = DateTime.Now - System.Diagnostics.Process.GetCurrentProcess().StartTime;
                            send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Whisper, "SYSTEM >> Online since " + _StartTime.Days + " days, " + _StartTime.Hours + " hours, " + _StartTime.Minutes + " minutes :)", SessionID, Nickname));
                            break;
                        }
                    case "gmmode":
                        {
                            if (Rank < 3) return false;
                            if (GMMode == true)
                            {
                                send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Whisper, "SYSTEM >> GM Mode turned off", SessionID, Nickname));
                                GMMode = false;
                            }
                            else if (GMMode == false)
                            {
                                send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Whisper, "SYSTEM >> GM Mode turned on", SessionID, Nickname));
                                GMMode = true;
                            }
                            return true;
                        }

                    case "inventory":
                        {
                            foreach (InventoryItem Item in Inventory)
                            {
                                if (Item != null)
                                    send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> " + Item._Code.ToUpper() + " - " + ItemManager.getItem(Item._Code).Name, 998, "SYSTEM"));
                            }
                            return true;
                        }
                    case "ban":
                        {
                            if (Rank < 5) return false;

                            foreach (virtualUser Client in UserManager.getAllUsers())
                            {
                                if (Client == null) continue;

                                if (Rank > Client.Rank)
                                {
                                    if (Client.Nickname.ToLower().Equals(args[1].ToLower()) || Client.Username.ToLower().Equals(args[1].ToLower()))
                                    {
                                        int BannedTime = 0;
                                        long Hours = Convert.ToInt32(args[2]);
                                        string Reason = "Unknown";
                                        if (args[3].Length > 0)
                                            Reason = args[3];
                                        if (Hours > 0)
                                        {
                                            DateTime _BanCuurent = DateTime.Now;
                                            _BanCuurent = _BanCuurent.AddHours(Hours);
                                            BannedTime = Convert.ToInt32(String.Format("{0:yyMMddHH}", _BanCuurent));
                                        }
                                        else
                                        {
                                            BannedTime = -1;
                                        }
                                        DB.runQuery("UPDATE users SET bantime='" + BannedTime + "', rank='0', banreason='" + Reason + "' WHERE id='" + Client.UserID + "'");
                                        Log.AppendText("---" + Nickname + " banned: " + Client.Nickname + "---");
                                        foreach (virtualUser Users in UserManager.getAllUsers())
                                            Users.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> " + Client.Nickname + " got banned from the server by " + Nickname + " for " + Convert.ToInt32(args[2]) + "hours with reason " + Reason + "!", 999, "NULL"));
                                        Client.disconnect();
                                        return true;
                                    }
                                }
                            }
                            send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> User " + args[1] + " is not online or doesn't exist!", 999, "NULL"));
                            return true;
                        }
                    case "hwban":
                        {
                            if (Rank < 6) return false;

                            foreach (virtualUser Client in UserManager.getAllUsers())
                            {
                                if (Client == null) continue;
                                if (Client.Nickname.ToLower().Equals(args[1].ToLower()) || Client.Username.ToLower().Equals(args[1].ToLower()))
                                {
                                    string[] HWIDCheck = DB.runReadRow("SELECT * FROM users_hwid WHERE hwid='" + Client.HWID + "'");
                                    if (HWIDCheck.Length > 0) return true;
                                    DB.runQuery("INSERT INTO users_hwid (`hwid`) VALUES('" + Client.HWID + "')");
                                    DB.runQuery("UPDATE users SET active='0', rank='0' WHERE id='" + Client.UserID + "'");
                                    Client.disconnect();
                                    return true;
                                }
                            }
                            send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> User " + args[1] + " is not online or doesn't exist!", 999, "NULL"));
                            return true;
                        }
                    case "mute":
                        {
                            if (Rank < 3) return false;

                            foreach (virtualUser Client in UserManager.getAllUsers())
                            {
                                if (Client == null) continue;

                                if (Client.Nickname.ToLower().Equals(args[1].ToLower()) || Client.Username.ToLower().Equals(args[1].ToLower()))
                                {
                                    if (Rank > Client.Rank)
                                    {
                                        foreach (virtualUser Players in UserManager.getAllUsers())
                                        {
                                            Players.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> User " + Client.Nickname + " has been muted by " + Nickname, 999, "NULL"));
                                        }
                                        Client.isMuted = true;
                                        return true;
                                    }
                                    else
                                    {
                                        send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> You cant mute " + Client.Nickname + " becaus he has an higer rank!", 999, "NULL"));
                                        return true;
                                    }
                                }
                            }
                            send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> User " + args[1] + " is not online or doesn't exist!", 999, "NULL"));
                            return true;
                        }
                    case "hwid":
                        {
                            if (Rank < 4) return false;

                            foreach (virtualUser Client in UserManager.getAllUsers())
                            {
                                if (Client == null) continue;
                                if (Client.Nickname.ToLower().Equals(args[1].ToLower()) || Client.Username.ToLower().Equals(args[1].ToLower()))
                                {
                                    send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> The HWID of: " + Client.Nickname + " is: " + Client.HWID, 999, "NULL"));
                                    return true;
                                }
                            }
                            send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> User " + args[1] + " is not online or doesn't exist!", 999, "NULL"));
                            return true;
                        }
                    case "givecoupon":
                        {
                            if (Rank < 5) return false;
                            foreach (virtualUser Client in UserManager.getAllUsers())
                            {
                                int _CouponToAdd = Convert.ToInt32(args[2]);
                                if (Client.Nickname.ToLower().Equals(args[1].ToLower()) || Client.Username.ToLower().Equals(args[1].ToLower()))
                                {
                                    Client.Coupons += _CouponToAdd;
                                    DB.runQuery("UPDATE users SET coupons='" + Client.Coupons + "' WHERE id='" + Client.UserID.ToString() + "'");
                                    send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> Sucessfully gaved " + _CouponToAdd + " coupons, to " + Client.Nickname + "!", 999, "NULL"));
                                    Client.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> You received " + _CouponToAdd + " coupons, from" + Nickname + "!", 999, "NULL"));
                                    return true;
                                }
                            }
                            send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> User is not online or doesn't exist!!", 999, "NULL"));
                            return true;
                        }
                    case "givecash":
                        {
                            if (Rank < 5) return false;
                            foreach (virtualUser Client in UserManager.getAllUsers())
                            {
                                int _Cash = Convert.ToInt32(args[2]);
                                if (Client.Nickname.ToLower().Equals(args[1].ToLower()) || Client.Username.ToLower().Equals(args[1].ToLower()) && Rank > 4)
                                {
                                    Client.Cash += _Cash;
                                    DB.runQuery("UPDATE users SET cash='" + Client.Cash + "' WHERE id='" + Client.UserID.ToString() + "'");
                                    Log.AppendText("---" + Nickname + " gaved: " + _Cash + " cashs to: " + Client.Nickname + "---");
                                    send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> Sucessfully gaved " + _Cash + " cash, to " + Client.Nickname + "!", 999, "NULL"));
                                    Client.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> You received " + _Cash + " cash, from " + Nickname + "!", 999, "NULL"));
                                    return true;
                                }
                            }
                            send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> User is not online or doesn't exist!!", 999, "NULL"));
                            return true;
                        }
                    case "givecostume":
                        {
                            if (Rank < 5) return false;
                            foreach (virtualUser Client in UserManager.getAllUsers())
                            {
                                string ItemCode = args[2];
                                int Days = Convert.ToInt32(args[3]);
                                if (Client.Nickname.ToLower().Equals(args[1].ToLower()) || Client.Username.ToLower().Equals(args[1].ToLower()))
                                {
                                    int InventorySlot = Client.CostumeSlots;
                                    if (InventorySlot > 0)
                                    {
                                        Client.AddCostume(ItemCode, Days);
                                        Client.LoadItems();
                                        Log.AppendText("---" + Nickname + " gaved: " + ItemCode + " to: " + Client.Nickname + " for: " + Days + " days!" + "---");
                                        send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> Sucessfully gaved " + ItemCode + ", for " + Days + " days to " + Client.Nickname + "!", 999, "NULL"));
                                        Client.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> You received " + ItemCode + ", for " + Days + " days from " + Nickname + "!", 999, "NULL"));
                                        return true;
                                    }
                                    else
                                    {
                                        send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> User has not other inventory slot!", 999, "NULL"));
                                        return true;
                                    }
                                }
                            }
                            send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> User is not online or doesn't exist!!", 999, "NULL"));
                            return true;
                        }
                    case "giveitem":
                        {
                            if (Rank < 5) return false;
                            foreach (virtualUser Client in UserManager.getAllUsers())
                            {
                                string ItemCode = args[2].ToUpper();
                                int Days = Convert.ToInt32(args[3]);
                                if (Client.Nickname.ToLower().Equals(args[1].ToLower()) || Client.Username.ToLower().Equals(args[1].ToLower()))
                                {
                                    int InventorySlot = Client.InventorySlots;
                                    if (InventorySlot > 0)
                                    {
                                        Client.AddOutBoxItem(ItemCode, Days, 1);
                                        Client.LoadItems();
                                        Log.AppendText("---" + Nickname + " gaved: " + ItemCode + " to: " + Client.Nickname + " for: " + Days + " days!" + "---");
                                        send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> Sucessfully gaved: " + ItemCode + ", for: " + Days + " days in his/her outbox!", 999, "NULL"));
                                        Client.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> You received: " + ItemCode + ", for: " + Days + " days from: " + Nickname + ", look your outbox!!", 999, "NULL"));
                                        return true;
                                    }
                                    else
                                    {
                                        send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> User has not other inventory slot!", 999, "NULL"));
                                        return true;
                                    }
                                }
                            }
                            send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> User is not online or doesn't exist!!", 999, "NULL"));
                            return true;
                        }
                    case "map":
                        {
                            if (Rank < 3) return false;
                            if (Room == null && Room.RoomStatus == 1) return true;
                            Room.MapID = Convert.ToInt32(args[1]);
                            foreach (virtualUser User in Room.Players)
                                User.send(new SP_ROOM_INFO(51, Room.RoomMasterSlot, 55, 2, 1, 0, Room.MapID, 0, 0, 0, 0, 0, 0, 0));
                            foreach (virtualUser SpectatorUser in Room.Spectators)
                                SpectatorUser.send(new SP_ROOM_INFO(51, Room.RoomMasterSlot, 55, 2, 1, 0, Room.MapID, 0, 0, 0, 0, 0, 0, 0));
                            return true;
                        }
                    case "event":
                        {
                            if (Rank < 5) return false;
                            int Minute = Convert.ToInt32(args[1]);
                            Structure.EventTime = Minute * 60;
                            Structure.EXPEvent = (Convert.ToInt32(args[2]) / 100);
                            Structure.DinarEvent = (Convert.ToInt32(args[2]) / 100);
                            if (Convert.ToInt32(args[1]) != -1)
                            {
                                Structure.isEvent = true;
                                Structure.EXPBanner = 1;
                            }
                            else
                            {
                                Structure.isEvent = false;
                                Structure.EventTime = -1;

                            }
                            foreach (virtualUser Player in UserManager.getAllUsers())
                            {
                                Player.send(new PACKET_PING(Player));
                                Player.send(new PACKET_EVENT_MESSAGE(PACKET_EVENT_MESSAGE.EventCodes.EXP_Activate));
                            }
                            return true;
                        }
                    case "eventend":
                        {
                            if (Rank < 5) return false;

                            Structure.isEvent = false;
                            Structure.EventTime = -1;
                            Structure.EXPBanner = 0;

                            foreach (virtualUser Player in UserManager.getAllUsers())
                            {
                                Player.send(new PACKET_PING(Player));
                                Player.send(new PACKET_EVENT_MESSAGE(PACKET_EVENT_MESSAGE.EventCodes.EXP_Deactivate));
                            }
                            return true;
                        }
                    case "rdis":
                        {
                            if (Rank < 3) return false;
                            int RoomToClose = Convert.ToInt32(args[1]);
                            virtualRoom TargetRoom = RoomManager.getRoom(Channel, Convert.ToInt32(args[1]));
                            if (TargetRoom != null)
                            {
                                foreach (virtualUser RoomUser in TargetRoom.Players)
                                {
                                    RoomUser.send(new PACKET_LEAVE_ROOM(RoomUser, TargetRoom, RoomUser.RoomSlot, TargetRoom.RoomMasterSlot));
                                    RoomUser.Room = null;
                                }
                                TargetRoom.remove();
                                RoomManager.removeRoom(Channel, Convert.ToInt32(args[1]));
                                foreach (virtualUser _User in UserManager.getUsersInChannel(TargetRoom.Channel, false))
                                {
                                    if (_User.Page == Math.Floor(Convert.ToDecimal(TargetRoom.ID / 14)))
                                    {
                                        _User.send(new PACKET_ROOMLIST_UPDATE(TargetRoom, 2));
                                    }
                                }
                                return true;
                            }
                            send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> This room doesn't exist!!", 999, "NULL"));
                            return true;

                        }
                    case "setlevel":
                        {
                            if (Rank < 6) return false;
                            foreach (virtualUser Client in UserManager.getAllUsers())
                            {
                                if (Client.Nickname.ToLower().Equals(args[1].ToLower()) || Client.Username.ToLower().Equals(args[1].ToLower()))
                                {
                                    int _UserID = Client.UserID;
                                    long _Level = LevelCalculator.getExpForLevel(Convert.ToInt32(args[2]));
                                    Client.disconnect();
                                    DB.runQuery("UPDATE users SET exp='" + _Level + "' WHERE id='" + Client.UserID + "'");
                                    return true;
                                }
                            }

                            send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> User " + args[1] + " is not online or doesn't exist!", 999, "NULL"));
                            return true;
                        }
                    case "roominfo":
                        {
                            if (Rank < 4) return false;
                            int RoomID = Convert.ToInt32(args[1]);
                            virtualRoom TargetRoom = RoomManager.getRoom(Channel, RoomID);
                            if (TargetRoom == null) return true;
                            send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> Here all information about room N° " + RoomID, 999, "NULL"));
                            send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> Room Name: " + TargetRoom.Name, 999, "NULL"));
                            send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> Room Status: " + (TargetRoom.RoomStatus == 2 ? "Play" : "Wait"), 999, "NULL"));
                            send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> Password: " + TargetRoom.Password + " / MapID: " + TargetRoom.MapID, 999, "NULL"));
                            send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> Type: " + TargetRoom.RoomType + " / Mode: " + TargetRoom.Mode, 999, "NULL"));
                            send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> Players: " + TargetRoom.Players.Count + "/" + TargetRoom.MaxPlayers + ", Spectators " + TargetRoom.Spectators.Count + "/10", 999, "NULL"));
                            return true;
                        }
                    case "userinfo":
                        {
                            if (Rank < 4) return false;

                            foreach (virtualUser Client in UserManager.getAllUsers())
                            {
                                if (Client == null) continue;
                                if (Client.Nickname.ToLower().Equals(args[1].ToLower()) || Client.Username.ToLower().Equals(args[1].ToLower()))
                                {
                                    string rPermissions = "User";
                                    string rPremium = "Free2Play";
                                    string rChannel = "NONE";
                                    switch (Client.Rank)
                                    {
                                        case 0: rPermissions = "Banned"; break;
                                        case 1: rPermissions = "User"; break;
                                        case 2: rPermissions = "User"; break;
                                        case 3: rPermissions = "Moderator"; break;
                                        case 4: rPermissions = "Moderator"; break;
                                        case 5: rPermissions = "Administrator"; break;
                                        case 6: rPermissions = "Administrator / Leader"; break;
                                    }
                                    switch (Client.Channel)
                                    {
                                        case 1: rChannel = "Close Quarter Combat"; break;
                                        case 2: rChannel = "Battle Group"; break;
                                        case 3: rChannel = "A.I Mode"; break;
                                    }
                                    switch (Client.Premium)
                                    {
                                        case 0: rPremium = "Free2Play"; break;
                                        case 1: rPremium = "Bronze"; break;
                                        case 2: rPremium = "Silver"; break;
                                        case 3: rPremium = "Gold"; break;
                                        case 4: rPremium = "Platinum"; break;
                                    }
                                    send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> Here all information about: " + Client.Nickname + " with ID: " + Client.UserID, 999, "NULL"));
                                    send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> UserID: " + Client.Username + ", Nickname: " + Client.Nickname, 999, "NULL"));
                                    send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> Kills: " + Client.Kills + ", Deaths: " + Client.Deaths, 999, "NULL"));
                                    send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> Dinars: " + Client.Dinar + ", Cash: " + Client.Cash, 999, "NULL"));
                                    send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> Level: " + LevelCalculator.getLevelforExp(Client.Exp) + ", Rank: " + Client.Rank, 999, "NULL"));
                                    if (Client.Room != null)
                                        send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> Room: " + Client.Room.ID, 999, "NULL"));
                                    send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> Actual Channel: " + rChannel, 999, "NULL"));
                                    send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> Premium: " + rPremium + ", PremiumID: " + Client.Premium, 999, "NULL"));
                                    send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> Rank: " + Client.Rank, 999, "NULL"));
                                    send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> Permissions: " + rPermissions, 999, "NULL"));
                                    send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> HWID: " + Client.HWID + ", IP: " + Client.IPAddr, 999, "NULL"));
                                    return true;
                                }
                            }
                            send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> User " + args[1] + " is not online or doesn't exist!", 999, "NULL"));
                            return true;
                        }
                    case "unmute":
                        {
                            if (Rank < 4) return false;

                            foreach (virtualUser Client in UserManager.getAllUsers())
                            {
                                if (Client == null) continue;

                                if (Client.Nickname.ToLower().Equals(args[1].ToLower()) || Client.Username.ToLower().Equals(args[1].ToLower()))
                                {
                                    if (Rank > Client.Rank)
                                    {
                                        if (Client.isMuted != false)
                                        {
                                            foreach (virtualUser Players in UserManager.getAllUsers())
                                            {
                                                Players.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> User " + Client.Nickname + " has been unmuted by " + Nickname + "!", 999, "NULL"));
                                            }
                                            Client.isMuted = false;
                                            Client.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Whisper, "SYSTEM >> You Have been unmuted by " + Nickname + "!", 12, "SYSTEM"));
                                            return true;
                                        }
                                        else
                                        {
                                            send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> User " + Client.Nickname + " is not muted!", 999, "NULL"));
                                        }
                                    }
                                    else
                                    {
                                        send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> You cant unmute " + Client.Nickname + " becaus he has an higer rank!", 999, "NULL"));
                                        return true;
                                    }
                                }
                            }
                            send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> User " + args[1] + " is not online or doesn't exist!", 999, "NULL"));
                            return true;
                        }
                    case "kickr":
                        {
                            if (Rank < 3) return false;
                            foreach (virtualUser Client in UserManager.getAllUsers())
                            {
                                if (Client == null) continue;

                                if (Client.Nickname.ToLower().Equals(args[1].ToLower()) || Client.Username.ToLower().Equals(args[1].ToLower()))
                                {
                                    if (Client.Room == null) return true;
                                    Client.Room.RemoveUser(Client.RoomSlot);
                                    return true;
                                }
                            }
                            send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> User " + args[1] + " is not online or doesn't exist!", 999, "Server"));
                            return true;
                        }
                    case "colorlist":
                        {
                            if (Rank < 3) return false;

                            send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Whisper, "SYSTEM >> http://www.colorpicker.com/ ", 999, "Server"));

                            return true;
                        }

                    case "kick":
                        {
                            if (Rank < 3) return false;

                            foreach (virtualUser Client in UserManager.getAllUsers())
                            {
                                if (Client == null) continue;

                                if (Client.Nickname.ToLower().Equals(args[1].ToLower()) || Client.Username.ToLower().Equals(args[1].ToLower()))
                                {
                                    if (Rank >= Client.Rank)
                                    {
                                        foreach (virtualUser Serv in UserManager.getAllUsers())
                                        {
                                            Serv.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> User " + Client.Nickname + " got kicked from the server by " + Nickname + "!", 999, "Server"));
                                        }
                                        Client.disconnect();
                                        return true;
                                    }
                                    else
                                    {
                                        send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> You cant kick " + Client.Nickname + " becaus he has an higer rank!", 999, "Server"));
                                        return true;
                                    }
                                }
                            }
                            send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> User " + args[1] + " çevrimiçi değil veya yok!", 999, "Server"));
                            return true;
                        }

                    default: { return false; }
                }
            }
            catch { return false; }

            return true;
        }
        #endregion

        #region Networking

        public int iSocketID { get { return SocketID; } }

        private int SocketID;
        public Socket uSocket;
        private byte[] dataBuffer = new byte[1024];
        private bool isDisconnected = false;
        public bool pingOK = false;
        public long Ping = 0;

        public long nIP = 0;
        public int nPort = 0;
        public long lIP = 0;
        public int lPort = 0;
        public int rHeadshots;

        public int PortToInt(int Port)
        {
            try
            {
                byte[] PortBytes = BitConverter.GetBytes(Port);
                byte[] PortBytesNew = new byte[2] { PortBytes[1], PortBytes[0] };
                ushort newPort = BitConverter.ToUInt16(PortBytesNew, 0);

                return newPort;
            }
            catch (Exception ex)
            {
                Log.AppendText("Error on PortToInt: " + ex.Message);
                return -1;
            }
        }

        public String IPAddr { get { return uSocket.RemoteEndPoint.ToString().Split(':')[0]; } }
        public String Hostname { get { return Dns.GetHostEntry(this.IPAddr).HostName; } }

        public string ReverseIP(string tString)
        {
            try
            {
                string[] bString = tString.Split(new char[] { '.' });
                string tNew = "";
                for (int i = (bString.Length - 1); i > -1; i--)
                    tNew += bString[i] + ".";
                return tNew.Substring(0, tNew.Length - 1);
            }
            catch (Exception ex)
            {
                Log.AppendText("Error on ReverseIP: " + ex.Message);
                return null;
            }
        }

        public long IPToInt(string addr)
        {
            try
            {
                return (long)(uint)IPAddress.NetworkToHostOrder((int)IPAddress.Parse(ReverseIP(addr)).Address);
            }
            catch (Exception ex)
            {
                Log.AppendText("Error on IPToInt: " + ex.Message);
                return -1;
            }
        }
        public void SetUpNetwork(IPEndPoint GroupEP, byte UDPId)
        {
            try
            {
                nIP = IPToInt(GroupEP.Address.ToString());
                nPort = PortToInt(GroupEP.Port);
                Log.WriteDebug(string.Concat(new object[] { "Setting Up Network! UDPID: ", UDPId, " (nIP: ", this.lNetworkIP, ":", this.iNetworkPort, " lIP: ", this.lLocalIP, ":", this.iLocalPort }));
            }
            catch (Exception ex)
            {
                Log.AppendError("Error on SetUpNetwork: " + ex.Message);
            }
        }

        public bool isDefaultWeapon(string Weapon)
        {
            if (Weapon == "DF01" || Weapon == "DQ01" || Weapon == "DR01" || hasRetail(Weapon) || (Weapon == "DF10" || Weapon == "DF06" || Weapon == "DG08" || Weapon == "DC04" || Weapon == "DH02") || Weapon == "DN01" || Weapon == "DC02" || Weapon == "DG05" || Weapon == "DB01" || Weapon == "DL01" || Weapon == "DJ01" || Weapon == "DA02")
                return true;
            return false;
        }

        public string getInventoryID(string WeaponCode)
        {
            string sString = null;
            for (int I = 0; I < Inventory.Length; I++)
            {
                InventoryItem Item = Inventory[I];
                if (Item != null)
                {
                    if (Item._Code.ToUpper() == WeaponCode.ToUpper())
                    {
                        if (I >= 100)
                            sString = "I" + I;
                        else if (I >= 10)
                            sString = "I0" + I;
                        else
                            sString = "I00" + I;
                        return sString;
                    }
                }
            }
            return sString;
        }

        public int getEAItems(string Code)
        {
            string[] strArray = DB.runReadRow("SELECT count, itemcode FROM inventory WHERE ownerid='" + this.UserID + "' AND itemcode='" + Code + "'");
            if (strArray.Length > 0 && ItemManager.getItem(strArray[1]).BuyType == 2)
                return int.Parse(strArray[0]);
            else
                return 0;
        }

        public void DeleteItem(string ItemCode, int count)
        {
            try
            {
                string[] strArray = DB.runReadRow("SELECT itemcode, count FROM inventory WHERE ownerid='" + this.UserID + "' AND itemcode='" + ItemCode + "'");
                if (strArray.Length <= 0)
                    return;
                if (ItemManager.getItem(ItemCode).BuyType == 2 && int.Parse(strArray[1]) > 1)
                {
                    int num = int.Parse(strArray[1]) - count;
                    if (num <= 0)
                        return;
                    DB.runQuery("UPDATE inventory SET count='" + num + "' WHERE itemcode='" + ItemCode + "' AND ownerid='" + this.UserID + "'");
                    this.Inventory = new InventoryItem[105];
                    this.LoadItems();
                }
                else
                {
                    DB.runQuery("DELETE FROM inventory WHERE ownerid = '" + this.UserID + "' AND itemcode = '" + ItemCode + "'");
                    this.Inventory = new InventoryItem[105];
                    this.LoadItems();
                }
            }
            catch (Exception ex)
            {
                Log.AppendError(ex.Message);
            }
        }

        public string getInventoryCode(string ID)
        {
            for (int I = 0; I < Inventory.Length; I++)
            {
                int inventoryID = -1;
                if (ID.StartsWith("I00"))
                    inventoryID = Convert.ToInt32(ID.Substring(3));
                else if (ID.StartsWith("I0"))
                    inventoryID = Convert.ToInt32(ID.Substring(2));
                else
                    inventoryID = Convert.ToInt32(ID.Substring(1));
                InventoryItem Item = Inventory[I];
                if (inventoryID == I && Item != null)
                {
                    return Item._Code.ToUpper();
                }
            }
            return null;
        }

        #region Arrival

        public byte[] Decrypt(byte[] sPacket, int len)
        {
            byte[] numArray = new byte[len];
            for (int index = 0; index < len; ++index)
                numArray[index] = Convert.ToByte((int)sPacket[index] ^ 10);
            return numArray;
        }

        private void arrivedData(IAsyncResult iAr)
        {
            try
            {
                int length = this.uSocket.EndReceive(iAr);
                if (length > 1)
                {
                    byte[] sPacket = new byte[length];
                    Array.Copy((Array)this.dataBuffer, 0, (Array)sPacket, 0, length);
                    string @string = Encoding.GetEncoding("Windows-1250").GetString(this.Decrypt(sPacket, length));
                    char[] chArray = new char[1]
                    {
            Convert.ToChar("\n")
                    };
                    foreach (string thePacket in @string.Split(chArray))
                    {
                        if (thePacket.Length > 5)
                        {
                            if (!thePacket.Contains("25600") && Structure.PacketView)
                            {
                                Log.WritePackets("C=>" + thePacket);
                            }
                            PacketHandler packetHandler = PacketManager.parsePacket(thePacket);
                            if (packetHandler != null)
                                packetHandler.Handle(this);
                        }
                    }
                }
                this.uSocket.BeginReceive(this.dataBuffer, 0, this.dataBuffer.Length, SocketFlags.None, new AsyncCallback(this.arrivedData), null);
            }
            catch
            {
                this.disconnect();
            }
        }

        /*if (!String.Contains("25600") && Program.PacketView)
                            {
                            Log.WritePlain("C=>" + String);
                            }*/
        #endregion

        #region Send

        public void send(Packet p)
        {
            byte[] bytes = p.getBytes();
            try
            {
                this.uSocket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, new AsyncCallback(this.sendCallBack), null);
            }
            catch
            {
                this.disconnect();
            }
        }

        public void sendBuffer(byte[] buffer)
        {
            try
            {
                this.uSocket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(this.sendCallBack), null);
            }
            catch
            {
                this.disconnect();
            }
        }

        private void sendCallBack(IAsyncResult iAr)
        {
            try
            {
                this.uSocket.EndSend(iAr);
            }
            catch
            {
                this.disconnect();
            }
        }
        #endregion

        #region Disconnect
        public void disconnect()
        {
            try
            {
                if (isDisconnected) return;

                isDisconnected = true;

                if (Room != null)
                {
                    if (isSpectating == false)
                        Room.RemoveUser(RoomSlot);
                    else
                        Room.removeSpectator(this);
                }

                UserManager.removeUser(this);

                NetworkSocket.freeSlot(SocketID, this.IPAddr, this.Hostname);
            }
            catch { }

            try { uSocket.Close(); }
            catch { }
        }
        #endregion

        #endregion

        #region Tunneling & Peer2Peer

        public IPEndPoint remoteEndPoint { get { return _RemoteEndPoint; } }
        public IPEndPoint localEndPoint { get { return _LocalEndPoint; } }

        public void setRemoteEndPoint(IPEndPoint Target)
        {
            try
            {
                nIP = IPToInt(Target.Address.ToString());
                nPort = PortToInt(Target.Port);
                _RemoteEndPoint = Target;
            }
            catch (Exception ex)
            {
                Log.AppendError("Some error on setRemoteEndPoint: " + ex.Message);
            }
        }

        public void setLocalEndPoint(IPEndPoint Target)
        {
            try
            {
                lIP = IPToInt(Target.Address.ToString());
                lPort = PortToInt(Target.Port);
                _LocalEndPoint = Target;
            }
            catch (Exception ex)
            {
                Log.AppendError("Some error on setLocalEndPoint: " + ex.Message);
            }
        }

        #endregion
    }
}
