using ReBornWarRock_PServer.GameServer;
using ReBornWarRock_PServer.GameServer.Managers;
using ReBornWarRock_PServer.GameServer.Networking.Handlers;
using ReBornWarRock_PServer.GameServer.Networking.Packets;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using System.Collections.Concurrent;
using System.IO;
using System.Xml;

namespace ReBornWarRock_PServer.GameServer.Virtual_Objects.Room
{
    class virtualPlacment
    {
        public int ID;
        public virtualUser Planter;
        public string Code;
        public bool Used;
    }

    class virtualRoom
    {

        /*Escape Mode*/
        public int EscapeHack = 10;
        public int ReverseCount = 20;
        public int HackingPause = 0;
        public bool InHacking = false;
        /*-----------*/

        public int AutoCount = 0;
        //Pow Camp
        public int InitialTime = 720000;
        public int ZombieSpawnPlace = 0;
        public int TryZombie = 0;
        public int TimeZombie = 0;
        public int ZombieFollower = -1;
        public int TotalWarDerb = 0;
        public int TotalWarNIU = 0;
        public int BackedToRoom = 0;

        #region DeathHill
        public Dictionary<virtualUser, int> DeathFlags = new Dictionary<virtualUser, int>();
        public int FlagsNiu = 0;
        public int FlagsDerb = 0;
        #endregion

        #region Conquest

        public int ConquestCountdown = 30;
        public int WinningTeam = -1;

        public bool runningCountdown = false;

        #endregion
        #region Escape
        public int EscapeZombie = 0;
        public int EscapeHuman = 0;

        #endregion

        /*Test Packet End Game FreeWar*/
        public int KillsDerb = 0;
        public int DeathDerb = 0;
        public int KillsNiu = 0;
        public int DeathNiu = 0;
        /**/

        public bool BackToRoom = false;

        public int HackPercentageA = 0;
        public int HackPercentageB = 0;
        public bool PickuppedC4 = false;
        //public string Mission1 = "";
        public bool timeattack = true;
        public bool LastHackBase = false;
        public int SiegeWarTime = 0;
        //public string SiegeWarC4User = "";
        //calcolo tempo millisecondi
        public Stopwatch Stage1 = new Stopwatch();
        public Stopwatch Stage2 = new Stopwatch();
        public Stopwatch Stage3 = new Stopwatch();
        public Stopwatch Stage4 = new Stopwatch();

        public bool IsTimeOpenDoor = false;
        public int TimeToOpenDoor = 0;
        public ConcurrentDictionary<int, virtualUser> UsersDic = new ConcurrentDictionary<int, virtualUser>();
        public Dictionary<int, virtualZombie> Zombies = new Dictionary<int, virtualZombie>();
        public Dictionary<string, int> SupplyBox = new Dictionary<string, int>();


        public long milliSec = 0;
        public virtualUser SiegeWarC4User;
        public string Mission1;
        public string Mission2;
        public string Mission3;
        public int PowPlayer = 0; //Time Attack
        public int PowSupply = 0;
        public int IntoPassing = 0;
        public bool Destructed = false;
        public int DropID = 0;
        public int DropThisWave = 0;
        public int DropKills = 0;
        public bool LobbyChange = false;
        public int PXItemID = 0;
        public bool BossKilled = false;
        public bool WeaponsGot = false;

        int numero = 5;
        int asd = 0;
        //--Test Pow FreeWar--//


        public bool WantToStart = false;
        //----//
        public int NewMode = 0;
        public int SubNewMode = 0;
        public int UserReadys;
        public bool AllPlayerReady = false;
        public bool Restart = true;


        /**/
        public int ID;
        public int Channel = 1;
        public String Name = "Default-Room";
        public int EnablePassword = 0;
        public String Password = "NULL";
        public bool SuperMaster = false;
        public int MapID = 0;
        public int Kills = 2;
        public int PremiumOnly = 0;
        public int VoteKick = 0;
        public int RoomStatus = 1; // 1 Waiting - 0 & 2 Playing
        public int Mode = 0; // Mode : 0 = Explosive, 1 = FFA, 2 = Deathmatch, 3 = Conquest
        public int RoomType = 0;
        public int LevelLimit = 0;
        public int Ping = 0;
        public bool AutoStart = false;
        public int Rounds = 3; // 0 = 1, 1 = 3, 2 = 5, 3 = 7, 4 = 9
        public int VehicleRespawnCount = 0;
        public bool FirstBlood = false;
        public bool UserLimit = false;
        public int UserLimitCount = 0;
        public bool VehiclesChecked = false;
        public int cNIUExplosivePoints = 0;
        public int cDerbExplosivePoints = 0;
        public bool Sleep = false;
        public int FFAKillPoints = 10;
        public int ZombieDifficulty = 1;
        public bool TunnelCheck = false;
        public virtualUser[] oPlayers = new virtualUser[0];
        public ArrayList NIUPlayers = new ArrayList();
        public ArrayList DeberanPlayers = new ArrayList();


        public int RoundTimeLeft = 180000;
        public int RoundTimeSpent = 0;
        //public int[] Flags = new int[32];
        public int[] Flags;

        private Hashtable Users = new Hashtable();
        private int RoomMaster = 0;
        private int MaxUsers = 0;

        #region In-Game

        public bool GameActive = false;
        public bool EndGameFreeze = false;

        public int LastTick = 0;

        public DateTime RoundEnd = DateTime.Now;
        public bool bombPlanted = false;
        public bool bombDefused = false;
        public int explosiveRounds = 0;
        public int dmRounds = 0;

        public int highestKills = 0;
        public int SpawnLocation = -1;

        public int KillsNIULeft = 10;
        public int KillsDeberanLeft = 10;
        public int TimeLimit = 4;
        public int VoteKickTime = 0;
        public int KickSeat = -1;
        public int KickYes = 0;
        public int KickNo = 0;
        public int RespawnVehicleCount = 120;
        public bool KickAlready = false;
        public string[] KickString = new string[1];

        public int waitExplosiveTime = 0;
        public bool isNewRound = false;
        public int spawnedPlayers = 0;
        public bool readyZombie = false;
        public bool beginSpawn = false;

        public ArrayList Spectators = new ArrayList();
        public virtualMapData MapData = null;
        public Dictionary<int, Vehicle> Vehicles = new Dictionary<int, Vehicle>();
        public int debug = 0;

        #endregion

        public virtualRoom() { }

        ~virtualRoom()
        {
            GC.Collect();
        }

        public int MaxPlayers
        {
            get { return MaxUsers; }
            set { MaxUsers = value; }
        }

        public ArrayList Players
        {
            get
            {
                return new ArrayList(Users.Values);
            }
        }

        public bool haveItem(virtualUser User, string Weapon)
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

        public virtualUser getSpectatorByID(int SpectatorID)
        {
            foreach (virtualUser SpectatorUser in Spectators)
            {
                if (SpectatorUser.SpectatorID == SpectatorID)
                    return SpectatorUser;
            }
            return null;
        }

        public int getMaxIDSpectator
        {
            get
            {
                int SpectatorID = 1;
                foreach (virtualUser SpectatorUser in Spectators)
                {
                    if (SpectatorUser.SpectatorID > SpectatorID)
                        SpectatorID = SpectatorUser.SpectatorID;
                }
                return SpectatorID;
            }
        }

        public int getIDSpectator(virtualUser User)
        {
            for (int I = 0; I < Spectators.Count; I++)
            {
                if (getSpectatorByID(I) == User)
                    return I;
            }
            int Count = getMaxIDSpectator + 1;
            if (Count > 10) Count -= 10;
            return Count;
        }

        public bool addSpectator(virtualUser usr)
        {
            if (Spectators.Count >= 10) return false;
            if (Spectators.Contains(usr) == false && usr != null)
            {
                int id = Spectators.Count;
                usr.isSpectating = true;
                usr.Room = this;
                usr.RoomSlot = 32 + id; // Spectator ID
                usr.SpectatorID = id;
                Spectators.Add(usr);
                return true;
            }
            return false;
        }

        public void removeSpectator(virtualUser User)
        {
            if (Spectators.Contains(User))
            {
                send(new PACKET_ROOM_UDP_SPECTATE(User, this));
                User.send(new PACKET_SPECTATE_ROOM());
                User.send(new PACKET_ROOM_LIST(User, 0));
                User.Room = null;
                User.RoomSlot = 0;
                User.SpectatorID = -1;
                User.isSpectating = false;
                //User.disconnect();
                Spectators.Remove(User);
            }
        }

        public int getPlayers()
        {
            ArrayList PlayerList = new ArrayList();
            int ValuePlayers = 0;
            for (int i = 0; i < MaxPlayers; i++)
            {
                if (oPlayers.GetValue(i) is virtualUser)
                {
                    ValuePlayers++;
                }
            }
            return ValuePlayers;
        }

        public int PlayerCount { get { return Users.Count; } }
        public int RoomMasterSlot { get { return RoomMaster; } }

        public void send(Packet p)
        {
            foreach (virtualUser RoomUser in ((Hashtable)Users.Clone()).Values)
            {
                RoomUser.send(p);
            }
            foreach (virtualUser SpectatorUser in Spectators)
            {
                SpectatorUser.send(p);
            }
        }

        public ArrayList getTeamPlayer(int Team)
        {
            ArrayList ar = new ArrayList();
            foreach (User.virtualUser c in Players)
            {
                if (getSide(c) == Team)
                {
                    ar.Add(c);
                }
            }
            return ar;
        }

        public bool isJoinAble()
        {
            if (Channel == 3 && RoomStatus != 1)
                return false;

            if (Players.Count == MaxPlayers || UserLimit)
                // Room is full
                return false;

            if (RoomStatus != 1 && Mode == 2 && (KillsNIULeft <= 30 || KillsDeberanLeft <= 30))
            {
                // Less then 30 Kills Left
                return false;
            }

            if (RoomStatus != 1 && Mode == 1 && FFAKillPoints <= 10)
            {
                // Less then 10 Kills Left - FFA
                return false;
            }
            return true;
        }

        /// <summary>
        /// Spawn all vehicles
        /// </summary>
        private void SpawnVehicles()
        {
            if (MapData != null)
            {
                int VehicleCount = 0;
                Vehicles.Clear();
                if (MapData.VehicleString != null && MapData.VehicleString != string.Empty && MapData != null)
                {
                    string[] VehiclesCodes = MapData.VehicleString.Split(new char[] { ';' });
                    foreach (string sCode in VehiclesCodes)
                    {
                        VehicleManager VehicleInfo = VehicleManager.getVehicleInfoByCode(sCode);
                        if (VehicleInfo != null)
                        {
                            Vehicle newVehicle = new Vehicle(VehicleCount, sCode, VehicleInfo.Name, VehicleInfo.MaxHealth, VehicleInfo.MaxHealth, VehicleInfo.RespawnTime, VehicleInfo.Seats, VehicleInfo.isJoinable);
                            Vehicles.Add(VehicleCount, newVehicle);
                            VehicleCount++;
                        }
                        else
                        {
                            Log.AppendError("Could not find the vehicle with the code " + sCode + "!");
                        }
                    }
                }
            }
        }


        public Vehicle GetVehicleById(int id)
        {
            if (this.Vehicles.ContainsKey(id))
            {
                return this.Vehicles[id];
            }
            return null;
        }
        public void RespawnVehicle(int ID)
        {
            try
            {
                Vehicle vehicleById = this.GetVehicleById(ID);
                if (vehicleById.RespawnTime == -1 || vehicleById == null)
                    return;
                vehicleById.RespawnTick = 0;
                vehicleById.SpawnProtection = 5;
                vehicleById.Health = vehicleById.MaxHealth;
                vehicleById.LoadSeats(vehicleById.SeatString);
                vehicleById.ChangedCode = string.Empty;
                vehicleById.TimeWithoutOwner = 0;
                this.send((Packet)new PACKET_VEHICLE_RESPAWN(ID));
            }
            catch
            {
            }
        }

        public int GetIncubatorVehicleId()
        {
            if (this.Channel == 3 && this.Mode == 10)
            {
                IEnumerable<Vehicle> source = Enumerable.Where<Vehicle>((IEnumerable<Vehicle>)this.Vehicles.Values, (Func<Vehicle, bool>)(r => r.Name.ToUpper() == "FIXED_BRK_INCUBATOR"));
                if (Enumerable.Count<Vehicle>(source) > 0)
                    return Enumerable.FirstOrDefault<Vehicle>(source).ID;
            }
            return -1;
        }

