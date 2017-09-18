using ReBornWarRock_PServer.GameServer.Managers;
using ReBornWarRock_PServer.GameServer.Networking.Packets;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.Room;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;
using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace ReBornWarRock_PServer.GameServer.Networking.Handlers
{
    class HANDLE_ROOM_DATA : PacketHandler
    {
            ~HANDLE_ROOM_DATA()
            {
                GC.Collect();
            }
            ArrayList saveSpawn = new ArrayList();
            public enum Subtype : int
            {
                Start = 1,
                ServerStart = 4,
                ServerNewRound = 5,
                ServerPrepareNewRound = 6,
                NewRound = 7,
                BackToRoom = 9,
                VoteKickActive = 14,
                ReadyState = 50,
                MapChange = 51,
                ModeChange = 52,
                TimeChange = 54,
                KillLimitDeathmatchChange = 53,
                KillLimitExplosiveChange = 55,
                PingChange = 59,
                //VoteKick = 61,
                KickUser = 61,
                AutostartChange = 62,
                SwitchTeam = 56,
                UserLimit = 58,
                unknow = 48,


                MagicSubtype1 = 100,
                MagicSubtype2 = 154,

                Heal = 101,
                Damage = 103,
                AmmoRecharge = 105,
                Spawn = 150,
                ServerKill = 152,
                WeaponSwitch = 155,
                Flag = 156,
                Suicide = 157,
                Place = 400,
                PlaceUse = 401,
                RoomReady = 402,
                ServerRoomReady = 403,
                FallDamage = 500,
                DeathCam = 800,

                CaptureFlag = 165,
                CaptureModeResponse = 157,
                CaptureModeRequest = 180,
                //
                ZombieExplode = 900,
                //901 drop ai
                ZombieResurrection = 902,
                VanishPlacement = 903,
                //

                RepairVehicle = 102,
                DamageVehicle = 104,
                VehicleKill = 153,
                JoinVehicle = 200,
                ChangeVehicleSeat = 201,
                LeaveVehicle = 202,
                ArtilleryRequest = 159,

            }
            public bool haveItem(GameServer.Virtual_Objects.User.virtualUser User, string Weapon)
            {
                //DF10 - DF06 - DG08 - DC04 - DH02
                if (User.hasItem(Weapon) || Weapon == "DA50" || Weapon == "DL01" || Weapon == "D001" || User.PCItem && (Weapon == "D501" || Weapon == "D602" || Weapon == "DG13" || Weapon == "D801" || Weapon == "D902") || Weapon == "DF01" || User.hasRetail(Weapon) || /* Retail part */ (/*P90*/Weapon == "DF02" || /*medic 2*/Weapon == "DQ02" || /*mp5k*/Weapon == "DB03" || /*flashbang*/Weapon == "DO02" || /*mp5k*/Weapon == "DB03") || /*end retail part*/  Weapon == "DN01" || Weapon == "DC02" || Weapon == "DG05" || Weapon == "DB01" || Weapon == "DJ01" || Weapon == "DA02")
                    return true;
                return false;
            }

            public bool AllowedAfterKill(string Weapon)
            {
                if (Weapon.StartsWith("DN") || Weapon.StartsWith("DM") || Weapon == "DU04")
                    return true;
                return false;
            }

            public bool isZombieWeapon(string Weapon)
            {
                if (Weapon == "DA50" || Weapon == "DA51" || Weapon == "DA52" || Weapon == "DA53" || Weapon == "DA54" || Weapon == "DA55" || Weapon == "DA56" || Weapon == "DA57" || Weapon == "DA58" || Weapon == "DA59" || Weapon == "DA60" || Weapon == "DA61" || Weapon == "DA62" || Weapon == "DA63" || Weapon == "DA64" || Weapon == "DA65" || Weapon == "DA66" || Weapon == "DA67" || Weapon == "DN51" || Weapon == "DN52" || Weapon == "DN53" || Weapon == "DN54" || Weapon == "DN55" || Weapon == "DN56")
                    return true;
                return false;
            }

            public override void Handle(GameServer.Virtual_Objects.User.virtualUser User)
            {
                try
                {
                    if (Blocks.Length >= 1 && User.Room != null)
                    {
                        int sourceSeat = Convert.ToInt32(Blocks[0]);
                        int roomID = Convert.ToInt32(Blocks[1]);
                        if (sourceSeat != User.RoomSlot || roomID != User.Room.ID)
                            return;
                        if (sourceSeat == User.RoomSlot && roomID == User.Room.ID)
                        {
                            //bool LobbyChange = false;
                            virtualRoom currentRoom = User.Room;
                            int subtype1 = Convert.ToInt32(Blocks[3]);
                            Subtype subtype = (Subtype)subtype1;
                            int tType = Convert.ToInt32(getBlock(3));
                            int tValue = Convert.ToInt32(getBlock(6));
                            int Value = Convert.ToInt32(getBlock(9));
                            int Place1 = Convert.ToInt32(getBlock(6));
                            int Place2 = Convert.ToInt32(getBlock(7));
                            string[] ArrSorted = new string[getAllBlocks().Length];
                            Array.Copy(getAllBlocks(), ArrSorted, getAllBlocks().Length);
                            int P0 = Convert.ToInt32(getBlock(0));
                            int P1 = Convert.ToInt32(getBlock(1));
                            string[] sendBlocks = new string[Blocks.Length - 1];
                            Array.Copy(Blocks, sendBlocks, sendBlocks.Length);
                            switch (subtype)
                            {
                                case Subtype.Start:
                                    {
                                        if (currentRoom.RoomMasterSlot == User.RoomSlot && currentRoom.RoomStatus != 2)
                                        {
                                            if (currentRoom.isPremMap() == true && User.Premium < 1)
                                            {
                                                User.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> You cannot start premium map as free user!!", 999, "NULL"));
                                                break;
                                            }
                                            else if (currentRoom.Channel == 4 && User.Rank < 2)
                                            {
                                                User.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Whisper, "SYSTEM >> This channel is not available yet.", 998, "NULL"));
                                                break;
                                            }
                                            else if (currentRoom.Channel == 3 && User.Rank < 6 && currentRoom.Mode == 11)
                                            {
                                                User.Room.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Whisper, "SYSTEM >> Time Attack Mode isn't available yet, but we're working on!!", 998, "NULL"));
                                                break;
                                            }
                                            else if (currentRoom.Channel == 3 && User.Rank < 6 && currentRoom.Mode == 12)
                                            {
                                                User.Room.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Whisper, "SYSTEM >> Escope Mode isn't available yet, but we're working on!!", 998, "NULL"));
                                                break;
                                            }
                                            else if (currentRoom.RoomType == 1)
                                            {
                                                if (currentRoom.getSideCount(0) != currentRoom.getSideCount(1))
                                                {
                                                    currentRoom.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Whisper, "SYSTEM >> Teems need to be balanced.", 998, "NULL"));
                                                    break;
                                                }
                                            }
                                            else if (currentRoom.Mode == 11)
                                            {
                                                currentRoom.FirstSpawn = true;
                                            }

                                            if (ConfigServer.Debug == 0)
                                            {
                                                if (currentRoom.Channel != 3 && currentRoom.Mode != 1) if (currentRoom.Players.Count <= 1 || currentRoom.getSideCount(0) > currentRoom.getSideCount(1) + 1 || currentRoom.getSideCount(1) > currentRoom.getSideCount(0) + 1 || currentRoom.getSideCount(0) == 0 || currentRoom.getSideCount(1) == 0) break;
                                            }
                                            if (currentRoom.Start())
                                            {
                                                subtype = Subtype.ServerStart;
                                                sendBlocks[6] = currentRoom.MapID.ToString();
                                                currentRoom.Channel = User.Channel;
                                                currentRoom.RoomStatus = 2;
                                                currentRoom.LobbyChange = true;
                                            }
                                        }
                                        break;
                                    }
                                case Subtype.NewRound:
                                    {
                                        break;
                                    }
                                case Subtype.MapChange:
                                    {
                                        if (currentRoom.RoomMasterSlot == User.RoomSlot && currentRoom.RoomStatus != 2)
                                        {
                                            currentRoom.LobbyChange = true;
                                            currentRoom.MapID = tValue;
                                            sendBlocks[7] = currentRoom.Mode.ToString();
                                        }
                                        break;
                                    }
                                case Subtype.AutostartChange:
                                    {
                                        if (currentRoom.RoomMasterSlot == User.RoomSlot && currentRoom.RoomStatus != 2)
                                        {
                                            //LobbyChange = true;
                                            //User.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> AutoStart Not Work !!!", 999, "NULL"));
                                        }
                                        break;
                                    }
                                case Subtype.ArtilleryRequest:
                                    {
                                        if (currentRoom.Channel != 2 || User.Class != 2 || !User.hasItem("DX01")) return;

                                        currentRoom.send(new SP_Unknown(30000, 1, User.RoomSlot, currentRoom.ID, 2, 159, 0, 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, sendBlocks[19], sendBlocks[20], sendBlocks[21], 0, 0, 0, 0, 0, "$"));
                                        break;
                                    }

                                case Subtype.KillLimitDeathmatchChange:
                                    {
                                        if (currentRoom.RoomMasterSlot == User.RoomSlot && currentRoom.RoomStatus != 2)
                                        {
                                            currentRoom.LobbyChange = true;
                                            currentRoom.Rounds = Convert.ToInt32(sendBlocks[6]);
                                        }
                                        break;
                                    }
                                case Subtype.unknow:
                                    {
                                        // C => 866974286 30000 0 48 1 9 0 0 0 0 0 0 0 0 0 0
                                        // C => 866974286 30000 0 48 1 9 0 0 0 0 0 0 0 0 0 0 
                                        // S => 831751812 30000 1 0 48 1 9 0 0 0 0 0 0 0 0 0 0
                                        sendBlocks[0] = "1";
                                        sendBlocks[1] = "0";
                                        sendBlocks[2] = "48";
                                        sendBlocks[3] = "1";
                                        sendBlocks[4] = "9";
                                        sendBlocks[5] = "0";
                                        sendBlocks[6] = "0";
                                        sendBlocks[7] = "0";
                                        sendBlocks[8] = "0";
                                        sendBlocks[9] = "0";
                                        sendBlocks[10] = "0";
                                        sendBlocks[11] = "0";
                                        sendBlocks[12] = "0";
                                        sendBlocks[13] = "0";
                                        sendBlocks[14] = "0";
                                        break;
                                    }
                                case Subtype.KillLimitExplosiveChange:
                                    {
                                        if (currentRoom.RoomMasterSlot == User.RoomSlot && currentRoom.RoomStatus != 2)
                                        {
                                            currentRoom.LobbyChange = true;
                                            if (currentRoom.Mode == 1)
                                            {
                                                if (User.Premium > 0)
                                                    currentRoom.Rounds = Convert.ToInt32(sendBlocks[6]);
                                                else
                                                    currentRoom.Rounds = 0;
                                            }
                                            else
                                                currentRoom.Rounds = Convert.ToInt32(sendBlocks[6]);
                                        }
                                        break;
                                    }
                                case Subtype.ModeChange:
                                    {
                                        if (currentRoom.RoomMasterSlot == User.RoomSlot && currentRoom.RoomStatus != 2)
                                        {
                                            currentRoom.LobbyChange = true;
                                            int OldMode = -1;
                                            int NewMode = -1;
                                            OldMode = currentRoom.Mode;
                                            NewMode = Convert.ToInt32(Blocks[6]);
                                            #region SendChange
                                            switch (currentRoom.Channel)
                                            {
                                                case 1:
                                                    if (NewMode == 1)
                                                    {
                                                        currentRoom.Mode = 1;
                                                        if (User.Premium > 0)
                                                            currentRoom.Rounds = 2;
                                                        else
                                                            currentRoom.Rounds = 0;
                                                        sendBlocks[7] = (currentRoom.MapID = 12).ToString();
                                                        sendBlocks[9] = currentRoom.Rounds.ToString();
                                                    }
                                                    else if (NewMode == 0)
                                                    {
                                                        currentRoom.Mode = 0;
                                                        sendBlocks[7] = (currentRoom.MapID = 12).ToString();
                                                        sendBlocks[8] = (currentRoom.Rounds = 3).ToString();
                                                        sendBlocks[9] = (currentRoom.Rounds = 3).ToString();
                                                    }
                                                    else if (NewMode == 2)
                                                    {
                                                        currentRoom.Mode = 2;
                                                        sendBlocks[7] = (currentRoom.MapID = 27).ToString();
                                                        sendBlocks[8] = (currentRoom.Rounds = 2).ToString();
                                                        sendBlocks[9] = (currentRoom.Rounds = 2).ToString();
                                                        sendBlocks[10] = "2";
                                                    }
                                                    break;
                                                case 2:
                                                    if (NewMode == 3)
                                                    {
                                                        sendBlocks[7] = (currentRoom.MapID = 40).ToString();
                                                        currentRoom.MapID = 40;
                                                        sendBlocks[10] = "3";
                                                    }
                                                    else if (NewMode == 2)
                                                    {
                                                        sendBlocks[7] = (currentRoom.MapID = 1).ToString();
                                                        sendBlocks[8] = "2";
                                                        currentRoom.Kills = 2;
                                                        currentRoom.MapID = 1;
                                                        sendBlocks[10] = "3";
                                                    }
                                                    break;
                                                case 3:
                                                    break;
                                                case 4:
                                                    if (OldMode == 7 && NewMode == 8)
                                                    {
                                                        sendBlocks[7] = currentRoom.MapID.ToString();
                                                    }
                                                    else if (OldMode == 8 && NewMode == 7)
                                                    {
                                                        sendBlocks[7] = (currentRoom.MapID = 46).ToString();
                                                    }
                                                    break;
                                            }
                                            #endregion
                                        }
                                        break;
                                    }
                                /*case Subtype.VoteKick:
                                    {
                                        int kickTarget = Convert.ToInt32(getBlock(7));
                                        bool kickActive = Convert.ToInt32(getBlock(6)) == 1;

                                        //sendBlocks[3] = "14";
                                        subtype = Subtype.VoteKickActive;
                                        sendBlocks[0] = currentRoom.getPlayer(kickTarget).RoomSlot.ToString();
                                        sendBlocks[1] = currentRoom.ID.ToString();
                                        sendBlocks[2] = User.SessionID.ToString();
                                        sendBlocks[7] = kickTarget.ToString();
                                        sendBlocks[10] = currentRoom.getPlayer(kickTarget).RoomSlot.ToString();


                                        break;
                                    }*/

                                case Subtype.KickUser:
                                    {

                                        if (currentRoom.KickAlready == true || currentRoom.Mode == 1 || currentRoom.VoteKick == 0) return;
                                        if (currentRoom.VoteKickTime > 0)
                                        {
                                            if (sendBlocks[6] == "1")
                                            {
                                                currentRoom.KickYes++; sendBlocks[6] = "1";
                                            }
                                            else if (sendBlocks[6] == "0")
                                            {
                                                currentRoom.KickNo++; sendBlocks[6] = "0";
                                            }
                                        }
                                        else
                                        {
                                            currentRoom.KickSeat = Convert.ToInt32(sendBlocks[7]);
                                            currentRoom.VoteKickTime = 30;
                                            currentRoom.KickAlready = true;
                                            Array.Resize<string>(ref currentRoom.KickString, sendBlocks.Length);
                                            Array.Copy(sendBlocks, currentRoom.KickString, sendBlocks.Length);
                                            currentRoom.KickString[5] = "4";
                                            sendBlocks[6] = "1";
                                        }

                                        break;
                                    }
                                case Subtype.CaptureModeRequest:
                                    {
                                        if (User.currentVehicle.Code == "ED13")
                                        {
                                            subtype = Subtype.CaptureModeResponse;
                                            sendBlocks[8] = "7"; // ??
                                            int side = int.Parse(getBlock(9));
                                            if (currentRoom.getSide(User) == 1/*currentRoom.getSide(1)*/ && side == 1/*(int)currentRoom.Side.NIU*/)
                                            {
                                                currentRoom.cNIUExplosivePoints += 20;
                                            }
                                            else
                                            {
                                                currentRoom.cDerbExplosivePoints += 20;
                                            }
                                            User.rPoints += 50;
                                        }
                                        break;
                                    }
                                case Subtype.ServerKill:
                                    {
                                        if (User.Health > 0) return;
                                        User.Health = 0;
                                        User.isSpawned = false;
                                        break;
                                    }
                                case Subtype.PingChange:
                                    {
                                        if (currentRoom.RoomMasterSlot == User.RoomSlot && currentRoom.RoomStatus != 2)
                                        {
                                            currentRoom.LobbyChange = true;
                                            currentRoom.Ping = Convert.ToInt32(Blocks[6]);
                                        }
                                        break;
                                    }
                                case Subtype.ReadyState:
                                    {
                                        if (User.F5 == false)
                                        {
                                            User.isReady = !User.isReady;
                                            sendBlocks[6] = Convert.ToByte(User.isReady).ToString();
                                        }
                                        else
                                        {
                                            //nulla
                                        }
                                        break;
                                    }
                                case Subtype.BackToRoom:
                                    {
                                        currentRoom.BackToRoom = true;
                                        User.Health = 0;
                                        User.InGame = false;
                                        break;
                                    }
                                case Subtype.Suicide:
                                    {
                                        if (!currentRoom.GameActive || currentRoom.Mode == 0 || currentRoom.Mode == 1 || currentRoom.Channel == 3 || User.Health <= 0) return;

                                        if (User.LastSuicideTick + 5 > Structure.timestamp || User.currentVehicle != null) return;
                                        User.LastSuicideTick = Structure.timestamp;

                                        bool OutOfWorldSuicide = int.Parse(getBlock(7)) == 5;

                                        if (OutOfWorldSuicide)
                                        {
                                            currentRoom.send(new PACKET_SUICIDE(User.RoomSlot, PACKET_SUICIDE.SuicideType.Suicide, true));
                                            User.Die();
                                            return;
                                        }

                                        if (User.Health > 0)
                                        {
                                            User.Die();
                                            //currentRoom.updateTime();
                                        }
                                        break;
                                    }
                                case Subtype.SwitchTeam:
                                    {
                                        if (User.isSpectating) return;
                                        if (currentRoom.RoomType != 1)
                                        {
                                            if (currentRoom.canSwitch(User) == false) return;

                                            sendBlocks[7] = currentRoom.switchSide(User).ToString();
                                            sendBlocks[8] = currentRoom.RoomMasterSlot.ToString();
                                        }
                                        break;
                                    }
                                case Subtype.RoomReady:
                                    {
                                        subtype = Subtype.ServerRoomReady;
                                        sendBlocks[6] = "3";
                                        sendBlocks[7] = "882";//è sbagliato cambia sempre ed e collegato con packet_map_data
                                        sendBlocks[8] = "0";
                                        sendBlocks[9] = "1";

                                        if (currentRoom.Mode == 6 && currentRoom.Channel == 2)
                                        {
                                            sendBlocks[10] = Convert.ToString(currentRoom.Kills);
                                            sendBlocks[11] = "20";
                                            User.TotalWarPoint = 20;
                                        }
                                        break;
                                    }



                                case Subtype.Spawn:
                                    {
                                        if (User.isSpawned && User.Health > 0)
                                        {
                                            User.breaked = true;
                                            break;
                                        }
                                        int Mode = currentRoom.Mode;
                                        int Selection = currentRoom.RoomType;

                                        #region New Modes
                                        switch (Mode)
                                        {
                                            case 1:
                                                if (Selection == 0)
                                                {
                                                    Item item = ItemManager.getItem("DA02");
                                                    if (item != null)
                                                    {
                                                        User.Weapon = item.ID;
                                                    }
                                                }
                                                break;
                                            case 3:
                                                if (Selection == 0)
                                                {
                                                    Item item = ItemManager.getItem("DB01");
                                                    if (item != null)
                                                    {
                                                        User.Weapon = item.ID;
                                                    }
                                                }
                                                break;
                                            case 4:
                                                if (Selection == 0)
                                                {
                                                    Item item = ItemManager.getItem("DN01");
                                                    if (item != null)
                                                    {
                                                        User.Weapon = item.ID;
                                                    }
                                                }
                                                else if (Selection == 1)
                                                {
                                                    Item item = ItemManager.getItem("D202");
                                                    if (item != null)
                                                    {
                                                        User.Weapon = item.ID;
                                                    }
                                                }
                                                break;
                                            case 5:
                                                if (Selection == 0)
                                                {
                                                    Item item = ItemManager.getItem("DB25");
                                                    if (item != null)
                                                    {
                                                        User.Weapon = item.ID;
                                                    }
                                                }
                                                else if (Selection == 1)
                                                {
                                                    Item item = ItemManager.getItem("DC74");
                                                    if (item != null)
                                                    {
                                                        User.Weapon = item.ID;
                                                    }
                                                }
                                                else if (Selection == 2)
                                                {
                                                    Item item = ItemManager.getItem("DG42");
                                                    if (item != null)
                                                    {
                                                        User.Weapon = item.ID;
                                                    }
                                                }
                                                break;
                                            case 6:
                                                if (Selection == 1)
                                                {
                                                    Item item = ItemManager.getItem("DA06");
                                                    if (item != null)
                                                    {
                                                        User.Weapon = item.ID;
                                                    }
                                                }
                                                break;
                                        }
                                        #endregion
                                        if (currentRoom.Mode == 1)
                                        {
                                            sendBlocks[10] = currentRoom.SpawnLocation.ToString();
                                            sendBlocks[11] = currentRoom.SpawnLocation.ToString();
                                            sendBlocks[12] = currentRoom.SpawnLocation.ToString();
                                            ++currentRoom.SpawnLocation;
                                        }
                                        if (currentRoom.Channel == 3)
                                        {
                                            currentRoom.spawnedPlayers++;

                                            saveSpawn.Add(sendBlocks);

                                            if (currentRoom.spawnedPlayers >= currentRoom.PlayerCount)
                                            {
                                                currentRoom.readyZombie = true;
                                                foreach (string[] sa in saveSpawn)
                                                {
                                                    User.Rockets = User.Granade = User.killFromSpawn = 0;
                                                    User.Plantings = User.rSkillPoints = 0;
                                                    User.SpawnProtection = 3;
                                                    User.Health = 1000;
                                                    User.isSpawned = true;
                                                    User.Alive = true;
                                                    User.Class = Convert.ToInt32(getBlock(7));
                                                    User.ClassCode = getBlock(26);
                                                    User.currentVehicle = null;
                                                    User.currentSeat = null;
                                                    currentRoom.send(new PACKET_ROOM_DATA(User, sa));
                                                }
                                                saveSpawn.Clear();
                                            }

                                            /*++currentRoom.spawnedPlayers;
                                            if (currentRoom.PlayerCount == currentRoom.spawnedPlayers)
                                                User.Room.readyZombie = true;*/
                                            if (currentRoom.Mode == 12)
                                            {
                                                currentRoom.send(new SP_Unknown(30053, 0, 0, 5, 5));
                                                //currentRoom.SpawnPlayerZombie(0, User.RoomSlot);
                                            }
                                            if (currentRoom.Mode == 11 && currentRoom.spawnedPlayers >= currentRoom.PlayerCount && User.rDeaths <= 0) // FreeWar : Mappa Di POW_CAMP
                                            {
                                                if (currentRoom.ZombieDifficulty == 0)
                                                {
                                                    currentRoom.send(new PACKET_TIMEATTACK_ALL(30053, 0, 0, 5, 5));
                                                }
                                                else
                                                {
                                                    //285778092 30053 0 1 4 3 
                                                    currentRoom.send(new PACKET_TIMEATTACK_ALL(30053, 0, 1, 4, 3));
                                                }


                                                if (currentRoom.ZombieDifficulty == 0)
                                                {
                                                    currentRoom.send(new PACKET_TIMEATTACK_STAGE(currentRoom, 2, 300));
                                                    currentRoom.TimeZombie = 300;
                                                }
                                                else
                                                {
                                                    currentRoom.send(new PACKET_TIMEATTACK_STAGE(currentRoom, 2, 500));
                                                    currentRoom.TimeZombie = 500;
                                                }
                                                //cazzoneso = "2";
                                                currentRoom.Stage1.Start(); // Start time Stage 1
                                                currentRoom.IntoPassing++;
                                            }

                                            return;
                                        }
                                        User.Rockets = User.Granade = User.killFromSpawn = 0;
                                        User.Plantings = User.rSkillPoints = 0;
                                        User.SpawnProtection = 3;
                                        User.Health = 1000;
                                        User.isSpawned = true;
                                        User.Alive = true;
                                        User.Class = Convert.ToInt32(getBlock(7));
                                        User.ClassCode = getBlock(26);
                                        User.currentVehicle = null;
                                        User.currentSeat = null;
                                        /* Snow fight */
                                        /*if(currentRoom.MapID == 72 && Config.XmasEvent)
                                        {
                                            User.Weapon = 122;
                                        }*/
                                        break;
                                    }

                                case Subtype.FallDamage:
                                    {
                                        int damage = Convert.ToInt32(getBlock(9));
                                        int Types = Convert.ToInt32(getBlock(6));
                                        if (Types == 1)
                                        {
                                            if (damage < 0 || User.Health <= 0) { return; }
                                            #region bodydefence
                                            if (User.BodyDefence)
                                            {
                                                int classe = User.Class;
                                                string[] _CostumeE = User.CostumeE.Split(new char[] { ',' });
                                                string[] _CostumeM = User.CostumeM.Split(new char[] { ',' });
                                                string[] _CostumeS = User.CostumeS.Split(new char[] { ',' });
                                                string[] _CostumeA = User.CostumeA.Split(new char[] { ',' });
                                                string[] _CostumeH = User.CostumeH.Split(new char[] { ',' });

                                                if (classe == 0 && _CostumeE[0] != "BA01")
                                                {
                                                    string[] CodeCostume = DB.runReadRow("SELECT falldamage FROM costume_defence WHERE code='" + _CostumeE[0] + "'");
                                                    double coefficente = Convert.ToInt32(CodeCostume[0]);
                                                    double RCoef = (coefficente / 100.0);
                                                    double vOut = Convert.ToDouble(damage);
                                                    damage = (int)(vOut * RCoef);
                                                }
                                                if (classe == 1 && _CostumeM[0] != "BA02")
                                                {
                                                    string[] CodeCostume = DB.runReadRow("SELECT falldamage FROM costume_defence WHERE code='" + _CostumeM[0] + "'");
                                                    double coefficente = Convert.ToInt32(CodeCostume[0]);
                                                    double RCoef = (coefficente / 100.0);
                                                    double vOut = Convert.ToDouble(damage);
                                                    damage = (int)(vOut * RCoef);
                                                }
                                                if (classe == 2 && _CostumeS[0] != "BA03")
                                                {
                                                    string[] CodeCostume = DB.runReadRow("SELECT falldamage FROM costume_defence WHERE code='" + _CostumeS[0] + "'");
                                                    double coefficente = Convert.ToInt32(CodeCostume[0]);
                                                    double RCoef = (coefficente / 100.0);
                                                    double vOut = Convert.ToDouble(damage);
                                                    damage = (int)(vOut * RCoef);
                                                }
                                                if (classe == 3 && _CostumeA[0] != "BA04")
                                                {
                                                    string[] CodeCostume = DB.runReadRow("SELECT falldamage FROM costume_defence WHERE code='" + _CostumeA[0] + "'");
                                                    double coefficente = Convert.ToInt32(CodeCostume[0]);
                                                    double RCoef = (coefficente / 100.0);
                                                    double vOut = Convert.ToDouble(damage);
                                                    damage = (int)(vOut * RCoef);
                                                }
                                                if (classe == 4 && _CostumeH[0] != "BA05")
                                                {
                                                    string[] CodeCostume = DB.runReadRow("SELECT falldamage FROM costume_defence WHERE code='" + _CostumeH[0] + "'");
                                                    double coefficente = Convert.ToInt32(CodeCostume[0]);
                                                    double RCoef = (coefficente / 100.0);
                                                    double vOut = Convert.ToDouble(damage);
                                                    damage = (int)(vOut * RCoef);
                                                }
                                            }
                                            #endregion
                                            User.Health -= damage;

                                            if (User.Health <= 0)
                                            {
                                                Blocks[3] = "157";
                                                Blocks[6] = User.RoomSlot.ToString();
                                                User.Health = 0;
                                                User.rDeaths++;
                                                User.rPoints += 2;
                                                User.isSpawned = false;
                                                if (currentRoom.getSide(User) == 0)
                                                {
                                                    currentRoom.KillsDeberanLeft--;
                                                }
                                                else
                                                {
                                                    currentRoom.KillsNIULeft--;
                                                }
                                            }
                                            else
                                            {
                                                sendBlocks[11] = damage.ToString();
                                                sendBlocks[12] = User.Health.ToString();
                                            }
                                        }
                                        else
                                        {
                                            int vehicleID = Convert.ToInt32(getBlock(8));
                                            bool underWater = Convert.ToInt32(getBlock(10)) == 1;
                                            Vehicle vehicleById = currentRoom.GetVehicleById(vehicleID);

                                            if (vehicleById == null) { sendPacket = false; return; };

                                            damage = (int)Math.Ceiling((decimal)(vehicleById.MaxHealth * (int.Parse(getBlock(9)) / int.Parse(getBlock(10)))) / 100);
                                            if (underWater)
                                            {
                                                damage = (int)Math.Truncate((double)(vehicleById.MaxHealth * 60) / 100.0);
                                            }
                                            vehicleById.Health -= damage;
                                            sendBlocks[9] = damage.ToString();
                                            sendBlocks[11] = damage.ToString();
                                            sendBlocks[12] = vehicleById.Health.ToString();

                                            if (vehicleById.Health <= 0)
                                            {
                                                foreach (VehicleSeat virtualVehicleSeats in vehicleById.Seats.Values)
                                                {
                                                    if (virtualVehicleSeats.seatOwner != null)
                                                    {
                                                        if (vehicleById.Side == 0)
                                                        { currentRoom.KillsDeberanLeft--; }
                                                        else
                                                        { currentRoom.KillsNIULeft--; }

                                                        ++virtualVehicleSeats.seatOwner.rDeaths;
                                                        ++virtualVehicleSeats.seatOwner.rPoints;
                                                        virtualVehicleSeats.seatOwner.isSpawned = false;
                                                        virtualVehicleSeats.seatOwner.Health = 0;
                                                        currentRoom.send((Packet)new PACKET_ROOM_DATA(User, new object[28] { User.RoomSlot, currentRoom.ID, 2, 157, 0, 1, User.RoomSlot, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "$" }));
                                                    }
                                                }
                                                currentRoom.send((Packet)new PACKET_VEHICLE_EXPLODE(currentRoom.ID, vehicleID, User.RoomSlot));
                                                vehicleById.Health = 0;
                                                return;
                                            }
                                        }
                                        break;

                                    }
                                case Subtype.AmmoRecharge:
                                    {
                                        break;
                                    }
                                case Subtype.Place:
                                    {
                                        if (User.Plantings > 8) return;

                                        string ItemCode = Blocks[27];
                                        if (User.hasItem(ItemCode))
                                        {
                                            User.Plantings++;
                                            sendBlocks[8] = currentRoom.AddPlacment(User, ItemCode).ToString();

                                        }
                                        else
                                        {
                                            User.disconnect();
                                        }
                                        break;
                                    }
                                /*case Subtype.Place:
                                    {
                                        if (User.Plantings > 8) return;

                                        string ItemCode = Blocks[27];
                                        if (User.hasItem(ItemCode))
                                        {
                                            User.Plantings++;
                                            sendBlocks[8] = currentRoom.AddPlacment(User, ItemCode).ToString();
                                        }
                                        else

                                            User.disconnect();

                                        break;
                                    }*/

                                case Subtype.ZombieResurrection:
                                    {
                                        int DropType = Convert.ToInt32(getBlock(7));

                                        sendBlocks[1] = currentRoom.ID.ToString();

                                        switch (DropType)
                                        {
                                            case 0: // Respawn
                                                {
                                                    break;
                                                }
                                            case 1: // Medic
                                                {
                                                    User.Health = 1000;
                                                    sendBlocks[10] = User.Health.ToString();
                                                    break;
                                                }
                                            case 2: // Ammo
                                                {
                                                    break;
                                                }
                                            case 3: // Repair
                                                {
                                                    int VehicleID = (currentRoom.MapID == 46 && currentRoom.MapID == 51 && currentRoom.MapID == 80 ? 5 : 1);
                                                    currentRoom.Vehicles[VehicleID].Health = 100000;

                                                    sendBlocks[11] = "100000";
                                                    sendBlocks[12] = "100000";
                                                    break;
                                                }
                                        }

                                        currentRoom.DropID--;
                                        break;
                                    }
                                case Subtype.VanishPlacement:
                                    {
                                        break;
                                    }
                                default:
                                    {
                                        //Log.AppendText(string.Concat("New Subtype for RoomData: ", subtype));
                                        //Log.AppendText(string.Concat("Blocks: ", string.Join(" ", base.getAllBlocks())));
                                        //Log.AppendText("This subtype has been blocked and rejected");
                                        return;
                                    }

                                case Subtype.ZombieExplode:
                                    {
                                        int slot = int.Parse(getBlock(8));
                                        virtualZombie z = currentRoom.GetZombieById(slot);

                                        if (z.name != "Breaker") currentRoom.send(new PACKET_SUICIDE(slot));

                                        if (z != null && z.Health > 0 && z.name != "Breaker")
                                        {
                                            z.Health = 0;
                                            z.RespawnTick = Structure.timestamp + 4;
                                            if(currentRoom.Mode != 11) currentRoom.zombieKills++;
                                        }
                                        break;
                                    }
                                case Subtype.Damage:
                                    {
                                        /*0 0 2 103 0 1 1 4 0 39 0 39 2 0 0 1242 0 0 4488.7671 1105.4860 3464.1897 2.0000 0 14.0000 5.0000 0 DF01 */
                                        /*4 0 2 103 0 1 1 0 4 24 0 24 4 0 0 3620 0 0 0 4571.9575 1104.7207 3315.2612 4.0000 0 20.0000 5.0000 0 DC06 3499 */
                                        int UserRoomSlot = Convert.ToInt32(getBlock(0));
                                        int HeadshotCalculation = Convert.ToInt32(getBlock(15)) - User.SessionID;
                                        string block21 = getBlock(22);
                                        string Weapon = getBlock(27).Substring(0, 4);
                                        bool IsHeadshot = (HeadshotCalculation == (1237 + User.SessionID) ? true : false);
                                        int Damage = 0;
                                        if (Weapon.StartsWith("E")) Damage = ItemManager.getVehDamage(Weapon);
                                        else Damage =  ItemManager.getDamage(Weapon);
                                        int weaponSlot = Convert.ToInt32(getBlock(13)); //FreeWar : Indica lo slot usato
                                        int TargetID = Convert.ToInt32(getBlock(7));
                                        if (currentRoom.Channel == 3) TargetID = Convert.ToInt32(getBlock(8));
                                        int num15 = new Random().Next(300, 600);
                                        int Point = 5;
                                        int num17 = 0;
                                        if (User.currentVehicle != null)
                                        {
                                            bool flag2 = Convert.ToInt32(this.getBlock(13)) == 0;
                                            if (Weapon == User.currentVehicle.Code)
                                            {
                                                num17 += 2;
                                                if (flag2)
                                                {
                                                    Damage = ItemManager.getDamage(User.currentSeat.MainCTCode, 2);
                                                    if (Damage > 600)
                                                        Damage -= num15;
                                                }
                                                else
                                                {
                                                    Damage = ItemManager.getDamage(User.currentSeat.SubCTCode, 2);
                                                    if (Damage > 600)
                                                        Damage -= num15;
                                                }
                                            }
                                        }
                                        #region Zombie
                                        if (currentRoom.Channel == 3 && currentRoom.Mode != 12)
                                        {
                                            int ZombieID = Convert.ToInt32(getBlock(7));
                                        //currentRoom.ZombieID = ZombieID;
                                        if (ZombieID > 3)
                                            {
                                                virtualZombie _Zombie = currentRoom.GetZombieById(ZombieID);
                                                {
                                                    if (_Zombie.timestamp > Structure.timestamp) return;
                                                    if (_Zombie.Death) return;
                                                    if (currentRoom.isZombieWeapon(Weapon) && !(_Zombie.doDamage == 800)) return;
                                                    //Damage = _Zombie.doDamage;
                                                    if (_Zombie.ID == ZombieID)
                                                    {
                                                        if (_Zombie.Points != 0)
                                                        {
                                                            if (_Zombie != null && _Zombie.Health > 0)
                                                            {
                                                            if (User.IsPowerUp) Damage = Damage * 20;
                                                                if (_Zombie.timestamp > Structure.timestamp) return;
                                                                if (_Zombie.name == "Breaker")
                                                                {
                                                                    double RCoef = (30 / 100.0);
                                                                    double vOut = Convert.ToDouble(Damage);
                                                                    Damage = (int)(vOut * RCoef);
                                                                }

                                                                _Zombie.Health -= Damage;

                                                                if (_Zombie.name == "Breaker")
                                                                {
                                                                    User.BossDamage += Damage;
                                                                }

                                                                sendBlocks[12] = Convert.ToString(getBlock(12));
                                                                sendBlocks[15] = Convert.ToString(_Zombie.Health);
                                                                sendBlocks[16] = Convert.ToString(_Zombie.Health - Damage);
                                                                sendBlocks[22] = Convert.ToString(getBlock(22));

                                                                if (_Zombie.Health > 0 && _Zombie.name != "Breaker")
                                                                {
                                                                sendBlocks[15] = _Zombie.Health.ToString();
                                                                sendBlocks[16] = (_Zombie.Health + Damage).ToString();
                                                                sendBlocks[27] = Weapon.ToString();
                                                            }  

                                                            }

                                                            if (_Zombie.Health <= 0) //zombi vita <=0
                                                            {
                                                                if (_Zombie.givesSkillPoints)
                                                                {
                                                                    User.rSkillPoints++;
                                                                    if (User.rSkillPoints >= 5)
                                                                        User.send(new PACKET_SKILL_POINT(User));
                                                                }
                                                                User.rKills++;
                                                                if (User.rKills % 7 == 0 && currentRoom.Mode == 11)
                                                                {
                                                                    currentRoom.spawnZombie(7);
                                                                    currentRoom.spawnZombie(6);
                                                                }

                                                            currentRoom.DropKills++;
                                                            if (currentRoom.DropKills == 25 && currentRoom.Mode == 11)
                                                            {
                                                                currentRoom.DropKills = 0;
                                                                currentRoom.DropID++;
                                                                currentRoom.send(new PACKET_DROP_AI(User, ZombieID, currentRoom.DropID, 2));
                                                            }
                                                            if (currentRoom.DropKills == 15 && currentRoom.Mode == 11)
                                                            {
                                                                currentRoom.DropID++;
                                                                currentRoom.send(new PACKET_DROP_AI(User, ZombieID, currentRoom.DropID, 1));
                                                            }

                                                            
                                                            if (currentRoom.DropKills >= 30 && currentRoom.Mode != 11)//ogni 30 zombiekill spawna 1 drop 
                                                                {
                                                                if (currentRoom.Mode == 9 /*survival*/&& currentRoom.Wave > 2)
                                                                    {
                                                                        currentRoom.DropKills = 0;
                                                                        currentRoom.DropID++;
                                                                        currentRoom.send(new PACKET_DROP_AI(User, ZombieID, currentRoom.DropID, new Random().Next(0, 2)));
                                                                    }
                                                                    if (currentRoom.Mode == 10 /*defence*/&& currentRoom.Wave > 2)
                                                                    {
                                                                        currentRoom.DropKills = 0;
                                                                        currentRoom.DropID++;
                                                                        currentRoom.send(new PACKET_DROP_AI(User, ZombieID, currentRoom.DropID, new Random().Next(0, 4)));
                                                                    }
                                                                }

                                                                if (HeadshotCalculation == (1237))
                                                                {
                                                                    if (Weapon.Contains("DA") || Weapon.Contains("DN") || Weapon.Contains(("DJ")))
                                                                    {
                                                                        User.rPoints += _Zombie.Points;
                                                                        currentRoom.zombiePoints += _Zombie.Points;
                                                                        block21 = "2.0000";
                                                                    }
                                                                    else
                                                                    {
                                                                        User.rPoints += _Zombie.Points * 2;
                                                                        currentRoom.zombiePoints += _Zombie.Points * 2;
                                                                        block21 = "99.0000";
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    User.rPoints += _Zombie.Points;
                                                                    currentRoom.zombiePoints += _Zombie.Points;
                                                                }

                                                                _Zombie.RespawnTick = Structure.timestamp + 3;
                                                                _Zombie.Health = 0;
                                                                _Zombie.Death = true;

                                                                currentRoom.highestKills++;
                                                                currentRoom.zombieKills++;

                                                                subtype = Subtype.ServerKill;
                                                                sendBlocks[8] = TargetID.ToString();
                                                                sendBlocks[22] = block21;

                                                            if (currentRoom.Mode == 11)
                                                            {
                                                                if (_Zombie.name == "Chariot" || _Zombie.name == "Crusher" || _Zombie.name == "Heavy" || _Zombie.name == "Madman" || _Zombie.name == "Maniac" || _Zombie.name == "Bomber" || _Zombie.name == "Defender")
                                                                {
                                                                    if (_Zombie.name == "Chariot") currentRoom.spawnedChariots--;
                                                                    if (_Zombie.name == "Crusher") currentRoom.spawnedCrushers--;
                                                                    if (_Zombie.name == "Heavy") currentRoom.spawnedHeavys--;
                                                                    if (_Zombie.name == "Madman") currentRoom.spawnedMadmans--;
                                                                    if (_Zombie.name == "Maniac") currentRoom.spawnedManiacs--;
                                                                    if (_Zombie.name == "Bomber") currentRoom.spawnedBombers--;
                                                                    if (_Zombie.name == "Defender") currentRoom.spawnedDefenders--;
                                                                    //if (_Zombie.name == "") currentRoom.spawneds--;
                                                                }

                                                                if (_Zombie.name == "Breaker")
                                                                {
                                                                    currentRoom.Stage4.Stop();
                                                                    currentRoom.milliSec = currentRoom.Stage4.ElapsedMilliseconds;
                                                                    currentRoom.BossKilled = true;
                                                                }
                                                                if (_Zombie.name == "Bomber")
                                                                    currentRoom.NumBomber--;

                                                                if (currentRoom.zombieKills == currentRoom.TimeZombie && currentRoom.ZombieDifficulty == 0 && currentRoom.Stage == 0)
                                                                {
                                                                    currentRoom.Stage1.Stop();//Stop contatore tempo stage1
                                                                    currentRoom.milliSec = currentRoom.Stage1.ElapsedMilliseconds;
                                                                    currentRoom.InitialTime += 480000;
                                                                    currentRoom.send(new PACKET_SCORE_BOARD_AI_TIMEATTACK(currentRoom, User, currentRoom.milliSec));
                                                                    currentRoom.milliSec = 0;
                                                                }
                                                                if (currentRoom.zombieKills == 1 && currentRoom.ZombieDifficulty == 1 && currentRoom.Stage == 0)
                                                                {
                                                                    currentRoom.Stage1.Stop();//Stop contatore tempo stage1
                                                                    currentRoom.milliSec = currentRoom.Stage1.ElapsedMilliseconds;
                                                                    currentRoom.InitialTime += 480000;
                                                                    currentRoom.send(new PACKET_SCORE_BOARD_AI_TIMEATTACK(currentRoom, User, currentRoom.milliSec));
                                                                    currentRoom.milliSec = 0;
                                                                }
                                                            }
                                                        }
                                                        }
                                                    }
                                                }
                                            }
                                            else // se player
                                            {
                                                //Damage = _Zombie.dodamage;
                                                virtualZombie _Zombie = currentRoom.GetZombieById(TargetID);
                                                if (!User.GodMode) User.Health = User.Health - _Zombie.doDamage;
                                                if (User.Health > 0)
                                                {
                                                   
                                                    sendBlocks[15] = (User.Health + Damage).ToString();
                                                    sendBlocks[16] = User.Health.ToString();
                                                }
                                                else
                                                {
                                                    if (User.GodMode)
                                                    {
                                                        User.TheoreticalDeaths++;
                                                        User.send((Packet)new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> Theoretical Deaths : " + (object)User.TheoreticalDeaths, 999L, "SYSTEM"));
                                                        User.Health = 1000;
                                                    }
                                                    if (User.isSpawned == true)
                                                    {
                                                    subtype = Subtype.ServerKill;
                                                    User.rDeaths++;
                                                    }

                                                    User.Health = 0;
                                                    User.isSpawned = false;

                                                    if (currentRoom.Players.Count > 1)
                                                    {
                                                        currentRoom.send(new PACKET_ZOMBIR_CHANGE_ENEMY(currentRoom, User.RoomSlot));
                                                    }

                                                    if (User.rDeaths == 5 && currentRoom.Mode == 11 && currentRoom.ZombieDifficulty == 0 && currentRoom.getSideCount(0) == 0 || User.rDeaths == 3 && currentRoom.Mode == 11 && currentRoom.ZombieDifficulty == 1 && currentRoom.getSideCount(0) == 0)
                                                    {
                                                        User.send(new PACKET_TIMEATTACK_ENDLOSE(currentRoom));
                                                    }
                                                }
                                            }
                                            //User.breaked = true;
                                        }
                                        else
                                        {
                                            #endregion

                                            if (Damage < 0 || User.Health <= 0 && currentRoom.AllowedAfterKill(Weapon) == false || User.isSpawned == false && currentRoom.AllowedAfterKill(Weapon) == false) return;

                                            virtualUser Target = currentRoom.getPlayer(TargetID);
                                            if (currentRoom.getSide(User) == currentRoom.getSide(Target) && currentRoom.Mode != 1 && currentRoom.Channel != 3 && currentRoom.Mode != 12) return;
                                            if (Target != null && Target.isSpawned)
                                            {

                                                if (Target.SpawnProtection > 0 || currentRoom.getSide(User) == currentRoom.getSide(Target) && currentRoom.Mode != 1 && currentRoom.Channel != 3 && currentRoom.Mode != 12 || currentRoom.RoomStatus == 1) return;
                                                if (currentRoom.RoomType == 2)
                                                { if (Weapon.StartsWith("DA")) User.disconnect(); }

                                                if (Target.Health - Damage <= 0)
                                                {
                                                    if (Target.GodMode)
                                                    {
                                                        Target.TheoreticalDeaths++;
                                                        Target.send((Packet)new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> Theoretical Deaths : " + (object)Target.TheoreticalDeaths, 999L, "SYSTEM"));
                                                        Target.Health = 1000;
                                                        return;
                                                    }
                                                }

                                                if (currentRoom.Mode == 6)
                                                {
                                                    User.TotalWarPoint = 0;
                                                    Target.TotalWarSupport += 2;

                                                    sendBlocks[19] = Convert.ToString(User.TotalWarPoint);
                                                    sendBlocks[20] = Convert.ToString(User.TotalWarSupport);

                                                    switch (currentRoom.getSide(User))
                                                    {
                                                        case 0: currentRoom.TotalWarDerb += 5; currentRoom.TotalWarNIU += 2; break;
                                                        case 1: currentRoom.TotalWarNIU += 5; currentRoom.TotalWarDerb += 2; break;
                                                    }
                                                }

                                                //MISSILI DANNO

                                                if (Weapon.StartsWith("DJ"))
                                                {
                                                    if (HeadshotCalculation <= 100 && HeadshotCalculation >= 95)
                                                    {
                                                        Damage -= (Damage * 30 / 100);
                                                    }
                                                    else if (HeadshotCalculation <= 94 && HeadshotCalculation >= 75)
                                                    {
                                                        Damage -= (Damage * 50 / 100);
                                                    }
                                                    else if (HeadshotCalculation <= 74 && HeadshotCalculation >= 50)
                                                    {
                                                        Damage -= (Damage * 60 / 100);
                                                    }
                                                    else if (HeadshotCalculation <= 49 && HeadshotCalculation >= 0)
                                                    { Damage = 0; }
                                                    User.Rockets++;
                                                }

                                                //BOMBE DANNO

                                                else if (Weapon.StartsWith("DN"))
                                                {
                                                    if (HeadshotCalculation <= 80 && HeadshotCalculation >= 70)
                                                    {
                                                        Damage -= (Damage * 20 / 100);
                                                    }
                                                    else if (HeadshotCalculation <= 70 && HeadshotCalculation >= 40)
                                                    {
                                                        Damage -= (Damage * 65 / 100);
                                                    }
                                                    else if (HeadshotCalculation <= 40 && HeadshotCalculation >= 1)
                                                    {
                                                        Damage -= (Damage * 95 / 100);
                                                    }
                                                    else if (HeadshotCalculation == 0)
                                                    { Damage = 0; }
                                                    User.Granade++;
                                                }
                                                //1244 (piede braccia) -65%
                                                else if (HeadshotCalculation == (1241))
                                                {
                                                    Damage -= (Damage * 65 / 100);
                                                }
                                                // 1240 corpo  -35%
                                                else if (HeadshotCalculation == (1239))
                                                {
                                                    Damage -= (Damage * 35 / 100);
                                                }
                                                //  1238 testa +50%
                                                else if (HeadshotCalculation == (1237))
                                                {
                                                    if (Weapon.Contains("DA") || Weapon.Contains("DN") || Weapon.Contains("DJ") || Weapon.Contains("DM"))
                                                    {
                                                        block21 = "2.000";
                                                    }
                                                    else
                                                    {
                                                        block21 = "99.0000";
                                                        Damage += (Damage * 50 / 100);
                                                        if (Target.Health - Damage <= 0)
                                                        {
                                                            User.rHeadshots++;
                                                            Point += 3;
                                                        }
                                                        if (Target.Health - Damage <= 0 && currentRoom.FirstBlood)
                                                        {
                                                            User.send(new PACKET_TO_CLIENT(1));
                                                        }
                                                    }
                                                }


                                                // Every time that a rockets touch a player it will be counted as shooted rocket

                                                if (User.Granade > 3 || User.Rockets > 13) User.disconnect();

                                                int GetDefence = new Random().Next(0, 7);

                                                if (GetDefence == 4)
                                                {
                                                    if (HeadshotCalculation != (1237) && User.BodyDefence)
                                                    {
                                                        int classe = Target.Class;
                                                        string[] _CostumeE = Target.CostumeE.Split(new char[] { ',' });
                                                        string[] _CostumeM = Target.CostumeM.Split(new char[] { ',' });
                                                        string[] _CostumeS = Target.CostumeS.Split(new char[] { ',' });
                                                        string[] _CostumeA = Target.CostumeA.Split(new char[] { ',' });
                                                        string[] _CostumeH = Target.CostumeH.Split(new char[] { ',' });

                                                        if (classe == 0 && _CostumeE[0] != "BA01")
                                                        {
                                                            string[] CodeCostume = DB.runReadRow("SELECT headshot FROM costume_defence WHERE code='" + _CostumeE[0] + "'");
                                                            double coefficente = Convert.ToInt32(CodeCostume[0]);
                                                            double RCoef = (coefficente / 100.0);
                                                            double vOut = Convert.ToDouble(Damage);
                                                            Damage = (int)(vOut * RCoef);
                                                        }
                                                        if (classe == 1 && _CostumeM[0] != "BA02")
                                                        {
                                                            string[] CodeCostume = DB.runReadRow("SELECT headshot FROM costume_defence WHERE code='" + _CostumeM[0] + "'");
                                                            double coefficente = Convert.ToInt32(CodeCostume[0]);
                                                            double RCoef = (coefficente / 100.0);
                                                            double vOut = Convert.ToDouble(Damage);
                                                            Damage = (int)(vOut * RCoef);
                                                        }
                                                        if (classe == 2 && _CostumeS[0] != "BA03")
                                                        {
                                                            string[] CodeCostume = DB.runReadRow("SELECT headshot FROM costume_defence WHERE code='" + _CostumeS[0] + "'");
                                                            double coefficente = Convert.ToInt32(CodeCostume[0]);
                                                            double RCoef = (coefficente / 100.0);
                                                            double vOut = Convert.ToDouble(Damage);
                                                            Damage = (int)(vOut * RCoef);
                                                        }
                                                        if (classe == 3 && _CostumeA[0] != "BA04")
                                                        {
                                                            string[] CodeCostume = DB.runReadRow("SELECT headshot FROM costume_defence WHERE code='" + _CostumeA[0] + "'");
                                                            double coefficente = Convert.ToInt32(CodeCostume[0]);
                                                            double RCoef = (coefficente / 100.0);
                                                            double vOut = Convert.ToDouble(Damage);
                                                            Damage = (int)(vOut * RCoef);
                                                        }
                                                        if (classe == 4 && _CostumeH[0] != "BA05")
                                                        {
                                                            string[] CodeCostume = DB.runReadRow("SELECT headshot FROM costume_defence WHERE code='" + _CostumeH[0] + "'");
                                                            double coefficente = Convert.ToInt32(CodeCostume[0]);
                                                            double RCoef = (coefficente / 100.0);
                                                            double vOut = Convert.ToDouble(Damage);
                                                            Damage = (int)(vOut * RCoef);
                                                        }
                                                    }
                                                    if (HeadshotCalculation == (1241) && User.BodyDefence)
                                                    {

                                                        int classe = Target.Class;
                                                        string[] _CostumeE = Target.CostumeE.Split(new char[] { ',' });
                                                        string[] _CostumeM = Target.CostumeM.Split(new char[] { ',' });
                                                        string[] _CostumeS = Target.CostumeS.Split(new char[] { ',' });
                                                        string[] _CostumeA = Target.CostumeA.Split(new char[] { ',' });
                                                        string[] _CostumeH = Target.CostumeH.Split(new char[] { ',' });

                                                        if (classe == 0 && _CostumeE[0] != "BA01")
                                                        {
                                                            string[] CodeCostume = DB.runReadRow("SELECT body FROM costume_defence WHERE code='" + _CostumeE[0] + "'");
                                                            double coefficente = Convert.ToInt32(CodeCostume[0]);
                                                            double RCoef = (coefficente / 100.0);
                                                            double vOut = Convert.ToDouble(Damage);
                                                            Damage = (int)(vOut * RCoef);
                                                        }
                                                        if (classe == 1 && _CostumeM[0] != "BA02")
                                                        {
                                                            string[] CodeCostume = DB.runReadRow("SELECT body FROM costume_defence WHERE code='" + _CostumeM[0] + "'");
                                                            double coefficente = Convert.ToInt32(CodeCostume[0]);
                                                            double RCoef = (coefficente / 100.0);
                                                            double vOut = Convert.ToDouble(Damage);
                                                            Damage = (int)(vOut * RCoef);
                                                        }
                                                        if (classe == 2 && _CostumeS[0] != "BA03")
                                                        {
                                                            string[] CodeCostume = DB.runReadRow("SELECT body FROM costume_defence WHERE code='" + _CostumeS[0] + "'");
                                                            double coefficente = Convert.ToInt32(CodeCostume[0]);
                                                            double RCoef = (coefficente / 100.0);
                                                            double vOut = Convert.ToDouble(Damage);
                                                            Damage = (int)(vOut * RCoef);
                                                        }
                                                        if (classe == 3 && _CostumeA[0] != "BA04")
                                                        {
                                                            string[] CodeCostume = DB.runReadRow("SELECT body FROM costume_defence WHERE code='" + _CostumeA[0] + "'");
                                                            double coefficente = Convert.ToInt32(CodeCostume[0]);
                                                            double RCoef = (coefficente / 100.0);
                                                            double vOut = Convert.ToDouble(Damage);
                                                            Damage = (int)(vOut * RCoef);
                                                        }
                                                        if (classe == 4 && _CostumeH[0] != "BA05")
                                                        {
                                                            string[] CodeCostume = DB.runReadRow("SELECT body FROM costume_defence WHERE code='" + _CostumeH[0] + "'");
                                                            double coefficente = Convert.ToInt32(CodeCostume[0]);
                                                            double RCoef = (coefficente / 100.0);
                                                            double vOut = Convert.ToDouble(Damage);
                                                            Damage = (int)(vOut * RCoef);
                                                        }
                                                    }

                                                }
                                                Target.Health -= Damage;

                                                sendBlocks[16] = Target.Health.ToString(); //
                                                sendBlocks[17] = (Target.Health + Damage).ToString(); // 
                                                sendBlocks[18] = Point + ".0000"; // UPDATE 24.07.2013 - Showen points in the center of the screen when you kill
                                                sendBlocks[22] = block21; // Headshot or other shit handled from SessionID [ TODO: Costume calculate damage ]
                                                sendBlocks[27] = Weapon; // Weapon (NX removes the other 4 chars ( Splitted full is ) - DF01299 = Weapon[4]|Class[1]|Damage[3]

                                                if (Target.Health <= 2)
                                                {
                                                    if (currentRoom.FirstBlood == false)
                                                    {
                                                        currentRoom.send(new PACKET_TO_CLIENT(0));
                                                        currentRoom.FirstBlood = true;
                                                    }
                                                    if (User.rKills > currentRoom.highestKills) { currentRoom.highestKills = User.rKills; }
                                                    if (currentRoom.getSide(Target) == 0) { currentRoom.KillsDeberanLeft--; } else { currentRoom.KillsNIULeft--; }
                                                    subtype = Subtype.ServerKill;

                                                    if (currentRoom.getSide(User) == 0)
                                                    { currentRoom.KillsDerb++; currentRoom.DeathNiu++; }
                                                    else
                                                    { currentRoom.KillsNiu++; currentRoom.DeathDerb++; }

                                                    User.rKills++;
                                                    User.rPoints += Point;
                                                    User.killFromSpawn++;

                                                    Target.Plantings = 0;
                                                    currentRoom.spawnedPlayers--;
                                                    Target.rDeaths++;
                                                    if (currentRoom.Channel == 3 && currentRoom.Mode == 12 && Target.IsEscapeZombie) currentRoom.EscapeZombie--;
                                                    if (currentRoom.Channel == 3 && currentRoom.Mode == 12 && !Target.IsEscapeZombie)
                                                    {
                                                        currentRoom.EscapeHuman--;
                                                        Target.IsEscapeZombie = true;
                                                    }
                                                    Target.rPoints++;
                                                    Target.Health = 0;
                                                    Target.isSpawned = false;
                                                    Target.ClassCode = "-1";
                                                }
                                            }
                                        }
                                        break;
                                    }

                                case Subtype.RepairVehicle:
                                    {
                                        int vehicleId = int.Parse(getBlock(6));
                                        int tarGetSeat = int.Parse(getBlock(7));
                                        Vehicle vehicle = currentRoom.GetVehicleById(vehicleId);
                                        if (vehicle == null || vehicle.Side != currentRoom.getSide(User) && vehicle.Side != -1) return;

                                        if (vehicle.Health >= vehicle.MaxHealth || User.LastRepairTick > Structure.timestamp) return;

                                        double RepairPercentage = 0.075; // 7.5%

                                        string item = ItemManager.GetItemCodeByID(User.Weapon);

                                        switch (item)
                                        {
                                            case "DR01":
                                                {
                                                    RepairPercentage = 0.10; // 10%
                                                    break;
                                                }
                                            case "DR02":
                                                {
                                                    RepairPercentage = 0.15; // 15%
                                                    break;
                                                }
                                            case "DU51":
                                                {
                                                    RepairPercentage = 0.25; // 25%
                                                    break;
                                                }
                                        }

                                        int repair = (int)Math.Truncate(vehicle.MaxHealth * RepairPercentage);

                                        vehicle.Health += repair;
                                        if (vehicle.Health > vehicle.MaxHealth) vehicle.Health = vehicle.MaxHealth;

                                        User.LastRepairTick = Structure.timestamp + 2;

                                        sendBlocks[7] = vehicle.Health.ToString();
                                        sendBlocks[8] = vehicle.MaxHealth.ToString();
                                        break;
                                    }

                                case Subtype.VehicleKill:
                                    {
                                        sendBlocks[7] = sendBlocks[7] + 1;
                                        sendBlocks[15] = "100";
                                        sendBlocks[16] = "0";
                                        sendBlocks[17] = "1000";
                                        break;
                                    }
                                case Subtype.PlaceUse:
                                    {
                                        int plantingID = Convert.ToInt32(Blocks[8]);
                                        string plantingCode = getBlock(27);
                                        virtualPlacment Placment = currentRoom.getPlant(plantingID);
                                        if (User.Health <= 0 || User.isSpawned == false) return;
                                        if (Placment == null || Placment.Used)
                                        {
                                            //Log.AppendError(plantingID + " plant @ room " + currentRoom.ID + " doesn't exit or used"); return;
                                        }
                                        virtualUser Planter = currentRoom.getPlantUser(plantingID);
                                        int PlanterSide = currentRoom.getSide(Planter);
                                        int UserSide = currentRoom.getSide(User);


                                        switch (plantingCode)
                                        {
                                            case "DV01":
                                                {
                                                    if (Planter != null)
                                                    {
                                                        User.Health += 500; // FIXED IDK IF THATS RIGHT BLOCK = 27 NOT 26 SNIFFED BY SdfSdf
                                                        Planter.rPoints += 3;
                                                    }
                                                    break;
                                                }
                                            case "DU01":
                                                {
                                                    if (Planter != null)
                                                    {
                                                        if (User.Equals(Planter) == false && UserSide == PlanterSide && Planter.droppedAmmo < 10)
                                                        {
                                                            Planter.droppedAmmo++;
                                                            Planter.rPoints += 3;
                                                        }
                                                    }
                                                    break;
                                                }
                                            case "DU02":
                                                {
                                                    User.Health -= 50;
                                                    if (User.Health <= 1)
                                                        User.Health = 1;
                                                    if (Planter != null)
                                                    {
                                                        if (User.Equals(Planter) == false && UserSide != PlanterSide && Planter.droppedM14 < 8)
                                                        {
                                                            Planter.droppedM14++;
                                                            Planter.rPoints += 3;
                                                        }
                                                    }
                                                    break;
                                                }
                                            case "DS05":
                                                {
                                                    if (Planter != null)
                                                    {
                                                        if (User.Equals(Planter) == false && UserSide != PlanterSide && Planter.droppedFlash < 6)
                                                        {
                                                            Planter.droppedFlash++;
                                                            Planter.rPoints += 5;
                                                        }
                                                    }
                                                    break;
                                                }
                                        }

                                        if (User.Health >= 1000)
                                            User.Health = 1000;

                                        sendBlocks[10] = User.Health.ToString();

                                        Placment.Used = true;
                                        break;
                                    }
                                case Subtype.UserLimit:
                                    {
                                        if (currentRoom.RoomMasterSlot == User.RoomSlot)
                                        {
                                            if (currentRoom.RoomStatus == 1)
                                            {
                                                currentRoom.UserLimit = !currentRoom.UserLimit;
                                                if (currentRoom.UserLimit == true)
                                                    currentRoom.UserLimitCount = currentRoom.Players.Count;
                                                else
                                                    currentRoom.UserLimitCount = currentRoom.MaxPlayers;
                                                sendBlocks[6] = (currentRoom.UserLimit == true ? 1 : 0).ToString();
                                                currentRoom.LobbyChange = true;
                                            }
                                        }
                                        break;
                                    }
                                case Subtype.WeaponSwitch:
                                    {
                                        if (User.isSpawned == false || User.Health <= 0) return;
                                        User.Weapon = Convert.ToInt32(sendBlocks[6]);
                                        break;
                                    }
                                case Subtype.Heal:
                                    {
                                        /*
                                         * 0 0 2 101 0 0 0 0 0 0 0 0 0 0 // Normaly Heal
                                         * 0 0 2 101 0 0 0 0 1 0 0 0 0 0 // Remove HP
                                         * 0 0 2 101 0 0 0 1 0 0 0 0 0 0 // Medic Box Station
                                        */
                                        if (User.Weapon == 101) return; // If the weapon is stamina
                                        int TargetSlot = Convert.ToInt32(getBlock(6)); // The target 
                                        bool ToHeal = (getBlock(8) == "1" ? false : true); // If user needed to be healed or killed
                                        bool BoxStation = (getBlock(7) == "1" ? true : false); // If is an medic station or just normal healing
                                        if (TargetSlot >= 0 && TargetSlot <= User.Room.MaxPlayers)
                                        {
                                            GameServer.Virtual_Objects.User.virtualUser _Target = User.Room.getPlayer(TargetSlot);
                                            if (_Target != null)
                                            {
                                                if (ToHeal)
                                                {
                                                    if (_Target.Health <= 0 || _Target.Health >= 1000 || (User.Room.Mode != 1 && User.Room.getSide(User) != User.Room.getSide(_Target))) return; // Check for the health and don't let user to heal the other side players

                                                    if (_Target.RoomSlot != User.RoomSlot && _Target.Health <= 300) // Player assist
                                                        User.rPoints += 5;

                                                    switch (User.Weapon)
                                                    {
                                                        case 99: // Adrenaline
                                                            {
                                                                if (_Target.Health < 300)
                                                                    _Target.Health = 300;
                                                                else _Target.Health += 50;
                                                                break;
                                                            }
                                                        case 102: // HP Kit
                                                            {
                                                                _Target.Health += 150;
                                                                break;
                                                            }

                                                        case 94: // Medic_Kit 1
                                                            {
                                                                _Target.Health += 300;
                                                                break;
                                                            }
                                                        case 95: // Medic_Kit 2
                                                            {
                                                                _Target.Health += 450;
                                                                break;
                                                            }
                                                        case 96: // Medic_Kit 3
                                                            {
                                                                _Target.Health += 600;
                                                                break;
                                                            }


                                                    }

                                                    if (BoxStation)
                                                        _Target.Health += 500;

                                                    if (_Target.Health > 1000)
                                                        _Target.Health = 1000;
                                                }
                                                else // If needed to be killed
                                                {
                                                    _Target.Health -= 100; // Remove 1HP for tick!
                                                    if (_Target.Health <= 1) // Send suicide if user has no HP
                                                    {
                                                        subtype = Subtype.Suicide;
                                                        _Target.isSpawned = false;
                                                    }
                                                }

                                                sendBlocks[7] = _Target.Health.ToString();
                                            }
                                        }
                                        break;
                                    }
                                case Subtype.JoinVehicle:
                                    {
                                        int vehicleId = Convert.ToInt32(getBlock(6));
                                        Vehicle vehicle = currentRoom.GetVehicleById(vehicleId);
                                        if (vehicle == null || User.currentVehicle != null || vehicle.Seats.Count < 1 || vehicle.Side != currentRoom.getSide(User) && vehicle.Side != -1 || vehicle.Health <= 0 || User.Health <= 0 || !User.Alive || !vehicle.isJoinable)
                                        {
                                            User.breaked = true;
                                            break;
                                        }

                                        User.currentVehicle = vehicle;
                                        vehicle.TimeWithoutOwner = 0;

                                        vehicle.Join(User);//viene dato il seat e il vehicle all'user

                                        sendBlocks[6] = vehicle.ID.ToString();
                                        sendBlocks[7] = vehicle.GetSeatByUser(User).ID.ToString();
                                        sendBlocks[8] = vehicle.Health.ToString();
                                        sendBlocks[9] = vehicle.MaxHealth.ToString();
                                        sendBlocks[10] = User.currentSeat.MainCT.ToString();
                                        sendBlocks[11] = User.currentSeat.MainCTMag.ToString();
                                        sendBlocks[12] = User.currentSeat.SubCT.ToString();
                                        sendBlocks[13] = User.currentSeat.SubCTMag.ToString();

                                        break;
                                    }
                                case Subtype.ChangeVehicleSeat:
                                    {

                                        if (!currentRoom.GameActive || this.Blocks.Length < 7)
                                        {
                                            User.breaked = true;
                                            break;
                                        }
                                        int ID2 = int.Parse(this.getBlock(6));
                                        if (ID2 < 0 || ID2 > currentRoom.Vehicles.Count)
                                        {
                                            User.breaked = true;
                                            break;
                                        }
                                        int num9 = User.currentSeat.ID;
                                        int num10 = int.Parse(this.Blocks[7]);
                                        Vehicle vehicleById3 = currentRoom.GetVehicleById(ID2);
                                        if (vehicleById3.Side != currentRoom.getSide(User) || vehicleById3 == null || (User.currentSeat.ID == num10 || !vehicleById3.FreeSeat(num10)))
                                        {
                                            User.breaked = true;
                                            break;
                                        }
                                        User.currentSeat.MainCT = int.Parse(sendBlocks[8]);
                                        User.currentSeat.MainCTMag = int.Parse(sendBlocks[9]);
                                        User.currentSeat.SubCT = int.Parse(sendBlocks[10]);
                                        User.currentSeat.SubCTMag = int.Parse(sendBlocks[11]);
                                        vehicleById3.SwitchSeat(num10, User);
                                        sendBlocks[6] = vehicleById3.ID.ToString();
                                        sendBlocks[7] = num10.ToString();
                                        sendBlocks[8] = num9.ToString();
                                        sendBlocks[9] = User.currentSeat.MainCT.ToString();
                                        sendBlocks[10] = User.currentSeat.MainCTMag.ToString();
                                        sendBlocks[11] = User.currentSeat.SubCT.ToString();
                                        sendBlocks[12] = User.currentSeat.SubCTMag.ToString();
                                        break;


                                    }
                                case Subtype.LeaveVehicle:
                                    {


                                        if (!currentRoom.GameActive || this.Blocks.Length < 6)
                                        {
                                            User.breaked = true;
                                            break;
                                        }
                                        int ID3 = int.Parse(this.getBlock(6));
                                        if (ID3 < 0 || ID3 > currentRoom.Vehicles.Count)
                                        {
                                            User.breaked = true;
                                            break;
                                        }
                                        Vehicle vehicleById4 = currentRoom.GetVehicleById(ID3);
                                        if (vehicleById4 == null)
                                        {
                                            User.breaked = true;
                                            break;
                                        }
                                        vehicleById4.TimeWithoutOwner = 0;
                                        User.currentSeat.MainCT = int.Parse(this.Blocks[8]);
                                        User.currentSeat.MainCTMag = int.Parse(this.Blocks[9]);
                                        User.currentSeat.SubCT = int.Parse(this.Blocks[10]);
                                        User.currentSeat.SubCTMag = int.Parse(this.Blocks[11]);
                                        sendBlocks[6] = ID3.ToString();
                                        sendBlocks[7] = User.currentSeat.ID.ToString();
                                        vehicleById4.Leave(User);
                                        User.currentVehicle = null;
                                        break;
                                    }
                                //Nuovo//

                                case Subtype.DamageVehicle:
                                    if (User.Health <= 0)
                                    {
                                        return;
                                        //break;
                                    }
                                    int num19 = int.Parse(this.getBlock(6));
                                    int num20 = int.Parse(this.getBlock(7));
                                    int num21 = 5;
                                    string str7 = this.getBlock(27).Substring(0, 4);



                                    if (num20 < 0 || num20 > currentRoom.Vehicles.Count)
                                        return;
                                    Vehicle vehicleById6 = currentRoom.GetVehicleById(num20);
                                    if (vehicleById6.Side == currentRoom.getSide(User) || vehicleById6.SpawnProtection > 0 || (vehicleById6.Health <= 0 || vehicleById6 == null))
                                    {
                                        return;
                                    }
                                    int Type = num19 == 1 ? 0 : 1;
                                    int num22;
                                    if (User.currentVehicle != null)
                                    {
                                        bool flag2 = int.Parse(this.getBlock(13)) == 0;
                                        if (currentRoom.Mode == 10 && currentRoom.GetIncubatorVehicleId() == num20 && !this.isZombieWeapon(str7))
                                        {
                                            return;
                                        }
                                        num22 = !flag2 ? ItemManager.getVehDamage(User.currentSeat.SubCTCode, Type) : ItemManager.getVehDamage(User.currentSeat.MainCTCode, Type);
                                    }
                                    else
                                    {
                                        if (currentRoom.Mode == 10 && currentRoom.GetIncubatorVehicleId() == num20 && !this.isZombieWeapon(str7))
                                        {
                                            return;
                                        }

                                        num22 = ItemManager.getDamage(str7, Type);
                                        if (str7.StartsWith("DK") || str7.StartsWith("DJ") || str7.StartsWith("DN") || str7.StartsWith("DL"))
                                        {
                                            num22 = ItemManager.getDamage(str7, Type);
                                        }
                                        else if (currentRoom.Mode != 11)
                                        {
                                            num22 = 0;
                                        }
                                        if (currentRoom.Channel == 3 && this.isZombieWeapon(str7))
                                            num22 = 150 * (currentRoom.ZombieDifficulty + 1);
                                    }
                                    if (vehicleById6.Code == "EN17") num22 = ItemManager.getDamage(str7, Type);
                                    if (vehicleById6.Code == "EN17" && User.GodMode) num22 = vehicleById6.Health;
                                    if (vehicleById6.Code == "EN01")
                                        num22 = 250;
                                    else if (vehicleById6.Code == "EJ05" && str7.StartsWith("DK"))
                                      num22 = ItemManager.getDamage(str7, Type);
                                    vehicleById6.Health -= num22;
                                    sendBlocks[16] = vehicleById6.Health.ToString();
                                    sendBlocks[17] = (vehicleById6.Health + num22).ToString();
                                    sendBlocks[27] = str7;
                                    if (vehicleById6.Health >= 0 && vehicleById6.Code == "EN17" && num22 != 0)
                                    {
                                        User.DoorDamageTime++;
                                    }
                                    if (vehicleById6.Health <= 0 && vehicleById6.Code == "EN17")
                                    {
                                        currentRoom.Destructed = true;
                                        // S=> 2639655 30053 3 83104 0 -1 0  è uscito questo
                                        // S=> 2759104 30053 3 219734 0 500220 -1 0 deve uscire cosi
                                        currentRoom.Stage3.Stop();//Stop time Stage 3
                                        currentRoom.milliSec = currentRoom.Stage3.ElapsedMilliseconds;
                                        currentRoom.send(new PACKET_SCORE_BOARD_AI_TIMEATTACK(currentRoom, User, currentRoom.milliSec));
                                        if (currentRoom.ZombieDifficulty == 0)
                                        {
                                            currentRoom.send(new PACKET_TIMEATTACK_END(currentRoom));
                                            currentRoom.milliSec = 0;
                                            User.send(new PACKET_KILL_EVENT(User));
                                        }
                                        else
                                        {
                                            currentRoom.InitialTime += 480000;
                                        }
                                    }

                                    if (vehicleById6.Health <= 0)
                                    {
                                        currentRoom.CheckForMission(User, num20);
                                        if (vehicleById6.Players.Count > 0)
                                        {
                                            ((object[])Blocks)[3] = "152";
                                            foreach (VehicleSeat virtualVehicleSeats in vehicleById6.Seats.Values)
                                            {
                                                if (virtualVehicleSeats.seatOwner != null && virtualVehicleSeats.seatOwner.currentVehicle == vehicleById6 && virtualVehicleSeats.seatOwner.Health > 0)
                                                {
                                                    if (currentRoom.getSide(virtualVehicleSeats.seatOwner) == 0)
                                                        --currentRoom.KillsDeberanLeft;
                                                    else
                                                        --currentRoom.KillsNIULeft;

                                                    if (currentRoom.Mode == 6)
                                                    {
                                                        User.TotalWarPoint += 5;
                                                        virtualVehicleSeats.seatOwner.TotalWarSupport += 2;
                                                    }

                                                    int num6 = virtualVehicleSeats.seatOwner.RoomSlot;
                                                    ++virtualVehicleSeats.seatOwner.rPoints;
                                                    ++virtualVehicleSeats.seatOwner.rDeaths;
                                                    User.rPoints += num21;
                                                    ++User.rKills;
                                                    virtualVehicleSeats.seatOwner.Health = 0;
                                                    virtualVehicleSeats.seatOwner.isSpawned = false;

                                                    sendBlocks[7] = Convert.ToString(virtualVehicleSeats.seatOwner.RoomSlot);
                                                    /*Blocks[19] = Convert.ToString((currentRoom.Mode == 6 ? User.TotalWarPoint : Convert.ToInt32(Blocks[19])));
                                                    Blocks[20] = Convert.ToString((currentRoom.Mode == 6 ? User.TotalWarPoint : Convert.ToInt32(Blocks[20])));*/
                                                    sendBlocks[21] = Convert.ToString(num21 + ".00");
                                                    sendBlocks[22] = "2.000";
                                                    sendBlocks[27] = Convert.ToString(str7);

                                                    currentRoom.send((Packet)new PACKET_ROOM_DATA(User, new object[28]
                        {
                     this.getBlock(0),
                     this.getBlock(1),
                     this.getBlock(2),
                     152,
                     this.getBlock(4),
                     this.getBlock(5),
                     this.getBlock(6),
                     virtualVehicleSeats.seatOwner.RoomSlot,
                     this.getBlock(8),
                     this.getBlock(9),
                     this.getBlock(10),
                     this.getBlock(11),
                     this.getBlock(12),
                     this.getBlock(13),
                     this.getBlock(14),
                     this.getBlock(15),
                     this.getBlock(16),
                     this.getBlock(17),
                     this.getBlock(18),
                     this.getBlock(19),
                     this.getBlock(20),
                     "5.000",
                     "2.000",
                     this.getBlock(23),
                     this.getBlock(24),
                     this.getBlock(25),
                     this.getBlock(26),
                     str7
                        }));
                                                }
                                            }
                                        }
                                        currentRoom.send((Packet)new PACKET_VEHICLE_EXPLODE(currentRoom.ID, vehicleById6.ID, User.RoomSlot));
                                        if ((num20 == 25 || num20 == 24 || num20 == 23) && (currentRoom.MapID == 42 && currentRoom.Mode == 5))
                                        {
                                            currentRoom.endGame();
                                            return;
                                        }
                                        else if (num20 == 0 && currentRoom.MapID == 56 && currentRoom.Mode == 5)
                                        {
                                            currentRoom.endGame();
                                            return;
                                        }
                                        else
                                            return;
                                    }
                                    break;
                            //----//
                                case Subtype.CaptureFlag:
                                {
                                    /* 
                                     *           Derb NIU  Block6
                                    30000 4 0 2 165 0 0    2 0 0 0 -1 0 0 0 
                                    30000 0 0 2 165 0 0    5 0 0 0 -1 0 0 0
                                    */
                                    //bool Captured { get { return UsersDic.Values.Where(u => u.Alive && getSide(u) == 0 && u != null).Count(); } }
                                    int flagId = Convert.ToInt32(getBlock(6));
                                    if (currentRoom.MapData != null)
                                    {
                                        if (flagId == currentRoom.MapData.flag1 || flagId == currentRoom.MapData.flag2) return;
                                    }
                                    int flagSide = currentRoom.Flags[flagId];
                                    int mySide = currentRoom.getSide(User);
                                    if (flagSide == mySide) return;
                                    if (flagSide == -1) // Neutral
                                    {
                                        currentRoom.Flags[flagId] = mySide;
                                        if (currentRoom.getSide(User) == 0) currentRoom.FlagsDerb++;
                                        else currentRoom.FlagsNiu++;
                                        if (User.rFlags < ConfigServer.MaxFlags) User.rPoints += ConfigServer.OnTakeFlag;
                                        User.rFlags++;
                                    }
                                    else
                                    {
                                        currentRoom.Flags[flagId] = -1; // Make it neutral before acquiring it
                                    }
                                    int totalWarPoints = (flagId == 8 ? 30 : 15);
                                    User.TotalWarPoint += totalWarPoints;

                                    sendBlocks[6] = Convert.ToString(flagId); // Flag ID
                                    sendBlocks[7] = Convert.ToString(currentRoom.Flags[flagId]); // Actual State
                                    sendBlocks[8] = Convert.ToString(flagSide); // Old State
                                    sendBlocks[9] = Convert.ToString(flagId); // Flag ID
                                    sendBlocks[11] = Convert.ToString((currentRoom.Mode == 8 ? User.TotalWarPoint : 0));

                                    break;
                                }
                                case Subtype.Flag:
                                    {
                                        if (!currentRoom.GameActive) return;
                                        int flagId = Convert.ToInt32(getBlock(6));
                                        if (currentRoom.MapData != null)
                                        {
                                            if (flagId == currentRoom.MapData.flag1 || flagId == currentRoom.MapData.flag2) return;
                                        }
                                        int flagSide = currentRoom.Flags[flagId];
                                        int mySide = currentRoom.getSide(User);

                                        //Log.AppendText(mySide + " -> taking " + flagId + " [" + flagSide + "]");

                                        if (flagSide == mySide) return;
                                    
                                        bool RemovePoints = (currentRoom.Rounds > 2);

                                        if (flagSide == -1) // Neutral
                                        {
                                            currentRoom.Flags[flagId] = mySide;
                                            if (User.rFlags < ConfigServer.MaxFlags) User.rPoints += ConfigServer.OnTakeFlag;
                                            User.rFlags++;
                                        }
                                        else
                                        {
                                            currentRoom.Flags[flagId] = -1; // Make it neutral before acquiring it
                                        }

                                        if (currentRoom.Mode != 6)
                                        {
                                            if (RemovePoints)
                                            {
                                                switch (mySide)
                                                {
                                                    case 0: currentRoom.KillsNIULeft--; break;
                                                    case 1: currentRoom.KillsDeberanLeft--; break;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            int totalWarPoints = (flagId == 8 ? 30 : 15);
                                            User.TotalWarPoint += totalWarPoints;
                                        }

                                        sendBlocks[6] = Convert.ToString(flagId); // Flag ID
                                        sendBlocks[7] = Convert.ToString(currentRoom.Flags[flagId]); // Actual State
                                        sendBlocks[8] = Convert.ToString(flagSide); // Old State
                                        sendBlocks[9] = Convert.ToString(flagId); // Flag ID
                                        sendBlocks[11] = Convert.ToString((currentRoom.Mode == 8 ? User.TotalWarPoint : 0));

                                        currentRoom.UpdateTime();
                                        break;
                                    }

                            }


                            sendBlocks[3] = ((int)subtype).ToString();

                            if (subtype == Subtype.ServerRoomReady)
                            {
                                // User.send(new PACKET_INIT_PLAYER(currentRoom));
                                User.send(new PACKET_MAP_DATA(currentRoom, User));
                                User.send(new PACKET_ROOM_DATA(User, sendBlocks));
                            }
                            else if (subtype == Subtype.KickUser)
                            {
                                foreach (virtualUser Players in currentRoom.getTeamPlayer(currentRoom.getSide(User)))
                                    Players.send(new PACKET_ROOM_DATA(User, sendBlocks));
                            }
                            else
                            {
                                if (User.breaked == false/* && User.F5 == false*/)
                                {
                                    if (sendBlocks[3] == "9") { User.send(new PACKET_ROOM_DATA(User, sendBlocks)); User.BackedToRoom = true; currentRoom.BackedToRoom++; }

                                    if (sendBlocks[3] != "9") currentRoom.send(new PACKET_ROOM_DATA(User, sendBlocks));

                                    if (currentRoom.BackedToRoom >= currentRoom.PlayerCount)
                                    {
                                        currentRoom.BackToRoom = false;
                                        currentRoom.BackedToRoom = 0;
                                    }
                                }
                                if (sendBlocks[3] == "50") User.F5 = !User.F5;
                                if (User.isReady == true && currentRoom.AutoStart)
                                {
                                    currentRoom.UserReadys++;

                                    if (currentRoom.PlayerCount == currentRoom.UserReadys)
                                    {
                                        currentRoom.AllPlayerReady = true;
                                        // currentRoom.CheckReadys();

                                    }
                                }
                                if (User.isReady == false && currentRoom.AutoStart)
                                {
                                    currentRoom.UserReadys--;
                                    currentRoom.AllPlayerReady = false;
                                }
                                User.breaked = false;
                            }

                            if (currentRoom.LobbyChange)
                            {
                                foreach (virtualUser _User in UserManager.getUsersInChannel(currentRoom.Channel, false))
                                    if (_User.Page == Math.Floor(Convert.ToDecimal(currentRoom.ID / 14)))
                                    {
                                        _User.send(new PACKET_ROOMLIST_UPDATE(currentRoom));
                                    }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    //Log.AppendError(ex.Message + " at handle room data");
                }
            }
        }
    }