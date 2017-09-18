using System;
using System.Threading;
using ReBornWarRock_PServer.GameServer.Networking.Packets;

namespace ReBornWarRock_PServer.GameServer.Networking.Handlers
{
    class HANDLE_OUTBOX : PacketHandler
    {
        public override void Handle(GameServer.Virtual_Objects.User.virtualUser User)
        {
            int SubType = Convert.ToInt32(getBlock(0));
            int OutboxID = Convert.ToInt32(getBlock(1));
            switch (SubType)
            {

                //  1 Day  ->   86400
                //  7 Days ->  604800
                // 30 Days -> 2592000

                case 1119: // Delete
                    {
                        ;
                        string ItemToDelete = getBlock(4);
                        DB.runQuery("DELETE FROM outbox WHERE id='" + OutboxID + "' AND ownerid='" + User.UserID + "'");
                        User.RemoveOutBoxItem(OutboxID);
                        User.send(new PACKET_OUTBOX_LIST(User));
                        break;
                    }
                case 1118: // Activate
                    {
                        if (User.OutBoxItems.Count <= 0) return;
                        string itemCode = getBlock(4);
                        string Code = null;

                        int InventorySlot = User.InventorySlots;
                        if (InventorySlot == 0)
                        {
                            User.send(new PACKET_ITEMSHOP(PACKET_ITEMSHOP.ErrorCodes.InventoryFull, ""));
                            return;
                        }
                        string[] Data = DB.runReadRow("SELECT days, count FROM outbox WHERE id='" + OutboxID + "' AND ownerid='" + User.UserID + "'");
                        if (Data.Length > 0)
                        {
                            int Days = Convert.ToInt32(Data[0]);
                            if (itemCode == "CZ83" || itemCode == "CZ84" || itemCode == "CZ85")
                            {
                                User.AddItem(itemCode, -1, int.Parse(Data[1]));
                            }

                            else if (itemCode == "XX00") // Premium Platinum 7
                            {
                                if (User.Premium == 4)
                                    User.PremiumExpire += 604800;
                                else
                                    User.PremiumExpire = Structure.currTimeStamp + 604800;

                                User.Premium = 4;
                                User.AddItem("CC02", 7, 1);//supermaster
                                User.AddItem("CA01", 7, 1);//5slot
                                User.AddItem("CD01", 7, 1);//30%up
                                User.AddItem("CD02", 7, 1);//20%up
                                User.AddItem("D801", 7, 1);//m608th

                                DB.runQuery("UPDATE users SET premium='4', premiumExpire='" + User.PremiumExpire + "' WHERE id='" + User.UserID + "'");
                            }

                            else if (itemCode == "CC60") // Premium Platinum 30
                            {
                                if (User.Premium == 4)
                                    User.PremiumExpire += 2592000;
                                else
                                    User.PremiumExpire = Structure.currTimeStamp + 2592000;

                                User.Premium = 4;
                                User.AddItem("CC02", 30, 1);//supermaster
                                User.AddItem("CA01", 30, 1);//5slot
                                User.AddItem("CD01", 30, 1);//30%up
                                User.AddItem("CD02", 30, 1);//20%up
                                User.AddItem("D801", 30, 1);//m608th

                                DB.runQuery("UPDATE users SET premium='4', premiumExpire='" + User.PremiumExpire + "' WHERE id='" + User.UserID + "'");
                            }

                            else if (itemCode == "CC41") // Premium Gold 30
                            {
                                if (User.Premium == 3)
                                    User.PremiumExpire += 2592000;
                                else
                                    User.PremiumExpire = Structure.currTimeStamp + 2592000;
                                User.Premium = 3;
                                User.AddItem("CC02", 30, 1);//supermaster
                                User.AddItem("CA01", 30, 1);//5slot
                                User.AddItem("CD01", 30, 1);//30%up
                                User.AddItem("DC21", 30, 1);//Famas Brasile

                                DB.runQuery("UPDATE users SET premium='3', premiumExpire='" + User.PremiumExpire + "' WHERE id='" + User.UserID + "'");
                            }

                            else if (itemCode == "CC44") // Premium Silver 30
                            {
                                if (User.Premium == 2)
                                    User.PremiumExpire += 2592000;
                                else
                                    User.PremiumExpire = Structure.currTimeStamp + 2592000;
                                User.Premium = 2;
                                User.AddItem("CA01", 30, 1);//5slot
                                User.AddItem("CD02", 30, 1);//20%up
                                User.AddItem("DC33", 7, 1);//m4a1gold

                                DB.runQuery("UPDATE users SET premium='2', premiumExpire='" + User.PremiumExpire + "' WHERE id='" + User.UserID + "'");
                            }
                            else if (itemCode == "CC72") // Premium Bronze 30
                            {
                                if (User.Premium == 1)
                                    User.PremiumExpire += 2592000;
                                else
                                    User.PremiumExpire = Structure.currTimeStamp + 2592000;
                                User.Premium = 1;
                                User.AddItem("CA01", 30, 1);//5slot
                                User.AddItem("DF72", 7, 1);//mp7tc

                                DB.runQuery("UPDATE users SET premium='1', premiumExpire='" + User.PremiumExpire + "' WHERE id='" + User.UserID + "'");
                            }
                            else
                            {
                                switch (itemCode)
                                {
                                    #region Military Box
                                    case "CR39":
                                        {
                                            int Rand = new Random().Next(0, 100);
                                            Code = "DC68";//xm8_navy
                                            if (Rand > 0 && Rand <= 20) { Days = 3; }
                                            else if (Rand > 20 && Rand <= 50) { Days = 7; }
                                            else if (Rand > 50 && Rand <= 65) { Days = 15; }
                                            else if (Rand > 65 && Rand <= 90) { Days = 30; }
                                            else if (Rand > 90 && Rand <= 100) { Days = -1; }
                                            User.send(new PACKET_RANDOM_BOX(User, Code, Days));
                                            Thread.Sleep(7000);
                                            User.AddItem(Code, Days, 1);
                                            break;
                                        }
                                    case "CR09":
                                        {
                                            int Rand = new Random().Next(0, 100);
                                            Code = "DC80";//m4a1_desert
                                            if (Rand > 0 && Rand <= 20) { Days = 3; }
                                            else if (Rand > 20 && Rand <= 50) { Days = 7; }
                                            else if (Rand > 50 && Rand <= 65) { Days = 15; }
                                            else if (Rand > 65 && Rand <= 90) { Days = 30; }
                                            else if (Rand > 90 && Rand <= 100) { Days = -1; }
                                            User.AddItem(Code, Days, 1);
                                            User.send(new PACKET_RANDOM_BOX(User, Code, Days));
                                            break;
                                        }
                                    case "CR11":
                                        {
                                            int Rand = new Random().Next(0, 100);
                                            Code = "DG46";//AW50_desert
                                            if (Rand > 0 && Rand <= 20) { Days = 3; }
                                            else if (Rand > 20 && Rand <= 50) { Days = 7; }
                                            else if (Rand > 50 && Rand <= 65) { Days = 15; }
                                            else if (Rand > 65 && Rand <= 90) { Days = 30; }
                                            else if (Rand > 90 && Rand <= 100) { Days = -1; }
                                            User.AddItem(Code, Days, 1);
                                            User.send(new PACKET_RANDOM_BOX(User, Code, Days));
                                            break;
                                        }
                                    #endregion
                                    #region Package
                                    /*ENGINEER_PKG*/
                                    case "CC09":
                                        {

                                            User.AddCostume("BA06", 30);
                                            User.AddItem("DU01", 30, 1);
                                            User.AddItem("DU02", 30, 1);
                                            User.AddItem("DF48", 30, 1);
                                            User.AddItem("D101", 30, 1);
                                            break;
                                        }

                                    /*Medic Package*/
                                    case "CC10":
                                        {
                                            User.AddItem("DF40", 30, 1);
                                            User.AddItem("DB33", 30, 1);
                                            User.AddCostume("BA10", 30);
                                            User.AddItem("DV01", 30, 1);

                                            break;
                                        }

                                    /*Patrol_PKG*/
                                    case "CC11":
                                        {

                                            User.AddItem("DS10", 30, 1);
                                            User.AddItem("DB17", 30, 1);
                                            User.AddCostume("BA08", 30);
                                            User.AddItem("DG08", 30, 1);
                                            break;
                                        }

                                    /*Assalt_PKG*/
                                    case "CC12":
                                        {

                                            User.AddItem("DC16", 30, 1);
                                            User.AddItem("DH01", 30, 1);
                                            User.AddCostume("BA0E", 30);
                                            User.AddItem("DS01", 30, 1);
                                            break;
                                        }

                                    /*Antitank_PKG*/
                                    case "CC13":
                                        {

                                            User.AddItem("DJ63", 30, 1);
                                            User.AddItem("DJ35", 30, 1);
                                            User.AddCostume("BA0F", 30);
                                            User.AddItem("DN03", 30, 1);
                                            break;
                                        }

                                    /*Silver_PKG*/
                                    case "CC16":
                                        {


                                            User.AddItem("DF12", 30, 1);
                                            User.AddItem("DC13", 30, 1);
                                            User.AddItem("DT05", 30, 1);
                                            User.AddItem("DG44", 30, 1);
                                            break;
                                        }

                                    /*GOLD_PKG*/
                                    case "CC17":
                                        {

                                            User.AddItem("DJ33", 30, 1);
                                            User.AddItem("DB10", 30, 1);
                                            User.AddItem("DF35", 30, 1);
                                            User.AddItem("DC33", 30, 1);
                                            break;
                                        }
                                    case "CC20": //Christmas Package
                                        {
                                            User.AddItem("DA14", 30, 1);
                                            User.AddItem("DI10", 30, 1);
                                            User.AddItem("DB20", 30, 1);
                                            break;
                                        }

                                    case "CC21": //RAINBOW PACKAGE
                                        {
                                            User.AddItem("DC70", 30, 1);
                                            User.AddItem("DD04", 30, 1);
                                            User.AddItem("DG28", 30, 1);
                                            break;
                                        }

                                    case "CC23": //DESERT PACKAGE
                                        {
                                            User.AddItem("DJ10", 30, 1);
                                            User.AddItem("DE04", 30, 1);
                                            User.AddItem("DG57", 30, 1);
                                            break;
                                        }

                                    case "CC26": //Defese PKG
                                        {

                                            User.AddItem("DN03", 30, 1);
                                            User.AddItem("DJ08", 30, 1);
                                            User.AddItem("DJ05", 30, 1);
                                            User.AddItem("DF13", 30, 1);
                                            break;
                                        }
                                    case "CC40": //RED PACKAGE
                                        {
                                            User.AddItem("DC76", 30, 1);
                                            User.AddItem("DT73", 30, 1);
                                            User.AddItem("DG31", 30, 1);
                                            User.AddItem("DF71", 30, 1);
                                            break;
                                        }

                                    /*Camo_PKG*/
                                    case "CC48":
                                        {

                                            User.AddItem("DF17", 30, 1);
                                            User.AddItem("DG57", 30, 1);
                                            User.AddItem("DC32", 30, 1);
                                            User.AddItem("DI05", 30, 1);
                                            break;
                                        }

                                    /*Valentine_Day_Package*/
                                    case "CC49":
                                        {

                                            User.AddItem("DF36", 30, 1);
                                            User.AddItem("DC34", 30, 1);
                                            User.AddItem("DF65", 30, 1);
                                            User.AddItem("DC93", 30, 1);
                                            break;
                                        }



                                    /*Dragon_PKG*/
                                    case "CC59":
                                        {

                                            User.AddItem("DD32", 30, 1);
                                            User.AddItem("DI06", 30, 1);
                                            User.AddItem("DG26", 30, 1);
                                            User.AddItem("DK31", 30, 1);
                                            break;
                                        }

                                    /*Blue_PKG*/
                                    case "CZ96":
                                        {

                                            User.AddItem("DC94", 30, 1);//famas
                                            User.AddItem("DE09", 30, 1);//scar
                                            User.AddItem("DF66", 30, 1);//mp7 blue
                                            User.AddItem("DF67", 30, 1);//scorpion
                                            User.AddItem("DG61", 30, 1);//psg
                                            break;
                                        }
                                    #endregion
                                    #region package retail
                                    case "XX01"://famas retail
                                        {
                                            User.AddItem("D804", -1, 1);
                                            User.AddItem("CA04", -1, 1);
                                            User.AddItem("CI01", 30, 1);
                                            break;
                                        }
                                    case "XX02"://m249 retail
                                        {
                                            User.AddItem("D904", -1, 1);
                                            User.AddItem("CA04", -1, 1);
                                            User.AddItem("DS01", 30, 1);//adrenalina
                                            break;
                                        }
                                    case "XX03"://m134 retail *
                                        {
                                            User.AddItem("D903", -1, 1);
                                            User.AddItem("CA04", -1, 1);
                                            User.AddItem("DS10", 30, 1);//hpkit
                                            break;
                                        }
                                    case "XX04"://m60 retail *
                                        {
                                            User.AddItem("D803", -1, 1);
                                            User.AddItem("CA04", -1, 1);
                                            User.AddItem("DS03", 30, 1);//stamina
                                            break;
                                        }
                                    case "XX05"://Ai_Aw retail *
                                        {
                                            User.AddItem("D703", -1, 1);
                                            User.AddItem("CA04", -1, 1);
                                            User.AddItem("DS03", 30, 1);
                                            break;
                                        }
                                    case "XX06"://G36C_D retail *
                                        {
                                            User.AddItem("D503", -1, 1);
                                            User.AddItem("CA04", -1, 1);
                                            User.AddItem("DU01", -1, 1);//ammobox
                                            break;
                                        }
                                    case "XX07"://Tmp9_nickel retail *
                                        {
                                            User.AddItem("D503", -1, 1);
                                            User.AddItem("CA04", -1, 1);
                                            User.AddItem("DV01", -1, 1);//medicbox
                                            break;
                                        }
                                    #endregion
                                    case "CB53":
                                        {
                                            if (ClanManager.getClanMembersMaxCount(User.ClanID) >= 100)
                                            {
                                                User.send(new PACKET_ITEMSHOP(PACKET_ITEMSHOP.ErrorCodes.CannotBeBougth, "NULL"));
                                                return;
                                            }
                                            ClanManager.UpgradeClan(User);
                                            break;
                                        }
                                    default:
                                        {
                                            if (itemCode == "CB02" && User.hasItem("CB02") || itemCode == "CB54" && User.hasItem("CB54"))
                                            {
                                                User.send(new PACKET_ITEMSHOP(PACKET_ITEMSHOP.ErrorCodes.CannotBeBougth, "NULL"));
                                                return;
                                            }
                                            if (itemCode.StartsWith("B"))
                                            {
                                                User.AddCostume(itemCode, Days);
                                                User.rebuildCostumeList();
                                            }
                                            else User.AddItem(itemCode, Days, 1);
                                            break;
                                        }
                                }
                            }

                            DB.runQuery("DELETE FROM outbox WHERE id='" + OutboxID + "' AND ownerid='" + User.UserID + "'");
                            User.RemoveOutBoxItem(OutboxID);
                            User.send(new PACKET_OUTBOX_USE(User, itemCode));
                            User.send(new PACKET_OUTBOX_LIST(User));
                            User.reloadOutBox();
                        }
                        else
                            User.send(new PACKET_ITEMSHOP(PACKET_ITEMSHOP.ErrorCodes.CannotBeBougth, "NULL"));
                        break;
                    }
                default: // Unknown
                    {
                        Log.AppendText("New SubType: " + SubType);
                        Log.AppendText(string.Join(" ", getAllBlocks()));
                        break;
                    }
            }
        }
    }
}


/* */