        public bool isPremMap()
        {
            try
            {
                string[] Data = DB.runReadRow("SELECT ispremium FROM `maps` WHERE id=" + MapID);
                if (Data[0] == "0") return false;
                else return true;
            }
            catch
            {
                return false;
            }
        }
        public void sendNewRound(int WinningTeam)
        {
            if (waitExplosiveTime > 5 || !EndGameFreeze)
            {
                ArrayList tempPlayers = Players;
                foreach (virtualUser Spectator in Spectators)
                {
                    tempPlayers.Add(Spectator);
                }

                foreach (virtualUser Player in Players)
                {
                    tempPlayers.Add(Player);

                    if (bombPlanted == true)
                    { WinningTeam = 0; }
                    else if (bombDefused == true)
                    { WinningTeam = 1; }
                    Player.Health = 1000;
                    Player.isSpawned = false;
                    Player.Plantings = 0;
                }

                this.send(new RoomDataNewRound(this, WinningTeam, false));
                if (explosiveRounds != 1) Thread.Sleep(1500);
                this.send(new InitializeNewRound(this));

                RoundTimeLeft = 180000;
                RoundTimeSpent = 0;
                isNewRound = false;
                waitExplosiveTime = 0;
                Sleep = false;
                bombDefused = false;
                bombPlanted = false;
                Plantings.Clear();
            }
            else { return; }
        }

        public void prepareRound(int WinningTeam)
        {
            if (isNewRound == false)
            {
                waitExplosiveTime = 0;
                switch (WinningTeam)
                {
                    case 0: cDerbRounds++; break;
                    case 1: cNiuRounds++; break;
                }
                isNewRound = true;
            }

            Sleep = true;
            ArrayList tempPlayers = Players;

            foreach (virtualUser Player in Spectators)
            {
                Players.Add(Player);
            }
            foreach (virtualUser Player in Players)
            {
                Player.isSpawned = false;
                Player.Rockets = Player.Granade = 0;
            }

            if (explosiveRounds != 1) this.send(new RoomDataNewRound(this, WinningTeam, true));
            
            if (explosiveRounds == 1) isNewRound = false;

            if (WinningTeam == 0 && cDerbRounds >= explosiveRounds && !EndGameFreeze)
            { endGame(); }

            if (WinningTeam == 1 && cNiuRounds >= explosiveRounds && !EndGameFreeze)
            { endGame(); }
        }

        public void RespawnAllVehicle()
        {
            foreach (Vehicle virtualVehicle in this.Vehicles.Values)
                this.RespawnVehicle(virtualVehicle.ID);
        }

        private void ResetZombieRoomStart()
        {
            Wave = 0;
            Stage = 0;
            zTime = 0;
            xKill = 0;
            InitialTime = 720000;
            waveTime = 0;
            spawnedZombies = 0;
            zombiePoints = 0;
            xoldTime = 0;
            calculateDifference = 0;
            toWave = 20;
            waveTime = 15;
            spawnedPlayers = 0;
            freezeZombies = false;
            beginSpawn = false;
            firstWave = true;
            waveReqFinished = false;
            isFinished = false;
            readyZombie = false;
            Destructed = false;
            BossKilled = false;
            if (Mode != 11) resetStats();
            else resetCamps();
        }

