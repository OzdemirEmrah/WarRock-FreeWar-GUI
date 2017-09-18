using ReBornWarRock_PServer.GameServer;
using ReBornWarRock_PServer.GameServer.Managers;
using ReBornWarRock_PServer.GameServer.Networking.Packets;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.Room;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;
using ReBornWarRock_PServer.LoginServer;
using ReBornWarRock_PServer.LoginServer.Connection;
using ReBornWarRock_PServer.LoginServer.Docs;
using ReBornWarRock_PServer.LoginServer.Virtual.User;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReBornWarRock_PServer
{
    class Structure
    {
        public static int _AcceptedLogins;
        private static String _ServerKey = "";
        public static ConcurrentDictionary<string, User> LogFromLauncher = new ConcurrentDictionary<string, User>();

        public static string Format = "0";
        public static string Launcher = "0";
        public static string Updater = "0";
        public static string Client = "0";
        public static string Sub = "0";
        public static string Option = "0";

        public static string UpdateUrl = "";

        public static String ServerKey { get { return _ServerKey; } }

        private static Thread _ServerThread = null;
        private static Thread _CommandThread = null;

        private static bool _RunningLogin = false;
        public static bool RunningLogin { get { return _RunningLogin; } }
        public static bool _RunningServer = false;
        public static bool RunningServer { get { return _RunningServer; } }
        public static int BootTime;
        public static bool isEvent = false;
        public static int EventTime = -1;
        public static int EXPEvent = -1;
        public static int DinarEvent = -1;
        public static int EXPBanner = 0;
        public static int Debug = 0;
        public static bool PacketView = false;
        public static LookupService LookupModule;
        public static string Info = null;
        public static int TextBoxForPlayers = 0;

        /*[BINGO]*/
        public static bool BingoActive = false;
        public static int BingoTime = 36000000;
        /*FreeWar*/

        public static string convertToMD5(string Input)
        {
            try
            {
                System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(Input);
                byte[] hash = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int I = 0; I < hash.Length; I++)
                {
                    sb.Append(hash[I].ToString("x2"));
                }

                return sb.ToString();
            }
            catch { return null; }
        }

        public static long UnixTimestamp
        {
            get
            {
                TimeSpan ts = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
                return Convert.ToInt64(ts.TotalSeconds);
            }
        }

        public static int timestamp
        {
            get
            {
                TimeSpan ts = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
                return (int)(ts.TotalSeconds);
            }
        }

        public static String hashMD5(String Input)
        {
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(Input);
            byte[] hash = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }
            return sb.ToString();
        }
        public static void serverLoop()
        {

            try
            {
                while (!_RunningServer) { Thread.Sleep(100); }

                while (_RunningServer)
                {
                    Label t = Application.OpenForms["Form1"].Controls["label5"] as Label;
                    Info = ("RAM Usage : "  + (GC.GetTotalMemory(false) / 1024) + " KB" + " | " + UserManager.UserCount + " Online Players").ToString();
                    FormCalling.frm1.AppendTextBox1(Info);
                    if (UserManager.UserCount > 0) FormCalling.frm1.button6InvokeVisibility("true");
                    /*TimeSpan _StartTime = DateTime.Now - System.Diagnostics.Process.GetCurrentProcess().StartTime;
                    Console.Title = "FreeWar&Cod4 Emulator - Game Server | Ram usage: " + (GC.GetTotalMemory(false) / 1024) + " KB" + " | " + UserManager.UserCount + " online players (Running since: " + _StartTime.Days + " days, " + _StartTime.Hours + " hours, " + _StartTime.Minutes + ", minutes!)";
                    */
                    Thread.Sleep(2000);
                }
            }
            catch (Exception ex)
            {

                Log.AppendError(ex.Message);
            }
        }

        public static void LoginEventCheckLoop()
        {
            try
            {
                while (true)
                {
                    DateTime current = DateTime.Now;
                    long StartTime = long.Parse(String.Format("{0:yyMMdd}", current));
                    if (Structure.BootTime < StartTime)
                    {
                        Structure.BootTime = Convert.ToInt32(StartTime);
                        DB.runQuery("UPDATE users SET logineventcheck='0'");
                        DB.runQuery("DELETE FROM users_events WHERE eventid='5'");
                        DB.runQuery("DELETE FROM users_events WHERE eventid='6'");
                        foreach (virtualUser Players in UserManager.getAllUsers())
                        {
                            Players.LoginEventCheck = 0;
                            if (Players.LoginEvent >= 7)
                            {
                                DB.runQuery("UPDATE users SET loginevent='0' WHERE id='" + Players.UserID + "'");
                            }
                        }
                    }
                    Thread.Sleep(600000);
                }
            }
            catch (Exception ex)
            {
                Log.AppendError(ex.Message);
            }
        }
        public static void CheckCouponLoop()
        {
            try
            {
                while (true)
                {
                    DateTime current = DateTime.Now;
                    long StartTime = long.Parse(String.Format("{0:yyMMdd}", current));
                    if (Structure.BootTime < StartTime)
                    {
                        Structure.BootTime = Convert.ToInt32(StartTime);
                        foreach (virtualUser Players in UserManager.getAllUsers())
                        {
                            Players.TodayCoupon = 0;
                            DB.runQuery("UPDATE users SET todaycoupon='0'");
                            DB.runQuery("DELETE FROM users_events WHERE eventid='5'");
                        }
                    }
                    Thread.Sleep(60000);
                }
            }
            catch (Exception ex)
            {
                Log.AppendError(ex.Message);
            }
        }

        public static void Event()
        {
            while (true)
            {
                if (EventTime < -1) EventTime = -1;
                if (EventTime == -1)
                {
                    Form3 frm3 = FormCalling.frm3;
                    frm3.UnlockAll("true");
                    isEvent = false;
                    EventTime = -1;
                    EXPEvent = 1;
                    DinarEvent = 1;
                    if(EXPBanner == 5)
                    {
                        foreach (virtualUser Player in UserManager.getAllUsers())
                        {
                            string[] Event = DB.runReadRow("SELECT userid, eventid FROM users_events WHERE userid='" + Player.UserID + "'");
                            if (Event[0] == Player.UserID.ToString())
                            {
                                Player.ReceivedRandomBox = false;
                                DB.runQuery("DELETE FROM users_events WHERE eventid='5' AND userid ='" + Player.UserID + "'");
                                //Player.send(new PACKET_LOGIN_EVENT(Player , "CZ99"));
                            }
                        }
                    }
                    EXPBanner = 0;
                    
                }
                else
                {
                    EventTime -= 10;
                    if(EXPBanner == 5)
                    {
                        foreach (virtualUser Player in UserManager.getAllUsers())
                        {
                            string[] Event = DB.runReadRow("SELECT userid, eventid FROM users_events WHERE userid='" + Player.UserID + "'");
                            if (Event.Length <= 0)
                            {
                                Player.AddItem("CZ99", -1, 1);
                                Player.ReceivedRandomBox = true;
                                Player.send(new PACKET_LOGIN_EVENT_MESSEGE(Player));
                                DB.runQuery("INSERT INTO users_events (eventid, userid) VALUES ('" + 5 + "','" + Player.UserID + "')");
                                //Player.send(new PACKET_LOGIN_EVENT(Player , "CZ99"));
                            }

                        }
                    }
                }
                if (Structure.BingoActive)
                {
                    if (Structure.BingoTime == 0)
                    {
                        BingoWin.Extract();
                        Structure.BingoTime = 36000000;
                    }
                    else Structure.BingoTime -= 10000;
                }
                foreach (virtualUser Players in UserManager.getAllUsers())
                {
                    Players.send(new PACKET_PING(Players));
                }
                Thread.Sleep(10000);
            }
        }

        public static void shutDownByStop()
        {
            if (!_RunningServer)
            {
                Environment.Exit(0);
                return;
            }
            try
            {
                for (int I = 1; I <= 3; I++)
                {
                    foreach (virtualRoom _Room in RoomManager.getRoomsInChannel(I))
                    {
                        if (_Room == null) continue;
                        _Room.endGame();
                    }
                }
            }
            catch { }
            //DB.closeConnection();
            // MYSQL.closeConnection();
            Thread.Sleep(5000);

            try
            {
                foreach (virtualUser _User in UserManager.getAllUsers())
                {
                    if (_User == null) continue;
                    _User.disconnect();
                }
            }
            catch { }

            //Environment.Exit(0);

            _RunningServer = false;

            try
            {
                if (_ServerThread.IsAlive)
                    _ServerThread.Abort();
            }
            catch { }

            try
            {
                if (_CommandThread.IsAlive)
                    _CommandThread.Abort();
            }
            catch { }

            Program.sClient.CloseSocket();
            GameServer.NetworkSocket.CloseSocket();

            DB.runQuery("UPDATE users SET online='0'");
            Log.AppendText("All accounts have been set offline");
            //Environment.Exit(0);
        }

        public static void shutDown()
        {
            if (!_RunningServer)
            {
                Environment.Exit(0);
                return;
            }
            try
            {
                for (int I = 1; I <= 3; I++)
                {
                    foreach (virtualRoom _Room in RoomManager.getRoomsInChannel(I))
                    {
                        if (_Room == null) continue;
                        _Room.endGame();
                    }
                }
            }
            catch { }
            DB.closeConnection();
            MYSQL.closeConnection();
            Thread.Sleep(5000);

            try
            {
                foreach (virtualUser _User in UserManager.getAllUsers())
                {
                    if (_User == null) continue;
                    _User.disconnect();
                }
            }
            catch { }

            //Environment.Exit(0);

            _RunningServer = false;

            try
            {
                if (_ServerThread.IsAlive)
                    _ServerThread.Abort();
            }
            catch { }

            try
            {
                if (_CommandThread.IsAlive)
                    _CommandThread.Abort();
            }
            catch { }

            Program.sClient.CloseSocket();
            GameServer.NetworkSocket.CloseSocket();

            DB.runQuery("UPDATE users SET online='0'");
            Log.AppendText("All accounts have been set offline");
            Environment.Exit(0);
        }

        public static void commandLoop()
        {
            /*try
            {
                while (_Running)
                {
                    string Command = Console.ReadLine();
                    string[] args = Command.Split(' ');
                    switch (args[0].ToLower())
                    {
                        case "cmd":
                            {
                                Log.AppendText("notice");
                                Log.AppendText("p = view packets");
                                Log.AppendText("roominfo");
                                Log.AppendText("kick");
                                Log.AppendText("stop");
                                Log.AppendText("/");
                                Log.AppendText("debug = starta room da solo");
                                break;
                            }
                        case "notice":
                            {
                                foreach (virtualUser Client in UserManager.getAllUsers())
                                    Client.send(new PACKET_CHAT("Notice", PACKET_CHAT.ChatType.Notice1, Command.Substring(7), 100, "Notice"));

                                break;
                            }
                        case "p":
                            {
                                if (!PacketView) PacketView = true;
                                else PacketView = false;
                                Log.AppendText("SYSTEM >> Packet Visible: " + Convert.ToString(PacketView));
                                break;
                            }
                        case "roominfo":
                            {
                                int RoomID = Convert.ToInt32(args[1]);
                                int RoomChannel = Convert.ToInt32(args[2]);
                                virtualRoom TargetRoom = RoomManager.getRoom(RoomChannel, RoomID);
                                if (TargetRoom == null) break;
                                Log.AppendText("SYSTEM >> Informazioni stanza N° " + RoomID);
                                Log.AppendText("SYSTEM >> Room Nome: " + TargetRoom.Name);
                                Log.AppendText("SYSTEM >> Room Status: " + (TargetRoom.RoomStatus == 2 ? "Play" : "Wait"));
                                Log.AppendText("SYSTEM >> Password: " + TargetRoom.Password + " / MapID: " + TargetRoom.MapID);
                                Log.AppendText("SYSTEM >> Type: " + TargetRoom.RoomType + " / Mode:" + TargetRoom.Mode);
                                Log.AppendText("SYSTEM >> Players: " + TargetRoom.Players.Count + "/" + TargetRoom.MaxPlayers + ", Spectators " + TargetRoom.Spectators.Count + "/10");
                                break;
                            }
                        case "kick":
                            {
                                foreach (virtualUser Client in UserManager.getAllUsers())
                                {
                                    if (Client == null) continue;

                                    if (Client.Nickname.ToLower().Equals(args[1].ToLower()) || Client.Username.ToLower().Equals(args[1].ToLower()))
                                    {
                                        Client.disconnect();
                                        Log.AppendText("User " + Client.Nickname + " è stato kikkato dal server!");
                                        break;
                                    }
                                }
                                Log.AppendError("User " + args[1] + " Non online o inesistente!");
                                break;
                            }
                        case "reload":
                            {
                                ItemManager.DecryptBinFile("items.bin");
                                ItemManager.LoadItems();
                                Log.AppendText("SYSTEM --> Item Manager is been reloaded!");
                                virtualMapData.Load();
                                Log.AppendText("SYSTEM --> MapData is been reloaded!");
                                break;
                            }
                        case "stop":
                            {
                                foreach (virtualUser Player in UserManager.getAllUsers())
                                {
                                    Player.send(new PACKET_CHAT("Server", PACKET_CHAT.ChatType.Notice1, "ATTENZIONE: Spiacente, è necessario riavviare il server!!!", 100, "NULL"));
                                    Player.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM: Spiacente, è necessario riavviare il server!!!", 999, "Server"));
                                }
                                Log.AppendText("SYSTEM --> SERVER STOPPING!!");
                                Thread.Sleep(500);
                                shutDown();
                                break;
                            }
                        case "/":
                            {
                                foreach (virtualUser Player in UserManager.getAllUsers())
                                {
                                    Player.send(new PACKET_CUSTOM(args));
                                }
                                break;
                            }
                        case "debug":
                            {
                                if (Debug == 0) Debug = 1;
                                else Debug = 0;
                                Log.AppendText("SYSTEM >> Debug: " + Convert.ToString(Debug));
                                break;
                            }
                        default:
                            {
                                Log.AppendText("comando sconosciuto, riprova");
                                break;
                            }
                    }
                }
            }
            catch { }*/
        }
        public static bool StartUpLogin()
        {
            try
            {
                if (ServerSocket.openSocket(5010, 5) == false)
                {
                    Console.ReadKey();
                    return false;
                }

                // If tcp socket 5330 is not open //
                if (LoginServer.Connection.NetworkSocket.openSocket(5330) == false)
                {
                    Console.ReadKey();
                    return false;
                }

                ServersInformations.LOAD();
                LoginServer.Docs.PacketManager.setup();
                ServerManager.setup(0);
                //BanManager.load();
                FormCalling.frm1.AppendColorLabelBox1("");
                FormCalling.frm1.AppendLabelBox1("Started");
                Log.WriteBlank();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.Read();
                return false;
            }
        }
        public static void StartUpServer()
        {
            GameServer.Managers.PacketManager.setup();

            if (_RunningServer) return;
            _RunningServer = true;
            DateTime Start = DateTime.Now;
            
            Log.AppendText("");
            Log.AppendText("WarRocK GameServer started!");
            LookupModule = new LookupService("GeoIP.dat", LookupService.GEOIP_MEMORY_CACHE);
            Log.AppendText("GeoIP module initialized.");
            DB.runQuery("UPDATE users SET online='0'");
            Log.AppendText("All accounts have been set offline");

            
            _ServerThread = new Thread(new ThreadStart(Structure.serverLoop));
            _ServerThread.Priority = ThreadPriority.BelowNormal;
            _ServerThread.Start();

            _CommandThread = new Thread(new ThreadStart(Structure.commandLoop));
            _CommandThread.Priority = ThreadPriority.BelowNormal;
            _CommandThread.Start();


            //ItemManager.InitializeHexTable();

            Log.WriteBlank();

            ClanManager.Load();
            GameServer.Managers.BanManager.load();
            UserManager.setup();
            RoomManager.setup();
            virtualMapData.Load();
            EventManager.Load();
            NoticeManager.load();
            MapVehicleSeats.Load();
            VehicleManager.Load();
            WordManager.Load();
            //VehicleManagers2.Load();
            MapVehicles.Load();
            ZombieManager.Load();
            Thread EventThread = new Thread(Event);
            EventThread.Priority = ThreadPriority.AboveNormal;
            EventThread.Start();
            Thread LoginEventThread = new Thread(LoginEventCheckLoop);
            LoginEventThread.Priority = ThreadPriority.AboveNormal;
            LoginEventThread.Start();
            Thread CouponCheckThread = new Thread(CheckCouponLoop);
            CouponCheckThread.Priority = ThreadPriority.AboveNormal;
            CouponCheckThread.Start();
            //MapManager.load();
            

            DateTime current = DateTime.Now;
            long StartTime = long.Parse(String.Format("{0:yyMMdd}", current));

            BootTime = Convert.ToInt32(StartTime.ToString());

            FormCalling.frm1.AppendColorLabelBox2("");
            FormCalling.frm1.AppendLabelBox2("Started");
            TimeSpan bootTime = DateTime.Now - Start;
            Log.AppendText("Emulator has booted in " + bootTime.TotalMilliseconds + " milliseconds..");

            
            Log.WriteBlank();

            if (GameServer.NetworkSocket.openSocket(5340, 999999999) == false)
            {
                return;
            }
            GC.Collect();
        }

        public static System.Drawing.Color ConvertHexToRGB(string color)
        {
            byte r = 0;
            byte g = 0;
            byte b = 0;

            if (color.StartsWith("#"))
            {
                color = color.Remove(0, 1);
            }

            if (color.Length == 3)
            {
                r = Convert.ToByte(color[0] + "" + color[0], 16);
                g = Convert.ToByte(color[1] + "" + color[1], 16);
                b = Convert.ToByte(color[2] + "" + color[2], 16);
            }
            else if (color.Length == 6)
            {
                r = Convert.ToByte(color[0] + "" + color[1], 16);
                g = Convert.ToByte(color[2] + "" + color[3], 16);
                b = Convert.ToByte(color[4] + "" + color[5], 16);
            }
            return System.Drawing.Color.FromArgb(r, g, b);
        }

        public static long currTimeStamp
        {
            get
            {
                TimeSpan ts = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
                return Convert.ToInt64(ts.TotalSeconds);
            }
        }


    }
}