        public bool Start()
        {
            try
            {
                foreach (virtualUser RoomUser in Players)
                {
                    if (RoomUser.isReady == false && RoomUser.RoomSlot != RoomMasterSlot)
                    {
                        RoomUser.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Whisper, "SYSTEM >> All players must be ready before start the game!!", RoomUser.SessionID, RoomUser.Nickname));
                        return false;
                    }
                    else if (Channel == 3 && Mode == 12)
                    {
                        if (RoomUser.RoomSlot % 2 == 0 && RoomUser.RoomSlot != 0) RoomUser.IsEscapeZombie = true;
                    }
                    else if (RoomType == 1 && PlayerCount < 4)
                    {
                        RoomUser.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Whisper, "SYSTEM >> Not enough players to start a clanwar!!", RoomUser.SessionID, RoomUser.Nickname));
                        return false;
                    }
                    else if (RoomType == 1 && Rounds < 2)
                    {
                        string Need = null;
                        if (Mode == 0)
                            Need = "5+ rounds";
                        else
                            Need = "100+ kills";
                        RoomUser.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Whisper, "SYSTEM >> Need at least " + Need + " for start the clanwar!!", RoomUser.SessionID, RoomUser.Nickname));
                        return false;
                    }
                    if (Structure.Debug == 0)
                    {
                        if (Channel != 3 && PlayerCount <= 1 || Channel == 3 && Mode == 12 && PlayerCount <= 1)
                        {
                            RoomUser.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Whisper, "SYSTEM >> Need more players!!", RoomUser.SessionID, RoomUser.Nickname));
                            return false;
                        }
                    }
                }

                // Reset Room //
                Vehicles.Clear();
                MapData = virtualMapData.GetMapByID(MapID);
                SpawnLocation = 0;
                GameActive = true;
                EndGameFreeze = false;
                FirstBlood = false;
                isNewRound = false;
                highestKills = WarningCW = cNiuRounds = cDerbRounds = KillsNIULeft = KillsDeberanLeft = VehicleRespawnCount = TotalWarDerb = TotalWarNIU = cDerbExplosivePoints = cNIUExplosivePoints = 0;
                EscapeHuman = EscapeZombie = 0;
                FlagsDerb = FlagsNiu = 0;
                bombPlanted = false;
                bombDefused = false;
                Sleep = false;
                RoundTimeSpent = 0;
                Plantings.Clear();
                ResetZombieRoomStart();
                WantToStart = false;

                Flags = new int[this.MapData.Flags];
                GameActive = CWCheck = true;
                //
                if (Mode == 0)
                {
                    switch (Rounds)
                    {
                        case 0: explosiveRounds = 1; break;
                        case 1: explosiveRounds = 3; break;
                        case 2: explosiveRounds = 5; break;
                        case 3: explosiveRounds = 7; break;
                        case 4: explosiveRounds = 9; break;
                    }
                }
                else if (Mode == 1)
                {
                    FFAKillPoints = 10 + (5 * Rounds);
                }
                else if (Mode == 2 || Mode == 3)
                {
                    switch (Rounds)
                    {
                        case 0: KillsDeberanLeft = KillsNIULeft = 30; break;
                        case 1: KillsDeberanLeft = KillsNIULeft = 50; break;
                        case 2: KillsDeberanLeft = KillsNIULeft = 100; break;
                        case 3: KillsDeberanLeft = KillsNIULeft = 150; break;
                        case 4: KillsDeberanLeft = KillsNIULeft = 200; break;
                        case 5: KillsDeberanLeft = KillsNIULeft = 300; break;
                        case 6: KillsDeberanLeft = KillsNIULeft = 500; break;
                        case 7: KillsDeberanLeft = KillsNIULeft = 999; break;
                    }
                }
                else if (Mode == 4 || Mode == 8)
                {
                    switch (Rounds)
                    {
                        case 0: KillsDeberanLeft = KillsNIULeft = 100; break;
                        case 1: KillsDeberanLeft = KillsNIULeft = 200; break;
                        case 2: KillsDeberanLeft = KillsNIULeft = 300; break;
                        case 3: KillsDeberanLeft = KillsNIULeft = 500; break;
                        case 4: KillsDeberanLeft = KillsNIULeft = 999; break;
                    }
                }

                if (Channel == 2 || Channel == 3)
                {
                    try
                    {
                        VehiclesChecked = true;
                    }
                    catch { }
                }

                if (Mode != 0)
                {
                    switch (TimeLimit)
                    {
                        case 1:
                            RoundTimeLeft = 599000;
                            break;
                        case 2:
                            RoundTimeLeft = 1199000;
                            break;
                        case 3:
                            RoundTimeLeft = 1799000;
                            break;
                        case 4:
                            RoundTimeLeft = 2399000;
                            break;
                        case 5:
                            RoundTimeLeft = 2399000;
                            break;
                        case 6:
                            RoundTimeLeft = -1000;
                            break;
                    }
                }
                else
                {
                    RoundTimeLeft = 180000; // Round Timer
                }

                if (MapData != null)
                {
                    for (int I = 0; I < MapData.Flags; I++)
                    {
                        Flags[I] = -1;
                    }
                    Flags[MapData.flag1] = 0;
                    Flags[MapData.flag2] = 1;
                    SpawnVehicles();
                }


                foreach (virtualUser RoomUser in Players)
                {
                    ResetRoomStats(RoomUser);
                    RoomUser.Alive = true;
                    RoomUser.InGame = true;
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.AppendError("Couldn't start room " + ID + ", error: " + ex.Message);
                return false;
            }
        }

        public ArrayList ListCheckAliveRoomSlot = new ArrayList();
        public void CheckAliveRoomSlot()
        {
            ListCheckAliveRoomSlot.Clear();
            foreach (virtualUser _Player in Players)
            {
                if (_Player.Health > 0)
                { ListCheckAliveRoomSlot.Add(_Player.RoomSlot); ZombieFollower = _Player.RoomSlot; }
            }
        }

        #region SiegeWar & SiegeWar 2

        public void CheckForMission(virtualUser usr, int VehicleID)
        {
            if (VehicleID == GetIncubatorVehicleId() && Channel == 3 && Mode == 11)
            {
                endGame();
                return;
            }
            if (MapID == 42)
            {
                if (VehicleID == 8 || VehicleID == 9)
                {
                    usr.rPoints += 25;
                    if (Mission1 == null)
                    {
                        Mission1 = usr.Nickname;
                    }
                    Flags[2] = 0;
                    Flags[1] = 1;
                    send(new SP_Unknown(30000, 1, -1, ID, 2, 156, 0, 1, 2, 0, 1, 1, 0, 20, 0, 0, 0, 705882, 637900, 705882, 0, 5600.8521, 287.8355, 5443.2065, 267.1544, -90.9612, -101.7575, 0, 0, "DS05"));
                    send(new SP_Unknown(30000, 1, -1, ID, 2, 156, 0, 1, 1, 1, -1, -1, 0, 20, 0, 0, 0, 705882, 637900, 705882, 0, 5600.8521, 287.8355, 5443.2065, 267.1544, -90.9612, -101.7575, 0, 0, "DS05"));
                }
                else if (VehicleID == 7)
                {
                    usr.rPoints += 30;
                    if (Mission2 == null)
                    {
                        Mission2 = usr.Nickname;
                    }
                    Flags[1] = 0;
                    Flags[3] = 1;
                    send(new SP_Unknown(30000, 1, -1, ID, 2, 156, 0, 1, 1, 0, -1, 1, 0, 20, 0, 0, 0, 705882, 637900, 705882, 0, 5600.8521, 287.8355, 5443.2065, 267.1544, -90.9612, -101.7575, 0, 0, "DS05"));
                    send(new SP_Unknown(30000, 1, -1, ID, 2, 156, 0, 1, 3, 1, -1, -1, 0, 20, 0, 0, 0, 705882, 637900, 705882, 0, 5600.8521, 287.8355, 5443.2065, 267.1544, -90.9612, -101.7575, 0, 0, "DS05"));
                }
                else if (VehicleID == 25 || VehicleID == 24 || VehicleID == 23)
                {
                    usr.rPoints += 50;
                    if (Mission3 == null)
                    {
                        Mission3 = usr.Nickname;
                    }
                }
            }
            else if (MapID == 56)
            {
                if (VehicleID == 0)
                {
                    usr.rPoints += 50;
                    if (Mission2 == null)
                    {
                        Mission2 = usr.Nickname;
                    }
                }
            }
        }

        public void SiegeWar2Explosion()
        {
            try
            {
                send(new SP_Unknown(29985, 0, 0, 1, 4, 0, 100, 0, 0)); // Call animation of explosion
                Vehicle Vehicle = GetVehicleById(0);
                if (Vehicle != null)
                {
                    int Damage = Convert.ToInt32(((Math.Truncate((double)(Vehicle.MaxHealth * 89))) / 100).ToString());
                    Vehicle.Health -= Damage;
                    send(new SP_Unknown(30000, 1, -1, ID, 2, 104, 0, 1, 1, 0, 0, 92, 0, 92, -1, 0, 0, Vehicle.Health, Vehicle.Health, Vehicle.Health + Damage, 0, 2845.7510, 205.0797, 3374.0964, -70.9974, 45.4165, -287.9179, 0, 0, "DP05")); // Damage the 'vehicle'
                    send(new SP_Unknown(29985, 0, -1, 1, 5, -1, 0, -1, 0)); // Spawn the C4
                    if (Vehicle.Health <= 0)
                    {
                        if (Mission2 == null)
                            Mission2 = SiegeWarC4User.Nickname;
                        SiegeWarC4User.rPoints += 50;
                        endGame();
                        SiegeWarC4User = null;
                    }
                    SiegeWarTime = -1;
                }
            }
            catch { }
        }

        public int GetActualMission
        {
            get
            {
                int Count = 0;
                if (MapID == 42 || MapID == 56)
                {
                    if (Mission1 != null) Count++;
                    if (Mission2 != null) Count++;
                    if (Mission3 != null) Count++;
                }
                return Count;
            }
        }

        #endregion

        #region Zombie
        // Time Attack
        public bool firstStage = true;// Time Attack
        public int toStage = 0; // Time Attack
        public int Stage = 0; // Time Attack
        public int ZombieAlive = 5; // Time Attack
        public int SlotZombiefissi = 0;// Time Attack
        public int ZombieID = 0;// Time Attack
        public int ZombieSlot = 0;// Time Attack
        public int zombieKillsPow = 5;// Time Attack
        //
        public int xoldTime = 0;
        public bool prepareReady = false;
        public int calculateDifference = 0;
        public int spawnedZombies = 0;
        public bool freezeZombies = false;
        public bool firstWave = true;
        public int toWave = 20;
        public int zombiePoints = 0;
        public int zTime = 0;
        public int WarningCW = 0;
        public int waveTime = 15;
        public bool waveReqFinished = false;
        public bool SeatsResetet = false;
        public int ReadyToNext = 0;
        public int xKill = 0;
        public bool ZombiesFreeze = false;
        public int Wave = 0;
        public int ZombieWave = 0;
        public bool isFinished = false;
        public bool SeatsFull = false;
        public int zombieKills = 0;
        public int NumBomber = 0;
        public bool Explode = false;
        public bool Newstage = false;
        public bool FirstSpawn = false;

        public int oldTime = 0;
        public int newTime = 0;

        // int Zombie
        public int spawnedMadmans = 0;
        public int spawnedManiacs = 0;
        public int spawnedGrinders = 0;
        public int spawnedGrounders = 0;
        public int spawnedHeavys = 0;
        public int spawnedGrowlers = 0;
        public int spawnedLovers = 0;
        public int spawnedHandgemans = 0;
        public int spawnedChariots = 0;
        public int spawnedCrushers = 0;
        public int spawnedBusters = 0;
        public int spawnedCrashers = 0;
        public int spawnedEnvys = 0;
        public int spawnedClaws = 0;
        public int spawnedBombers = 0;
        public int spawnedDefenders = 0;
        public int spawnedBreakers = 0;
        public int spawnedMadSoldiers = 0;
        public int spawnedMadPrisoners = 0;
        //

        public virtualZombie GetAvaiableSeat()
        {
            foreach (virtualZombie z in Zombies.Values)
            {
                if (z.RespawnTick < Structure.timestamp && z.Health == 0)
                {
                    return z;
                }
            }

            return null;
        }

        #region ZombieInfo

        public void spawnZombie(int ZType)
        {
            lock (this)
            {

                if (spawnedZombies >= toWave && Mode != 11) return;

                virtualZombie z = GetAvaiableSeat();

                if (z != null)
                {
                    if (ZombieSpawnPlace >= 12) ZombieSpawnPlace = 0;
                    if (ZType == 14) ZombieSpawnPlace = 4;
                    if (ZType == 15) ZombieSpawnPlace = 5;
                    if (this.MapID == 55 && ZType == 16) ZombieSpawnPlace = 1;
                    if (ZType == 10 && spawnedBusters == 0) ZombieSpawnPlace = 25;
                    if (ZType == 10 && spawnedBusters == 1) ZombieSpawnPlace = 26;
                    if (ZType >= 0 && ZType <= 22)
                    {
                        int RandomTargetRoomSlot = new Random().Next(0, UsersDic.Count);

                        int foll = (Zombies.Count >= (int)(32 % UsersDic.Count) && UsersDic.Count > 1 ? RandomTargetRoomSlot : RoomMaster);

                        z.FollowUser = foll; /* foll */
                        z.timestamp = Structure.timestamp + 1;
                        z.Type = ZType;
                        z.Death = false;

                        z.Reset();

                        spawnedZombies++;
                        ZombieSpawnPlace++;

                        send(new PACKET_ZOMBIE_SPAWN(z.ID, z.FollowUser, ZombieSpawnPlace, ZType, z.Health));

                        switch (ZType)
                        {
                            case 0: spawnedMadmans++; break;
                            case 1: spawnedManiacs++; break;
                            case 2: spawnedGrinders++; break;
                            case 3: spawnedGrounders++; break;
                            case 4: spawnedGrowlers++; break;
                            case 5: spawnedHeavys++; break;
                            case 6: spawnedLovers++; break;
                            case 7: spawnedHandgemans++; break;
                            case 8: spawnedChariots++; break;
                            case 9: spawnedCrushers++; break;
                            case 10: spawnedBusters++; break;
                            case 11: spawnedCrashers++; break;
                            case 12: spawnedEnvys++; break;
                            case 13: spawnedClaws++; break;
                            case 14: spawnedBombers++; break;
                            case 15: spawnedDefenders++; break;
                            case 16: spawnedBreakers++; break;
                            case 17: spawnedMadSoldiers++; break;
                            case 18: spawnedMadPrisoners++; break;
                                //case 19: spawnedCrushers++; break;
                                //case 20: spawnedCrushers++; break;
                                //case 21: spawnedCrushers++; break;
                                //case 22: spawnedCrushers++; break;
                        }
                    }
                }
            }
        }
        #endregion

        public void zombieStart()
        {
            zTime = 10;
            if (isFinished == true)
            {
                beginSpawn = true;
            }
        }

        //TimeAttack
        public void newStage()  //Time Attack
        {
            spawnedZombies = 0;
            //freezeZombies = true;
            send(new PACKET_ZOMBIE_STAGE(0));
            Stage += 1;
            send(new PACKET_ZOMBIE_STAGE(Stage));

            //Reset
            //resetStats();
            resetCamps();

            //Do the new Wave
            xoldTime = RoundTimeSpent;
            prepareReady = true;
        }

        public void doStage(int StageNumber)
        {
            resetCamps();
            freezeZombies = false;
        }

        public void resetCamps()
        {
            spawnedMadmans = 0;
            spawnedManiacs = 0;
            spawnedGrinders = 0;
            spawnedGrounders = 0;
            spawnedHeavys = 0;
            spawnedGrowlers = 0;
            spawnedLovers = 0;
            spawnedHandgemans = 0;
            spawnedChariots = 0;
            spawnedCrushers = 0;
            spawnedZombies = 0;
            spawnedBusters = 0;
            spawnedCrashers = 0;
            spawnedClaws = 0;
            spawnedBombers = 0;
            spawnedDefenders = 0;
            spawnedBreakers = 0;
            spawnedMadSoldiers = 0;
            spawnedMadPrisoners = 0;

            //toWave = 0;
            xoldTime = 0;
            zombieKills = 0;
            Zombies.Clear();
            for (int i = 0; i < 26; i++)
            {
                Zombies.Add(i + 4, new virtualZombie((i + 4), 0, 0, 0));
            }
            freezeZombies = false;
            prepareReady = false;
            //waveTime = 15;
        }

        // TIME ATTACK
        public void spawnTimeAttack()
        {
            try
            {
                virtualZombie _Zombie = GetAvaiableSeat();

                if (ReadyToNext >= PlayerCount) //FreeWar : With this method we can send new stage when players are in pass zone!!
                {
                    newStage();
                    Newstage = true;
                    ReadyToNext = 0;
                    return;
                }
                if (freezeZombies) return;

                switch (Stage) // Time Attack
                {
                    case 0: // Stage 1
                        if (!beginSpawn)
                        {
                            send(new PACKET_ZOMBIE_STAGE(0));
                            zombieStart();
                        }
                        else
                        {
                            if (firstStage)
                            {
                                if (isFinished && zTime == 0)
                                {
                                    firstStage = false;
                                }
                            }
                            else
                            {
                                spawnZombie(0);
                                spawnZombie(1);

                                if (spawnedZombies >= 30)
                                {
                                    if (spawnedHeavys < 5) spawnZombie(5);
                                    if (spawnedZombies >= 100)
                                    {

                                        if (spawnedChariots < 2) spawnZombie(8);
                                        if (spawnedZombies >= 200 && spawnedCrushers < 1) spawnZombie(9);

                                    }
                                }
                            }
                        }
                        break;
                    case 1: // Stage 2
                        if (spawnedBombers < 1) spawnZombie(14);
                        if (spawnedZombies <= 30)
                        {
                            spawnZombie(0);
                            spawnZombie(1);
                        }
                        if (spawnedZombies >= 30)
                        {
                            if (spawnedHeavys < 5) spawnZombie(5);
                            if (spawnedManiacs < 5) spawnZombie(1);
                            if (spawnedMadmans < 5) spawnZombie(0);

                            if (spawnedChariots < 1) spawnZombie(8);
                            if (ZombieDifficulty == 1 && spawnedCrushers < 1) spawnZombie(9);
                        }
                        break;
                    case 2: // Stage 3
                        if (spawnedDefenders < 1) spawnZombie(15);
                        if (spawnedZombies <= 15)
                        {
                            spawnZombie(0);
                            spawnZombie(1);
                        }
                        if (spawnedZombies >= 15)
                        {
                            if (spawnedHeavys <= 5) spawnZombie(5);
                            if (spawnedManiacs <= 5) spawnZombie(1);
                            if (spawnedMadmans <= 5) spawnZombie(0);


                            if (spawnedChariots < 1) spawnZombie(8);
                           
                        }
                        break;
                    case 3: // Stage 4
                        //if (spawnedBreakers == 0) spawnZombie(16);
                        /*if(this.MapID == 55)
                        {
                            if (spawnedBusters < 2)
                            {
                                for (int id = spawnedBusters; id < 2; id++)
                                {
                                    spawnZombie(10);
                                }
                            }
                        }
                        if (spawnedZombies <= 30)
                                 {
                                 spawnZombie(1);
                                 spawnZombie(1);
                                 }
                                 if (spawnedZombies >= 30)
                                 {
                                     if (spawnedHeavys <= 5) spawnZombie(1);
                                     if (spawnedManiacs <= 5) spawnZombie(1);
                                     if (spawnedMadmans <= 5) spawnZombie(1);

                        if (spawnedChariots < 2) spawnZombie(1);
                        if (ZombieDifficulty == 1 && spawnedCrushers < 1) spawnZombie(1);  


                        }*/
                        break;
                }
            }
            catch { }
        }

        public void updateSleep()
        {
            if (zTime > 0)
            {
                isFinished = false;
                zTime--;
            }
            else
            {
                isFinished = true;
            }
        }

        //Wave
        public void newWave()
        {
            if (zombieKills >= spawnedZombies)
            {

                freezeZombies = true;
                send(new PACKET_ZOMBIE_WAVE(0));
                Wave += 1;
                send(new PACKET_ZOMBIE_WAVE(Wave));

                //Reset
                resetStats();

                //Do the new Wave
                xoldTime = RoundTimeSpent;
                prepareReady = true;

            }
            else
            {
                return;
            }
        }

        public void prepareStage(int time)
        {
            if (Newstage == true)
            {
                doStage(Stage);
                Newstage = false;
            }
        }

        public void prepareWave(int time)
        {
            if (spawnedZombies <= zombieKills)
            {
                calculateDifference = xoldTime - time;
                if (calculateDifference == -15000)
                {
                    doWave(Wave);
                }
            }
        }

        public void doWave(int waveNumber)
        {
            resetStats();
            freezeZombies = false;
        }

        public void resetStats()
        {
            spawnedMadmans = 0;
            spawnedManiacs = 0;
            spawnedGrinders = 0;
            spawnedGrounders = 0;
            spawnedGrowlers = 0;
            spawnedHeavys = 0;
            spawnedLovers = 0;
            spawnedHandgemans = 0;
            spawnedChariots = 0;
            spawnedCrushers = 0;
            spawnedZombies = 0;
            spawnedBusters = 0;
            spawnedCrashers = 0;
            spawnedClaws = 0;
            spawnedBombers = 0;
            spawnedDefenders = 0;
            spawnedBreakers = 0;
            spawnedMadSoldiers = 0;
            spawnedMadPrisoners = 0;

            toWave = 0;
            xoldTime = 0;
            zombieKills = 0;
            //ZombieSeats = 4;
            prepareReady = false;
            waveTime = 15;
        }

        public void waveSleep()
        {
            if (waveTime > 0)
            {
                waveReqFinished = false;
                waveTime--;
            }
            else
            {
                waveReqFinished = true;
            }
        }

        public void resetSpawners()
        {
            switch (Stage)
            {
                case 0: { xKill = 21; } break;
                case 1: { xKill = 21; } break;
                case 2: { xKill = 21; } break;
                case 4: { xKill = 21; } break;
            }

            /*if (ZombieSeats == 26)
            {
                ZombieSeats = 4;
            }*/

        }

        public void resetSeats()
        {
            /*Zombies.Clear();
            for (int i = 0; i < 26; i++)
            {
                Zombies.Add(i + 4, new virtualZombie((i + 4), 0, 0, 0));
            }*/
        }
        public virtualZombie GetZombieById(int id)
        {
            if (Zombies.ContainsKey(id))
            {
                return (virtualZombie)Zombies[id];
            }
            return null;
        }

        public void SpawnZombieList()
        {
            try
            {
                switch (Wave)
                {
                    case 0: { toWave = 20; } break; // Wave 1
                    case 1: { toWave = 27; } break; // Wave 2
                    case 2: { toWave = 35; } break; // Wave 3
                    case 3: { toWave = 43; } break; // Wave 4
                    case 4: { toWave = 52; } break; // Wave 5
                    case 5: { toWave = 65; } break; // Wave 6
                    case 6: { toWave = 72; } break; // Wave 7
                    case 7: { toWave = 83; } break; // Wave 8
                    case 8: { toWave = 92; } break; // Wave 9
                    case 9: { toWave = 100; } break; // Wave 10
                    case 10: { toWave = 101; } break; // Wave 11
                    case 11: { toWave = 107; } break; // Wave 12
                    case 12: { toWave = 115; } break; // Wave 13
                    case 13: { toWave = 117; } break; // Wave 14
                    case 14: { toWave = 125; } break; // Wave 15
                    case 15: { toWave = 128; } break; // Wave 16
                    case 16: { toWave = 139; } break; // Wave 17
                    case 17: { toWave = 144; } break; // Wave 18
                    case 18: { toWave = 153; } break; // Wave 19
                    case 19: { toWave = 169; } break; // Wave 20
                    case 20: { toWave = 192; } break; // Wave 21
                }

                if (spawnedZombies >= toWave)
                {
                    freezeZombies = true;
                    newWave();
                    return;
                }
                if (freezeZombies)
                    return;
                if (Wave >= 22) endGame();
                switch (Wave)
                {
                    case 0: // Wave 1
                        if (!beginSpawn)
                        {
                            send(new PACKET_ZOMBIE_WAVE(0));
                            zombieStart();
                        }
                        else
                        {
                            if (firstWave)
                            {
                                if (isFinished && zTime == 0)
                                {
                                    firstWave = false;
                                }
                            }
                            else
                            {
                                if (spawnedMadmans < 20)
                                    spawnZombie(0);
                                //20
                            }
                        }
                        break;
                    case 1: // Wave 2

                        if (spawnedMadmans < 20) spawnZombie(0);
                        if (spawnedManiacs < 7) spawnZombie(1);
                        //27
                        break;
                    case 2: // Wave 3

                        if (spawnedMadmans < 20) spawnZombie(0);
                        if (spawnedManiacs < 7) spawnZombie(1);
                        //
                        if (spawnedGrinders < 6 && zombieKills >= 20) spawnZombie(2);
                        if (spawnedLovers < 1 && zombieKills >= 30) spawnZombie(6);
                        if (spawnedHandgemans < 1 && zombieKills >= 30) spawnZombie(7);
                        //Tot:35
                        break;
                    case 3: // Wave 4
                        if (spawnedMadmans < 10) spawnZombie(0);
                        if (spawnedManiacs < 20) spawnZombie(1);
                        //
                        if (spawnedGrinders < 8 && zombieKills >= 20) spawnZombie(2);
                        if (spawnedLovers < 2 && zombieKills >= 30) spawnZombie(6);
                        if (spawnedHandgemans < 1 && zombieKills >= 40) spawnZombie(7);
                        if (spawnedHeavys < 2 && zombieKills >= 35) spawnZombie(5);

                        //Tot:43
                        break;
                    case 4: // Wave 5
                        if (spawnedMadmans < 21) spawnZombie(0);
                        if (spawnedManiacs < 10) spawnZombie(1);
                        //
                        if (spawnedGrinders < 10 && zombieKills >= 20) spawnZombie(2);
                        if (spawnedGrowlers < 4 && zombieKills >= 30) spawnZombie(4);
                        if (spawnedLovers < 3 && zombieKills >= 30) spawnZombie(6);
                        if (spawnedHandgemans < 2 && zombieKills >= 50) spawnZombie(7);
                        if (spawnedHeavys < 2 && zombieKills >= 30) spawnZombie(5);

                        //Tot:52
                        break;
                    case 5: // Wave 6

                        if (spawnedManiacs < 12) spawnZombie(1);
                        if (spawnedMadmans < 22) spawnZombie(0);
                        if (spawnedGrounders < 3) spawnZombie(3);
                        //
                        if (spawnedGrinders < 15 && zombieKills >= 20) spawnZombie(2);
                        if (spawnedGrowlers < 5 && zombieKills >= 30) spawnZombie(4);
                        if (spawnedLovers < 3 && zombieKills >= 40) spawnZombie(6);
                        if (spawnedHandgemans < 2 && zombieKills >= 60) spawnZombie(7);
                        if (spawnedHeavys < 3 && zombieKills >= 40) spawnZombie(5);

                        //Tot:65
                        break;
                    case 6: // Wave 7



                        if (spawnedManiacs < 12) spawnZombie(1);
                        if (spawnedMadmans < 22) spawnZombie(0);
                        if (spawnedGrinders < 18) spawnZombie(2);
                        if (spawnedGrounders < 4) spawnZombie(3);
                        //
                        if (spawnedGrowlers < 6 && zombieKills >= 30) spawnZombie(4);
                        if (spawnedLovers < 3 && zombieKills >= 50) spawnZombie(6);
                        if (spawnedHandgemans < 2 && zombieKills >= 60) spawnZombie(7);
                        if (spawnedHeavys < 4 && zombieKills >= 40) spawnZombie(5);
                        if (spawnedChariots < 1 && zombieKills >= 60) spawnZombie(8);

                        //Tot:72
                        break;
                    case 7: // Wave 8

                        if (spawnedManiacs < 15) spawnZombie(1);
                        if (spawnedMadmans < 25) spawnZombie(0);
                        if (spawnedGrinders < 20) spawnZombie(2);
                        if (spawnedGrounders < 5) spawnZombie(3);
                        //
                        if (spawnedGrowlers < 8 && zombieKills >= 25) spawnZombie(4);
                        if (spawnedLovers < 3 && zombieKills >= 40) spawnZombie(6);
                        if (spawnedHandgemans < 3 && zombieKills >= 60) spawnZombie(7);
                        if (spawnedHeavys < 4 && zombieKills >= 70) spawnZombie(5);

                        //Tot:83
                        break;
                    case 8: // Wave 9
                        if (spawnedManiacs < 15) spawnZombie(1);
                        if (spawnedMadmans < 25) spawnZombie(0);
                        if (spawnedGrinders < 23) spawnZombie(2);
                        if (spawnedGrounders < 6) spawnZombie(3);
                        //
                        if (spawnedLovers < 4 && zombieKills >= 30) spawnZombie(6);
                        if (spawnedHandgemans < 4 && zombieKills >= 60) spawnZombie(7);
                        if (spawnedHeavys < 7 && zombieKills >= 50) spawnZombie(5);
                        if (spawnedGrowlers < 8 && zombieKills >= 70) spawnZombie(4);

                        //Tot:92
                        break;
                    case 9: // Wave 10
                        if (spawnedManiacs < 20) spawnZombie(1);
                        if (spawnedMadmans < 25) spawnZombie(0);
                        if (spawnedGrinders < 12) spawnZombie(2);
                        if (spawnedGrowlers < 4) spawnZombie(4);
                        //
                        if (spawnedGrounders < 6 && zombieKills >= 50) spawnZombie(3);
                        if (spawnedLovers < 4 && zombieKills >= 60) spawnZombie(6);
                        if (spawnedHandgemans < 4 && zombieKills >= 70) spawnZombie(7);
                        if (spawnedHeavys < 7 && zombieKills >= 50) spawnZombie(5);
                        if (spawnedChariots < 2) spawnZombie(8);
                        if (spawnedGrinders < 24 && zombieKills >= 80) spawnZombie(2);
                        if (spawnedGrowlers < 8 && zombieKills >= 90) spawnZombie(4);
                        //Tot:100
                        break;
                    case 10: // Wave 11
                        if (spawnedManiacs < 20) spawnZombie(1);
                        if (spawnedMadmans < 25) spawnZombie(0);
                        if (spawnedGrinders < 12) spawnZombie(2);
                        if (spawnedGrowlers < 4) spawnZombie(4);
                        if (spawnedGrounders < 4) spawnZombie(3);
                        //
                        if (spawnedGrounders < 7 && zombieKills >= 50) spawnZombie(3);
                        if (spawnedLovers < 5 && zombieKills >= 60) spawnZombie(6);
                        if (spawnedHandgemans < 4 && zombieKills >= 70) spawnZombie(7);
                        if (spawnedHeavys < 8 && zombieKills >= 50) spawnZombie(5);
                        if (spawnedGrinders < 24 && zombieKills >= 70) spawnZombie(2);
                        if (spawnedGrowlers < 8 && zombieKills >= 80) spawnZombie(4);
                        //Tot:101
                        break;
                    case 11: // Wave 12
                        if (spawnedManiacs < 20) spawnZombie(1);
                        if (spawnedMadmans < 25) spawnZombie(0);
                        if (spawnedGrinders < 13) spawnZombie(2);
                        if (spawnedGrowlers < 5) spawnZombie(4);
                        if (spawnedGrounders < 3) spawnZombie(3);
                        //
                        if (spawnedGrounders < 7 && zombieKills >= 60) spawnZombie(3);
                        if (spawnedLovers < 5 && zombieKills >= 75) spawnZombie(6);
                        if (spawnedHandgemans < 5 && zombieKills >= 80) spawnZombie(7);
                        if (spawnedHeavys < 8 && zombieKills >= 60) spawnZombie(5);
                        if (spawnedChariots < 1 && zombieKills >= 70) spawnZombie(8);
                        if (spawnedCrushers < 1 && zombieKills >= 85) spawnZombie(9);
                        if (spawnedGrinders < 25 && zombieKills >= 60) spawnZombie(2);
                        if (spawnedGrowlers < 10 && zombieKills >= 70) spawnZombie(4);

                        //Tot:107
                        break;
                    case 12: // Wave 13

                        if (spawnedManiacs < 25) spawnZombie(1);
                        if (spawnedMadmans < 25) spawnZombie(0);
                        if (spawnedGrinders < 13) spawnZombie(2);
                        if (spawnedGrowlers < 6) spawnZombie(4);
                        if (spawnedGrounders < 4) spawnZombie(3);
                        //
                        if (spawnedGrounders < 8 && zombieKills >= 80) spawnZombie(3);
                        if (spawnedLovers < 5 && zombieKills >= 60) spawnZombie(6);
                        if (spawnedHandgemans < 5 && zombieKills >= 40) spawnZombie(7);
                        if (spawnedHeavys < 8 && zombieKills >= 60) spawnZombie(5);
                        if (spawnedChariots < 1 && zombieKills >= 70) spawnZombie(8);
                        if (spawnedChariots < 2 && zombieKills >= 80) spawnZombie(8);
                        if (spawnedGrinders < 26 && zombieKills >= 80) spawnZombie(2);
                        if (spawnedGrowlers < 11 && zombieKills >= 70) spawnZombie(4);
                        //Tot:115
                        break;
                    case 13: // Wave 14

                        if (spawnedManiacs < 25) spawnZombie(1);
                        if (spawnedMadmans < 25) spawnZombie(0);
                        if (spawnedGrinders < 13) spawnZombie(2);
                        if (spawnedGrowlers < 7) spawnZombie(4);
                        if (spawnedGrounders < 4) spawnZombie(3);
                        //
                        if (spawnedGrounders < 9 && zombieKills >= 50) spawnZombie(3);
                        if (spawnedLovers < 5 && zombieKills >= 60) spawnZombie(6);
                        if (spawnedHandgemans < 5 && zombieKills >= 80) spawnZombie(7);
                        if (spawnedHeavys < 8 && zombieKills >= 70) spawnZombie(5);
                        if (spawnedCrushers < 1 && zombieKills >= 80) spawnZombie(9);
                        if (spawnedGrinders < 26 && zombieKills >= 90) spawnZombie(2);
                        if (spawnedGrowlers < 13 && zombieKills >= 70) spawnZombie(4);
                        //Tot:117
                        break;
                    case 14: // Wave 15

                        if (spawnedManiacs < 25) spawnZombie(1);
                        if (spawnedMadmans < 30) spawnZombie(0);
                        if (spawnedLovers < 3) spawnZombie(6);
                        if (spawnedHandgemans < 3) spawnZombie(7);
                        if (spawnedGrinders < 14) spawnZombie(2);
                        if (spawnedGrowlers < 6) spawnZombie(4);
                        if (spawnedGrounders < 5) spawnZombie(3);
                        //
                        if (spawnedGrounders < 10 && zombieKills >= 80) spawnZombie(3);
                        if (spawnedLovers < 6 && zombieKills >= 50) spawnZombie(6);
                        if (spawnedHandgemans < 6 && zombieKills >= 60) spawnZombie(7);
                        if (spawnedHeavys < 9 && zombieKills >= 80) spawnZombie(5);
                        if (spawnedGrinders < 27 && zombieKills >= 75) spawnZombie(2);
                        if (spawnedGrowlers < 12 && zombieKills >= 80) spawnZombie(4);
                        //Tot:125
                        break;
                    case 15: // Wave 16

                        if (spawnedManiacs < 30) spawnZombie(1);
                        if (spawnedMadmans < 25) spawnZombie(0);
                        if (spawnedLovers < 4) spawnZombie(6);
                        if (spawnedHandgemans < 3) spawnZombie(7);
                        if (spawnedHeavys < 5) spawnZombie(5);
                        if (spawnedGrinders < 14) spawnZombie(2);
                        if (spawnedGrowlers < 6) spawnZombie(4);
                        if (spawnedGrounders < 5) spawnZombie(3);
                        //
                        if (spawnedGrounders < 10 && zombieKills >= 70) spawnZombie(3);
                        if (spawnedLovers < 7 && zombieKills >= 80) spawnZombie(6);
                        if (spawnedHandgemans < 6 && zombieKills >= 80) spawnZombie(7);
                        if (spawnedHeavys < 10 && zombieKills >= 90) spawnZombie(5);
                        if (spawnedChariots < 1 && zombieKills >= 75) spawnZombie(8);
                        if (spawnedGrinders < 27 && zombieKills >= 90) spawnZombie(2);
                        if (spawnedGrowlers < 12 && zombieKills >= 100) spawnZombie(4);
                        //Tot:128
                        foreach (virtualUser Player in Players)
                        {
                            if (WeaponsGot == false)
                            {
                                Player.send(new PACKET_CHAT("EVENT", PACKET_CHAT.ChatType.Room_ToAll, "EVENT >> You got CLAW_KNIFE (30 DAYS) because you survived wave 16!!", 999, "NULL"));
                                Player.AddOutBoxItem("DA13", 30, 1);
                                WeaponsGot = true;
                            }
                        }

                        break;
                    case 16: // Wave 17

                        if (spawnedManiacs < 30) spawnZombie(1);
                        if (spawnedMadmans < 30) spawnZombie(0);
                        if (spawnedGrinders < 14) spawnZombie(2);
                        if (spawnedGrowlers < 7) spawnZombie(4);
                        if (spawnedLovers < 5) spawnZombie(6);
                        if (spawnedHandgemans < 3) spawnZombie(7);
                        if (spawnedHeavys < 6) spawnZombie(5);
                        if (spawnedChariots < 1) spawnZombie(8);
                        if (spawnedGrounders < 6) spawnZombie(3);
                        //
                        if (spawnedGrounders < 11 && zombieKills >= 50) spawnZombie(3);
                        if (spawnedLovers < 8 && zombieKills >= 60) spawnZombie(6);
                        if (spawnedHandgemans < 6 && zombieKills >= 70) spawnZombie(7);
                        if (spawnedHeavys < 10 && zombieKills >= 80) spawnZombie(5);
                        if (spawnedChariots < 2 && zombieKills >= 90) spawnZombie(8);
                        if (spawnedCrushers < 1 && zombieKills >= 100) spawnZombie(9);
                        if (spawnedGrinders < 28 && zombieKills >= 90) spawnZombie(2);
                        if (spawnedGrowlers < 13 && zombieKills >= 110) spawnZombie(4);
                        //Tot:139
                        break;
                    case 17: // Wave 18

                        if (spawnedManiacs < 30) spawnZombie(1);
                        if (spawnedMadmans < 30) spawnZombie(0);
                        if (spawnedLovers < 4) spawnZombie(6);
                        if (spawnedHandgemans < 4) spawnZombie(7);
                        if (spawnedHeavys < 6) spawnZombie(5);
                        if (spawnedGrinders < 15) spawnZombie(2);
                        if (spawnedGrowlers < 8) spawnZombie(4);
                        if (spawnedGrounders < 7) spawnZombie(3);
                        //
                        if (spawnedGrounders < 13 && zombieKills >= 50) spawnZombie(3);
                        if (spawnedLovers < 8 && zombieKills >= 80) spawnZombie(6);
                        if (spawnedHandgemans < 7 && zombieKills >= 90) spawnZombie(7);
                        if (spawnedHeavys < 11 && zombieKills >= 95) spawnZombie(5);
                        if (spawnedGrinders < 30 && zombieKills >= 100) spawnZombie(2);
                        if (spawnedGrowlers < 15 && zombieKills >= 100) spawnZombie(4);
                        //Tot:144
                        break;
                    case 18: // Wave 19

                        if (spawnedManiacs < 35) spawnZombie(1);
                        if (spawnedMadmans < 30) spawnZombie(0);
                        if (spawnedGrinders < 15) spawnZombie(2);
                        if (spawnedGrowlers < 8) spawnZombie(4);
                        if (spawnedLovers < 5) spawnZombie(6);
                        if (spawnedHandgemans < 4) spawnZombie(7);
                        if (spawnedHeavys < 6) spawnZombie(5);
                        if (spawnedChariots < 1) spawnZombie(8);
                        if (spawnedCrushers < 1) spawnZombie(9);
                        if (spawnedGrounders < 7) spawnZombie(3);
                        //
                        if (spawnedGrounders < 13 && zombieKills >= 70) spawnZombie(3);
                        if (spawnedLovers < 9 && zombieKills >= 80) spawnZombie(6);
                        if (spawnedHandgemans < 7 && zombieKills >= 90) spawnZombie(7);
                        if (spawnedHeavys < 12 && zombieKills >= 100) spawnZombie(5);
                        if (spawnedGrinders < 30 && zombieKills >= 90) spawnZombie(2);
                        if (spawnedGrowlers < 15 && zombieKills >= 80) spawnZombie(4);
                        //Tot:153
                        break;
                    case 19: // Wave 20

                        if (spawnedManiacs < 35) spawnZombie(1);
                        if (spawnedMadmans < 35) spawnZombie(0);
                        if (spawnedGrinders < 16) spawnZombie(2);
                        if (spawnedGrowlers < 8) spawnZombie(4);
                        if (spawnedLovers < 6) spawnZombie(6);
                        if (spawnedHandgemans < 4) spawnZombie(7);
                        if (spawnedHeavys < 6) spawnZombie(5);
                        if (spawnedChariots < 2) spawnZombie(8);
                        if (spawnedGrounders < 7) spawnZombie(3);
                        //
                        if (spawnedGrounders < 14 && zombieKills >= 80) spawnZombie(3);
                        if (spawnedLovers < 12 && zombieKills >= 90) spawnZombie(6);
                        if (spawnedHandgemans < 9 && zombieKills >= 100) spawnZombie(7);
                        if (spawnedHeavys < 12 && zombieKills >= 90) spawnZombie(5);
                        if (spawnedChariots < 3 && zombieKills >= 100) spawnZombie(8);
                        if (spawnedCrushers < 1 && zombieKills >= 110) spawnZombie(9);
                        if (spawnedGrinders < 32 && zombieKills >= 120) spawnZombie(2);
                        if (spawnedGrowlers < 16 && zombieKills >= 130) spawnZombie(4);
                        //Tot:169
                        break;

                    case 20: // Wave 21

                        if (spawnedManiacs < 40) spawnZombie(1);
                        if (spawnedMadmans < 40) spawnZombie(0);
                        if (spawnedGrinders < 17) spawnZombie(2);
                        if (spawnedGrowlers < 9) spawnZombie(4);
                        if (spawnedLovers < 7) spawnZombie(6);
                        if (spawnedHandgemans < 5) spawnZombie(7);
                        if (spawnedHeavys < 8) spawnZombie(5);
                        if (spawnedChariots < 2) spawnZombie(8);
                        if (spawnedCrushers < 1) spawnZombie(9);
                        if (spawnedGrounders < 8) spawnZombie(3);
                        //
                        if (spawnedGrounders < 15 && zombieKills >= 90) spawnZombie(3);
                        if (spawnedLovers < 14 && zombieKills >= 100) spawnZombie(6);
                        if (spawnedHandgemans < 10 && zombieKills >= 120) spawnZombie(7);
                        if (spawnedHeavys < 15 && zombieKills >= 90) spawnZombie(5);
                        if (spawnedChariots < 4 && zombieKills >= 120) spawnZombie(8);
                        if (spawnedCrushers < 2 && zombieKills >= 140) spawnZombie(9);
                        if (spawnedGrinders < 34 && zombieKills >= 120) spawnZombie(2);
                        if (spawnedGrowlers < 18 && zombieKills >= 75) spawnZombie(4);
                        //Tot:192
                        foreach (virtualUser Player in Players)
                        {
                            if (WeaponsGot == false)
                            {
                                Player.send(new PACKET_CHAT("EVENT", PACKET_CHAT.ChatType.Room_ToAll, "EVENT >> You got M249 silver (3 DAYS) because you survived!!", 999, "NULL"));
                                Player.AddOutBoxItem("D901", 3, 1);
                                WeaponsGot = true;
                            }
                        }
                        break;
                }
            }
            catch { }
        }

        #endregion

        public int AliveUsers(int Side)
        {
            int Count = 0;
            foreach (virtualUser RoomUser in Players)
                if (RoomUser.Health > 0 && getSide(RoomUser) == Side && RoomUser.Alive) Count++;
            return Count;
        }

        public int AliveEscape(bool IsZombie)
        {
            int Count = 0;
            foreach (virtualUser RoomUser in Players)
                if (RoomUser.Health > 0 && RoomUser.IsEscapeZombie == IsZombie && RoomUser.Alive) Count++;
            return Count;
        }

        public bool canSwitch(virtualUser User)
        {
            if (User != null && User.Room != null && User.Room.ID == this.ID)
            {
                int Side = getSide(User);
                if (Side == 0)
                {
                    for (int I = MaxPlayers / 2; I < MaxPlayers; I++)
                        if (Users.ContainsKey(I) == false)
                            return true;
                }
                else
                {
                    for (int I = 0; I < MaxPlayers / 2; I++)
                        if (Users.ContainsKey(I) == false)
                            return true;
                }
            }
            return false;
        }


        public int switchSide(virtualUser User)
        {
            if (RoomStatus != 1) return -1;
            User.Health = 0;
            User.Alive = false;
            if (User != null && User.Room != null && User.Room.ID == this.ID)
            {
                int oldSlot = User.RoomSlot;
                int Side = getSide(User);

                if (Side == 0)
                {
                    for (int I = MaxPlayers / 2; I < MaxPlayers; I++)
                    {
                        if (Users.ContainsKey(I) == false)
                        {
                            if (User.RoomSlot == RoomMasterSlot)
                                RoomMaster = I;

                            Users.Remove(oldSlot);
                            Users.Add(I, User);
                            UsersDic.TryAdd(I, User);
                            User.RoomSlot = I;
                            return I;
                        }
                    }
                }
                else
                {
                    for (int I = 0; I < MaxPlayers / 2; I++)
                    {
                        if (Users.ContainsKey(I) == false)
                        {
                            if (User.RoomSlot == RoomMasterSlot)
                                RoomMaster = I;

                            Users.Remove(oldSlot);
                            Users.Add(I, User);
                            UsersDic.TryAdd(I, User);
                            User.RoomSlot = I;
                            return I;
                        }
                    }
                }
            }
            return -1;
        }
        public void CheckTimeEnd()
        {
            if (bombPlanted == true)
                prepareRound(0);
            else if (bombPlanted == false)
                prepareRound(1);
        }

        public void UpdateTime()
        {
            if (Sleep) { return; }
            //LastTick from room update, dont forget it!
            if (Channel == 3)
            {
                if (spawnedPlayers >= PlayerCount)
                {
                    if (Mode == 11)
                    {
                        if (IsTimeOpenDoor)
                        {
                            TimeToOpenDoor += 1000;
                            if (this.PowPlayer >= this.IntoPassing && this.Stage == 0) // 2° Stage
                            {
                                this.send(new PACKET_TIMEATTACK_ALL(30053, 5));
                                this.send(new PACKET_TIMEATTACK_ALL(30053, 4));
                                if (TimeToOpenDoor >= 6000)
                                {
                                    this.send(new PACKET_TIMEATTACK_ALL(30053, 2));
                                    this.freezeZombies = false;
                                    this.ReadyToNext++;
                                    this.Stage2.Start();
                                    IsTimeOpenDoor = false;
                                    this.PowPlayer = 0;
                                    TimeToOpenDoor = 0;
                                }
                            }

                            if (this.PowPlayer >= this.IntoPassing && this.Stage == 1) // 3° Stage
                            {
                                this.send(new PACKET_TIMEATTACK_NEWSTAGE(this, 5));
                                this.send(new PACKET_TIMEATTACK_NEWSTAGE(this, 4));
                                if (TimeToOpenDoor >= 6000)
                                {
                                    this.send(new PACKET_TIMEATTACK_ALL(30053, 2));
                                    this.freezeZombies = false;
                                    this.ReadyToNext++;
                                    this.Stage3.Start();
                                    IsTimeOpenDoor = false;
                                    this.PowPlayer = 0;
                                    TimeToOpenDoor = 0;
                                }
                            }

                            if (this.PowPlayer >= this.IntoPassing && this.Stage == 2 && this.ZombieDifficulty == 1) // 4° Stage
                            {
                                this.send(new PACKET_TIMEATTACK_NEWSTAGE(this, 5));
                                this.send(new PACKET_TIMEATTACK_NEWSTAGE(this, 4));
                                if (TimeToOpenDoor >= 6000)
                                {
                                    this.send(new PACKET_TIMEATTACK_ALL(30053, 2));
                                    this.freezeZombies = false;
                                    this.ReadyToNext++;
                                    this.Stage4.Start();
                                    IsTimeOpenDoor = false;
                                    this.PowPlayer = 0;
                                    TimeToOpenDoor = 0;
                                }
                            }
                        }
                        else TimeToOpenDoor = 0;
                        InitialTime -= 1000;
                    }
                    else
                    {
                        RoundTimeSpent += 1000;
                    }
                }
            }
            else
            {
                if (SiegeWarTime >= 0)
                {
                    SiegeWarTime--;
                    if (SiegeWarTime <= 0)
                    {
                        SiegeWar2Explosion();
                        SiegeWarTime = -1;
                    }
                }

                RoundTimeSpent += 1000;
            }

            send(new PACKET_ROOM_TICK(this));
        }

        bool CWCheck = true;

        public void AutoCountVoid()
        {
            Thread.Sleep(1000);
            if (AutoCount != DateTime.Now.Second)
            {
                asd += 1000;
                if (asd % 1000 == 0) numero--;
                foreach (virtualUser RoomUser in Players)
                {
                    if (numero == 0) RoomUser.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> Game Will Start NOW!", 998, "NULL"));
                    else RoomUser.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> Game Will Start After " + numero + " Second", 998, "NULL"));
                }
                if (numero == 0)
                {
                    //C=>991597 30000 0 1 1 1 1 0 0 0 0 0 0 0 0 0
                    //S=>991474 30000 1 0 1 1 4 1 0 5 0 0 0 0 0 0 0 
                    //S=>991475 30000 1 0 1 1 4 1 0 5 0 0 0 0 0 0 0 

                    //C=>957670 30000 0 0 1 1 1 0 0 0 0 0 0 0 0 0 
                    //S=>957566 30000 1 0 0 1 4 1 0 71 0 0 0 0 0 0 0 
                    Start();
                    foreach (virtualUser RoomUser in Players)
                        RoomUser.send(new PACKET_AUTOSTART(this));
                }
            }
            if (numero > 0) Repeat();
        }
        public void Repeat()
        {
            if (AllPlayerReady)
            {
                AutoCount = DateTime.Now.Second;
                AutoCountVoid();
            }
            else
            {
                foreach (virtualUser RoomUser in Players)
                {
                    if (numero < 5) RoomUser.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> The CountDown Was Stopped!!", 998, "NULL"));
                }
                numero = 5;
                asd = 0;
            }
        }
        public void CheckReadys()
        {
            if (PlayerCount == UserReadys)
                Repeat();
        }

        public void update()
        {
            try
            {
               if (Users.Count > 0)
                {
                    if (AutoStart && AllPlayerReady) CheckReadys();
                    if (GameActive && LastTick != DateTime.Now.Second && EndGameFreeze == false)
                    {
                        LastTick = DateTime.Now.Second;

                        UpdateTime();

                        if (SpawnLocation > 15) SpawnLocation = 0;

                        foreach (virtualUser RoomUser in Players)
                        {
                            if (RoomUser.SpawnProtection > 0)
                                RoomUser.SpawnProtection--;
                        }

                        foreach (Vehicle Vehicles in this.Vehicles.Values)
                        {
                            if (Vehicles.SpawnProtection > 0)
                                --Vehicles.SpawnProtection;
                            if (Vehicles.Health <= 0 && Vehicles.RespawnTime != -1)
                            {
                                ++Vehicles.RespawnTick;
                                if (Vehicles.RespawnTick >= Vehicles.RespawnTime)
                                {
                                    Vehicles.RespawnTick = 0;
                                    this.RespawnVehicle(Vehicles.ID);
                                }
                            }
                        }


                        if (RoomStatus == 2 && (Mode != 1 && Channel == 1))
                        {
                            if (getSideCount(0) == 0 || getSideCount(1) == 0)
                                endGame();
                        }

                        if (RoomType == 1)
                        {
                            if (getSideCount(0) != getSideCount(1))
                            {
                                if (WarningCW < 30)
                                {
                                    int SecLeft = 30 - WarningCW;
                                    send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> The game will end in " + SecLeft + " seconds if team not get balanced!!", 999, "NULL"));
                                    WarningCW++;
                                }
                                else
                                {
                                    cDerbExplosivePoints = cNIUExplosivePoints = cDerbRounds = cNiuRounds = KillsDeberanLeft = KillsNIULeft = 0;
                                    endGame();
                                }
                                CWCheck = false;
                            }
                            else
                                CWCheck = true;
                        }

                        if (Channel != 3)
                        {
                            VehicleRespawnCount++;
                            if (VehicleRespawnCount == 120)
                            {
                                foreach (Vehicle virtualVehicle in this.Vehicles.Values)
                                {
                                    if (virtualVehicle.SpawnProtection > 0)
                                        --virtualVehicle.SpawnProtection;
                                    if (virtualVehicle.Health <= 0 && virtualVehicle.RespawnTime != -1)
                                    {
                                        ++virtualVehicle.RespawnTick;
                                        if (virtualVehicle.RespawnTick >= virtualVehicle.RespawnTime)
                                        {
                                            virtualVehicle.RespawnTick = 0;
                                            this.RespawnVehicle(virtualVehicle.ID);
                                        }
                                    }
                                }
                                VehicleRespawnCount = 0;
                            }
                        }

                        if (RoundTimeLeft > 0) RoundTimeLeft -= 1000;

                        switch (Mode)
                        {
                            case 0: // Explosive
                                {
                                    if (Users.Count > 1 && GameActive)
                                    {
                                        waitExplosiveTime++;
                                        if (waitExplosiveTime >= 5 && isNewRound)
                                        {
                                            if (AliveUsers(0) == 0 && AliveUsers(1) > 0 && bombPlanted == false)
                                            {
                                                sendNewRound(1);
                                            }
                                            else if (AliveUsers(1) == 0 && AliveUsers(0) > 0)
                                            {
                                                sendNewRound(0);
                                            }
                                            else if (bombPlanted == true)
                                            {
                                                sendNewRound(0);
                                            }
                                            else
                                            {
                                                sendNewRound(1);
                                            }

                                        }
                                        else if (!Sleep)
                                        {
                                            if (Users.Count <= 1) { break; }
                                            if (cNiuRounds >= explosiveRounds || cDerbRounds >= explosiveRounds) { endGame(); }
                                            if (RoundTimeLeft <= 0) { CheckTimeEnd(); }
                                            if (AliveUsers(0) == 0 && AliveUsers(1) > 0 && bombPlanted == false)
                                            {
                                                prepareRound(1);
                                                break;
                                            }
                                            else if (AliveUsers(1) == 0 && AliveUsers(0) > 0)
                                            {
                                                prepareRound(0);
                                                break;
                                            }
                                            else if (AliveUsers(1) == 0 && AliveUsers(0) == 0)
                                            {
                                                if (bombPlanted == true)
                                                {
                                                    prepareRound(0);
                                                    break;
                                                }
                                                else
                                                {
                                                    prepareRound(1);
                                                    break;
                                                }
                                            }

                                        }
                                    }
                                    break;
                                }


                            case 1: // FFA
                                {
                                    if (SpawnLocation < 0 || SpawnLocation >= 19) SpawnLocation = 0;

                                    foreach (virtualUser RoomUser in Players)
                                    {
                                        if (RoomUser.rKills >= highestKills)
                                            highestKills = RoomUser.rKills;
                                    }

                                    if (RoundTimeLeft <= 0 || highestKills >= (10 + (5 * Rounds)))
                                    {
                                        endGame(); return;
                                    }
                                    break;
                                }
                            case 2: // 4 vs 4
                                {
                                    if (RoundTimeLeft == 0 || KillsNIULeft == 0 || KillsDeberanLeft == 0) endGame();
                                    break;
                                }
                            case 3: // Team Death Match
                                {
                                    if (RoundTimeLeft == 0 || KillsNIULeft == 0 || KillsDeberanLeft == 0) endGame();
                                    break;
                                }
                            case 6: // TotalWar
                                {
                                    if (RoundTimeLeft <= 0 || (TotalWarDerb >= Kills || TotalWarNIU >= Kills) || KillsNIULeft <= 0 || KillsDeberanLeft <= 0) { endGame(); }
                                    break;
                                }
                            case 9: // Survival Mode
                                {
                                    if (prepareReady) prepareWave(RoundTimeSpent);
                                    if (toWave >= 25) resetSeats();
                                    updateSleep();
                                    if (Wave >= 22) endGame();
                                    if (readyZombie)
                                    {
                                        SpawnZombieList();
                                    }
                                    if (AliveUsers(0) == 0)
                                        endGame();
                                    break;
                                }
                            case 10: // Defence Mode
                                {
                                    if (prepareReady) prepareWave(RoundTimeSpent);
                                    if (toWave >= 25) resetSeats();
                                    updateSleep();
                                    if (Wave >= 22) endGame();
                                    if (readyZombie)
                                    {
                                        SpawnZombieList();
                                    }
                                    if (AliveUsers(0) == 0)
                                        endGame();
                                    break;
                                }
                            case 11: // Time Attack
                                {

                                    if (prepareReady) prepareStage(RoundTimeSpent);

                                    if (toStage <= 4) resetSpawners();
                                    updateSleep();
                                    if (ZombieDifficulty == 0 && Destructed == true) endGame();
                                    if (ZombieDifficulty == 1 && BossKilled == true)
                                    {
                                        foreach (virtualUser Player in Players)
                                            this.send(new PACKET_SCORE_BOARD_AI_TIMEATTACK(this, Player, this.milliSec));
                                        this.send(new PACKET_TIMEATTACK_END(this));
                                        endGame();
                                    }

                                    if (InitialTime > 714000) InitialTime = 714000;

                                    if (readyZombie && InitialTime <= 714000)
                                    {
                                        spawnTimeAttack();
                                    }
                                    if (InitialTime <= 0)
                                    {
                                        endGame();
                                    }
                                    //if (AliveUsers(0) == 0)
                                    if (Destructed && ZombieDifficulty == 0)
                                    {
                                        endGame();
                                    }
                                    if (BossKilled && ZombieDifficulty == 1)
                                    {
                                        endGame();
                                    }
                                    if (ZombieDifficulty == 0)
                                    {
                                        foreach (virtualUser Player in Players)
                                            if (Player.rDeaths == 5 && AliveUsers(0) == 0)
                                                endGame();
                                    }

                                    if (ZombieDifficulty == 1)
                                    {
                                        foreach (virtualUser Player in Players)
                                            if (Player.rDeaths == 3 && AliveUsers(0) == 0)
                                                endGame();
                                    }
                                    break;
                                }
                            case 12: // Escaoe Mode
                                {
                                    //if (AliveEscape(true) == 0 && EscapeZombie == 0) endGame();
                                    if (AliveEscape(false) == 0 && EscapeHuman == 0) endGame();
                                    break;
                                }
                                // 4 Conquest - 5 Mission Mode - 6 Deathhill(?)
                        }
                    }
                }
                else
                {
                    RoomManager.removeRoom(Channel, ID);
                    foreach (virtualUser _User in UserManager.getUsersInChannel(Channel, false))
                        if (_User.Page == Math.Floor(Convert.ToDecimal(ID / 14)))
                            _User.send(new PACKET_ROOMLIST_UPDATE(this, 2));
                }
            }
            catch (Exception)
            {
                update();
                //Log.AppendError("Error at Update @ Room: " + ex.Message);
            }
        }

        public int cNiuRounds, cDerbRounds; // CurrentRound

        public ArrayList Plantings = new ArrayList();

        public int AddPlacment(virtualUser User, string ItemCode)
        {
            int PlantingID = Plantings.Count + 1;
            virtualPlacment newPlanting = new virtualPlacment();
            newPlanting.ID = PlantingID;
            newPlanting.Planter = User;
            newPlanting.Used = false;
            newPlanting.Code = ItemCode;
            Plantings.Add(newPlanting);
            return PlantingID;
        }

        public void RemovePlacment(int PlantID)
        {
            foreach (virtualPlacment Plant in Plantings)
            {
                if (Plant.ID == PlantID)
                    Plantings.Remove(Plant);
            }
        }

        public virtualPlacment getPlant(int PlantID)
        {
            foreach (virtualPlacment Plant in Plantings)
            {
                if (Plant.ID == PlantID)
                    return Plant;
            }
            return null;
        }

        public virtualUser getPlantUser(int PlantID)
        {
            foreach (virtualPlacment Plant in Plantings)
            {
                if (Plant.ID == PlantID)
                    return Plant.Planter;
            }
            return null;
        }

        public void ResetRoomStats(virtualUser Player)
        {
            Player.rKills = Player.Weapon = Player.rDeaths = Player.rFlags = Player.rPoints = Player.droppedAmmo = Player.droppedFlash = Player.droppedM14 = 0; // Reset Room Kills & Deaths
            Player.DinarEarned = Player.ExpEarned = Player.EarnedPoints = Player.Class = 0;
            Player.Health = 1000;
            Player.isReady = false;
            Player.isSpawned = false;
            Player.Alive = false;
            Player.ClassCode = "-1";
            Player.currentVehicle = null;
            Player.currentSeat = null;
            Player.TotalWarPoint = 0;
            Player.BackedToRoom = false;
            //Player.IsEscapeZombie = false;
        }

        public string getClanName(int Side)
        {
            foreach (virtualUser RoomUser in Players)
            {
                if (getSide(RoomUser) == Side)
                {
                    return RoomUser.ClanName;
                }
            }
            return "None";
        }

        public void endGame()
        {
            try
            {
                if (EndGameFreeze) return;
                EndGameFreeze = true;
                Thread.Sleep(1000);
                GameActive = false;
                bombPlanted = false;
                bombDefused = false;
                RoundTimeSpent = 10000;

                int SideWon = -1;
                int SideLose = -1;
                string DerbTeam = getClanName(0);
                string NIUTeam = getClanName(1);

                foreach (virtualUser Player in Players)
                {
                    /*Test Fix Bug Second Start FreeWar*/

                    Player.InGame = false;
                    Player.Health = 0;

                    /* */
                    int ExplosiveTeamPoints = (getSide(Player) == 0) ? cDerbExplosivePoints : cNIUExplosivePoints;
                    int haveUP1 = 0;
                    int haveUP2 = 0;
                    int haveDinar = 0;
                    /* PX's Buff*/
                    if (Player.hasItem("CD01") && Player.hasItem("CD02")) haveUP1 = 3;
                    else if (Player.hasItem("CD01")) haveUP1 = 2;
                    else if (Player.hasItem("CD02")) haveUP1 = 1;

                    if (Player.hasItem("CC05")) haveUP2 = 1;

                    if (Player.hasItem("CE01") && Player.hasItem("CE02")) haveDinar = 3;
                    else if (Player.hasItem("CE02")) haveDinar = 2;
                    else if (Player.hasItem("CE01")) haveDinar = 1;

                   
                    double DinarRate = ConfigServer.DinarRate;
                        DinarRate = (SuperMaster) ? +0.10 : DinarRate;
                    double ExpRate = ConfigServer.ExpRate;
                       ExpRate = (SuperMaster) ? +0.05 : ExpRate;
                    double[] PremiumBonus = new double[] { 0, 0.20, 0.30, 0.50, 0.70 };
                    double[] ExpUP = new double[] { 0, 0.20, 0.30, 0.50 };
                    double[] DinUP = new double[] { 0, 0.20, 0.30, 0.50 };
                    double[] DoubleUP = new double[] { 0, 0.25 };
                    ExpRate += PremiumBonus[Player.Premium];
                    ExpRate += ExpUP[haveUP1];
                    ExpRate += DoubleUP[haveUP2];
                    DinarRate += DoubleUP[haveUP2];
                    DinarRate += DinUP[haveDinar];
                    if (Channel == 3)
                    {
                        Player.EarnedPoints = Player.rKills + Player.rPoints;
                    }
                    else
                    {
                        Player.EarnedPoints = (Player.rKills * 5) + Player.rDeaths + Player.rPoints;
                    }

                    int _ExpEarned = 0;
                    int _DinarEarned = 0;

                    _ExpEarned = Convert.ToInt32(Math.Ceiling((double)Player.EarnedPoints * ExpRate) * ExpRate);
                    _DinarEarned = Convert.ToInt32(Math.Ceiling((double)Player.EarnedPoints * DinarRate) * ExpRate);

                    /*if (MapID == 69)
                        Player.playedsEventMap++;

                    if (Player.playedsEventMap >= 3)
                    {
                        int Rand = new Random().Next(1, 4);
                        switch (Rand)
                        {
                            case 1: Player.AddOutBoxItem("DF14", 3); break;
                            case 2: Player.AddOutBoxItem("DJ07", 3); break;
                            case 3: Player.AddOutBoxItem("DG37", 3); break;
                            case 4: Player.AddOutBoxItem("DC40", 3); break;
                        }
                        Player.playedsEventMap = 0;
                    }*/

                    if (Channel != 3) // If isn't Zombie channel
                    {
                        if (Mode == 0 && Channel == 1)
                        {
                            _DinarEarned *= 1;
                            _ExpEarned *= 1;
                        }

                        /*if (Player.PCItem)
                        {
                            _DinarEarned *= 5;
                            _ExpEarned *= 4;
                        }*/

                        if (Structure.isEvent == true)
                        {
                            _ExpEarned *= Structure.EXPEvent;
                            _DinarEarned *= Structure.DinarEvent;
                        }
                    }

                    Player.Exp += _ExpEarned; // Add exp

                    if (RoomType == 3) // If is event room
                    {
                        _DinarEarned += 5000;
                    }
                    else if (RoomType == 1) // If is clanwar
                    {
                        int DerbScore = (Mode == 0 && Channel == 1 ? cDerbRounds : KillsDeberanLeft);
                        int NIUScore = (Mode == 0 && Channel == 1 ? cNiuRounds : KillsNIULeft);
                        if (DerbScore > NIUScore)
                        {
                            SideWon = 0;
                            SideLose = 1;
                        }
                        else if (NIUScore > DerbScore)
                        {
                            SideWon = 1;
                            SideLose = 0;
                        }
                        else
                        {
                            SideWon = 2;
                            SideLose = 2;
                        }
                    }

                    string LastJoin = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    DB.runQuery("INSERT INTO users_matchs (userid, exp, dinar, channel, mode, kills, deaths, mapid, date, roomtype, level, premium) VALUES ('" + Player.UserID + "', '" + _ExpEarned + "', '" + _DinarEarned + "', '" + Channel + "', '" + Mode + "', '" + Player.rKills + "', '" + Player.rDeaths + "', '" + MapID + "', '" + LastJoin + "', '" + RoomType + "', '" + LevelCalculator.getLevelforExp(Player.Exp) + "', '" + Player.Premium + "')");
                    #region levelup
                    int CurrentLevel = LevelCalculator.getLevelforExp(Player.Exp);
                    if (Player.Exp >= LevelCalculator.getExpForLevel(CurrentLevel + 1)) // If ranked up
                    {
                        int Dinar = 0;
                        int Cash = 0;
                        int Level = LevelCalculator.getLevelforExp(Player.Exp);
                        if (Level < 7)
                        { Dinar = 5000; Cash = 500; }
                        else if (Level < 13)
                        { Dinar = 10000; Cash = 500; }
                        else if (Level < 18)
                        { Dinar = 15000; Cash = 500; }
                        else if (Level < 23)
                        { Dinar = 20000; Cash = 500; }
                        else if (Level < 29)
                        { Dinar = 25000; Cash = 500; }
                        else if (Level < 35)
                        { Dinar = 30000; Cash = 500; }
                        else if (Level < 41)
                        { Dinar = 35000; Cash = 500; }
                        else if (Level < 55)
                        { Dinar = 40000; Cash = 500; }
                        else if (Level < 100)
                        { Dinar = 50000; Cash = 500; }

                        Player.Dinar += Dinar;
                        Player.Cash += Cash;
                        Player.send(new PACKET_LEVEL_UP(Player, Dinar/*,item*/));
                        Log.AppendText("---" + Player.Nickname + " has level upped to " + LevelCalculator.getLevelforExp(Player.Exp) + "---");
                    }

                    #endregion
                    Player.Dinar += _DinarEarned; // Add dinar
                    Player.ExpEarned = _ExpEarned;
                    Player.DinarEarned = _DinarEarned;

                    if (Channel != 3)
                    {
                        Player.Headshots += Player.rHeadshots;
                        Player.Kills += Player.rKills;
                        Player.Deaths += Player.rDeaths;
                        int RandomAmount = new Random().Next(1, 50);
                        Player.Cash += RandomAmount;
                    }
                    DB.runQuery("UPDATE users SET headshots='" + Player.Headshots + "', kills='" + Player.Kills + "', deaths='" + Player.Deaths + "', exp='" + Player.Exp + "', dinar='" + Player.Dinar + "', cash='" + Player.Cash + "' WHERE id=" + Player.UserID);
                }
                #region CW
                if (SideWon != -1 && SideLose != -1 && CWCheck == true)
                {
                    int WonClanID = getClanID(SideWon);
                    int LoseClanId = getClanID(SideLose);
                    ClanManager.getClan(WonClanID).clanWarWin++;
                    ClanManager.getClan(LoseClanId).clanWarLose++;
                    ClanManager.getClan(WonClanID).clanEXP += 2;
                    ClanManager.getClan(LoseClanId).clanEXP++;
                    DateTime current = DateTime.Now;
                    string DateOfCW = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    int DerbScore = 0;
                    int NIUScore = 0;
                    if (SideLose != 2 && SideWon != 2)
                    {
                        DerbScore = (Mode == 0 && Channel == 1 ? cDerbRounds : KillsDeberanLeft);
                        NIUScore = (Mode == 0 && Channel == 1 ? cNiuRounds : KillsNIULeft);
                        DB.runQuery("UPDATE clans SET win=win+1, exp=exp+2 WHERE id='" + WonClanID + "'");
                        DB.runQuery("UPDATE clans SET lose=lose+1, exp=exp+1 WHERE id='" + LoseClanId + "'");
                    }
                    if (cDerbRounds == 0)
                        DerbScore = 0;
                    if (cNiuRounds == 0)
                        NIUScore = 0;
                    Log.AppendText(getClanName(0) + " vs. " + getClanName(1) + " | " + DerbScore + " - " + NIUScore + " end!");
                    DB.runQuery("INSERT INTO clans_clanwars (clanname1, clanname2, score, clanwon, date) VALUES ('" + getClanName(0) + "', '" + getClanName(1) + "', '" + DerbScore + "-" + NIUScore + "', '" + ClanManager.getClan(WonClanID).clanName + "', '" + DateOfCW + "')");
                }
                #endregion
                if (Channel == 3)
                { send(new PACKET_END_GAME_AI(this)); }
                else
                { send(new PACKET_END_GAME(this)); }

                foreach (virtualUser Player in Players)
                {
                    //ResetRoomStats(Player);
                    if (RoomType == 3)
                    {
                        Player.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> You got 5,000 dinars for playing an event!!", 999, "NULL"));
                    }
                    Player.send(new SP_ROOM_INFO(51, RoomMasterSlot, 55, 2, 1, 0, MapID, 0, 0, 0, 0, 0, 0, 0));
                }
                highestKills = cNiuRounds = cDerbRounds = KillsNIULeft = KillsDeberanLeft = KillsNiu = KillsDerb = DeathNiu = DeathDerb = 0;

                RoomStatus = 1;
            }
            catch { }
        }

        public void remove()
        {
            if (Players.Count > 0)
            {
                if (RoomStatus != 1)
                {
                    endGame();
                }

                foreach (virtualUser Player in Players)
                {
                    Player.send(new PACKET_ROOM_KICK(Player.RoomSlot));
                }
            }
            RoomManager.removeRoom(this.Channel, this.ID);
            foreach (virtualUser _User in UserManager.getUsersInChannel(Channel, false))
            {
                if (_User.Page == Math.Floor(Convert.ToDecimal(ID / 14)))
                {
                    _User.send(new PACKET_ROOMLIST_UPDATE(this, 2));
                }
            }
        }

        public int getSide(virtualUser Client)
        {
            if (Client.isSpectating) return -1;
            if (Channel == 3)
            {
                if (Client.HWID != null) return 0; else return 1;
            }

            if (Client.RoomSlot < (MaxPlayers / 2))
                return 0;
            else
                return 1;
        }

        public int getSideCount(int Side)
        {
            int Count = 0;
            foreach (virtualUser RoomUser in Players)
            {
                if (getSide(RoomUser) == Side)
                    Count++;
            }
            return Count;
        }

        public int getIDOfPlayer(virtualUser User)
        {
            for (int i = 0; i < MaxPlayers; i++)
            {
                if ((oPlayers.GetValue(i) is virtualUser) && oPlayers.GetValue(i).Equals(User))
                {
                    return i;
                }
            }

            return -1;
        }

        public bool isMyClan(virtualUser User)
        {
            foreach (virtualUser RoomUser in Players)
            {
                if (User.ClanID == RoomUser.ClanID)
                    return true;
            }
            return false;
        }

        public int getClanSide(virtualUser User)
        {
            foreach (virtualUser RoomUser in Players)
            {
                if (RoomUser.ClanID == User.ClanID)
                {
                    return getSide(RoomUser);
                }
            }
            if (getSideCount(0) == 0) return 0;
            else if (getSideCount(1) == 0) return 1;
            return -1;
        }

        public int getClanID(int Side)
        {
            int ID = -1;
            foreach (virtualUser Clients in Players)
            {
                if (getSide(Clients) == Side)
                    ID = Clients.ClanID;
            }
            return ID;
        }

        public bool joinClanWar(virtualUser User)
        {
            if (User.ClanID == -1 || User.ClanRank == -1) return false;
            int ClanSide = getClanSide(User);

            if (ClanSide == -1) return false;

            if (getSideCount(ClanSide) > (MaxPlayers / 2)) return false;

            if (Users.Count <= 0)
            {
                User.Room = this;
                User.RoomSlot = 0;
                Users.Add(0, User);
                UsersDic.TryAdd(0, User);
                RoomMaster = 0;
                return true;
            }
            else
            {
                if (Users.Count < MaxUsers)
                {
                    int Incr = 0;
                    for (int I = 0; I < MaxUsers; I++)
                    {
                        if (ClanSide == 0)
                        {
                            if (Users.ContainsKey(I / 2) == false)
                            {
                                User.RoomSlot = I / 2;
                                if (CheckForRoomSlot(User) == false) return false;
                                User.Room = this;
                                Users.Add(I / 2, User);
                                UsersDic.TryAdd(I / 2, User);
                                User.send(new PACKET_JOIN_ROOM(User, this));
                                DeberanPlayers.Add(User);
                                //Name = getClanName(0) + " vs. " + getClanName(1);
                                return true;
                            }
                        }
                        else
                        {
                            if (Users.ContainsKey((MaxUsers / 2) + Incr) == false)
                            {
                                User.RoomSlot = (MaxUsers / 2) + Incr;
                                if (CheckForRoomSlot(User) == false) return false;
                                User.Room = this;
                                Users.Add((MaxUsers / 2) + Incr, User);
                                UsersDic.TryAdd((MaxUsers / 2) + Incr, User);
                                User.send(new PACKET_JOIN_ROOM(User, this));
                                NIUPlayers.Add(User);
                                //Name = getClanName(0) + " vs. " + getClanName(1);
                                return true;
                            }
                            Incr++;
                        }
                    }
                }
            }
            return false;
        }

        public int FreeRoomSlotBySide(int side)
        {
            lock (this)
            {
                for (int i = (side == (int)1 ? (MaxUsers / 2) : 0); i < (side == (int)1 ? MaxUsers : (MaxUsers / 2)); i++)
                {
                    if (Users.ContainsKey(i) == false)
                    {
                        return i;
                    }
                }
                return -1;
            }
        }

        public bool joinUser(virtualUser User, int side = 2)
        {
            User.isReady = false;
            User.isSpawned = false;
            User.rKills = User.rDeaths = User.rPoints = 0;
            ResetRoomStats(User);
            User.Health = 0;
            User.ClassCode = "-1";

            if (RoomType == 1)
            {
                if (joinClanWar(User) == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            if (Users.Count <= 0)
            {
                User.Room = this;
                User.RoomSlot = 0;
                Users.Add(0, User);
                UsersDic.TryAdd(0, User);
                RoomMaster = 0;
                return true;
            }
            else
            {
                if (Users.Count < MaxUsers)
                {
                    User.Health = -1;
                    if (User.Channel != 3)
                    {
                        if (side == 0 || side == 1)
                        {
                            int EnemySide = side == 0 ? 1 : 0;
                            if (getSideCount(side) <= getSideCount(EnemySide))
                            {
                                User.RoomSlot = FreeRoomSlotBySide(side);
                                if (User.RoomSlot != -1)
                                {
                                    //if(GameActive){Users.Playing = true;}//aggiungere bool Users.Playing
                                    User.Room = this;
                                    Users.Add(User.RoomSlot, User);
                                    UsersDic.TryAdd(User.RoomSlot, User);
                                    User.send(new PACKET_JOIN_ROOM(User, this));
                                    if (side == 0)
                                    {
                                        DeberanPlayers.Add(User);
                                    }
                                    else if (side == 1)
                                    {
                                        NIUPlayers.Add(User);
                                    }
                                    return true;
                                }
                            }
                        }
                        else
                        {
                            int randomSide = (getSideCount(1) >= getSideCount(0) ? 0 : 1);
                            int rs = FreeRoomSlotBySide(randomSide);
                            if (rs != -1)
                            {
                                //if(GameActive){Users.Playing = true;}//aggiungere bool Users.Playing
                                User.RoomSlot = rs;
                                User.Room = this;
                                Users.Add(User.RoomSlot, User);
                                UsersDic.TryAdd(User.RoomSlot, User);
                                User.send(new PACKET_JOIN_ROOM(User, this));
                                if (randomSide == 0)
                                {
                                    DeberanPlayers.Add(User);
                                }
                                else if (randomSide == 1)
                                {
                                    NIUPlayers.Add(User);
                                }
                                return true;
                            }

                        }
                    }
                    else
                    {
                        for (int I = 0; I < 4; I++)
                        {
                            if (Users.ContainsKey(I) == false)
                            {
                                //if(GameActive){Users.Playing = true;}//aggiungere bool Users.Playing
                                User.RoomSlot = I;
                                User.Room = this;
                                UsersDic.TryAdd(User.RoomSlot, User);
                                Users.Add(User.RoomSlot, User);
                                User.send(new PACKET_JOIN_ROOM(User, this));
                                DeberanPlayers.Add(User);
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        public bool CheckForRoomSlot(virtualUser Player)
        {
            foreach (virtualUser User in Players)
                if (User.RoomSlot == Player.RoomSlot)
                {
                    Player.RoomSlot = 0;
                    return false;
                }

            return true;
        }

        /*public ArrayList ZombiesFollow = new ArrayList();
        public void ZombieFollowers(int RoomSlot)
        {
            ZombiesFollow.Clear();
            foreach (virtualZombie _Zombie in Zombies)
            {
                if (_Zombie.FollowUser == RoomSlot)
                {
                    ZombiesFollow.Add(_Zombie);
                }

            }

        }*/

        public List<virtualZombie> ZombieFollowers(int SlotID)
        {
            return Zombies.Values.Where(r => r != null && r.FollowUser == SlotID).ToList();
        }

        public virtualUser getPlayer(int SlotID)
        {
            if (Users.ContainsKey(SlotID))
            {
                return (virtualUser)Users[SlotID];
            }
            return null;
        }

        public bool RemoveUser(int SlotID)
        {
            try
            {
                if (SlotID >= 0 && Users.ContainsKey(SlotID))
                {
                    virtualUser uObj = (virtualUser)Users[SlotID];
                    if (uObj.currentVehicle != null)
                    {
                        uObj.currentVehicle.Leave(uObj);//TODO: Remove user from the car (202 subtype of the room data)
                    }

                    if (Channel != 3)
                    {
                        uObj.Kills += uObj.rKills;
                        uObj.Deaths += uObj.rDeaths;
                        uObj.Headshots += uObj.rHeadshots;
                    }
                    uObj.Room = null;
                    uObj.RoomSlot = -1;
                    DB.runQuery("UPDATE users SET kills='" + uObj.Kills + "', deaths='" + uObj.Deaths + "', headshots='" + uObj.Headshots + "' WHERE id='" + uObj.UserID + "'");
                    Users.Remove(SlotID);
                    virtualUser ur;
                    UsersDic.TryRemove(SlotID, out ur);

                    if (Mode == 1 && Channel == 1)
                    {// Check if is FFA
                        foreach (virtualUser User in Players)
                        {
                            if (User.rKills >= highestKills)
                            {
                                highestKills = User.rKills; // Set highest FFA kill
                            }
                        }
                    }
                    //else if (RoomType == 1)
                    //Name = getClanName(0) + " vs. " + getClanName(1);

                    if (SlotID == RoomMaster && Users.Count > 0) /* Select New Master First */
                    {
                        SuperMaster = false; // Remove Exp Buff
                        for (int I = 0; I < MaxPlayers; I++)
                        {
                            if (Users.ContainsKey(I))
                            {
                                RoomMaster = I;
                                break;
                            }
                        }
                    }

                    CheckAliveRoomSlot();

                    if (Channel == 3 && ListCheckAliveRoomSlot.Count >= 1)
                    {
                        send(new PACKET_ZOMBIR_CHANGE_ENEMY(this, SlotID));
                    }

                    send(new PACKET_LEAVE_ROOM(uObj, this, SlotID, RoomMaster)); // Send to the room about the user left

                    uObj.send(new PACKET_LEAVE_ROOM(uObj, this, SlotID, RoomMaster));

                    if (RoomStatus != 1 && Users.Count <= 1 && Channel != 3)
                    {
                        endGame(); // End Game
                    }
                    else if (Users.Count <= 0) // Remove room if is empty
                    {
                        RoomManager.removeRoom(this.Channel, this.ID);
                        foreach (virtualUser _User in UserManager.getUsersInChannel(Channel, false))
                        {
                            if (_User.Page == Math.Floor(Convert.ToDecimal(ID / 14)))
                            {
                                _User.send(new PACKET_ROOMLIST_UPDATE(this, 2));
                            }
                        }
                    }

                    // Update the lobby
                    foreach (virtualUser _User in UserManager.getUsersInChannel(Channel, false))
                    {
                        if (_User.Page == Math.Floor(Convert.ToDecimal(ID / 14)))
                        {
                            _User.send(new PACKET_ROOMLIST_UPDATE(this));
                        }
                    }
                    return true;
                }

                return false;
            }
            catch { return false; }
        }
    }
}