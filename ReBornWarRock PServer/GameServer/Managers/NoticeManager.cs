using System;
using System.Text;
using System.Threading;

namespace ReBornWarRock_PServer.GameServer.Managers
{
    class NoticeManager
    {
        private static Thread _NoticeThread = null;
        private static Thread _NoticeServerThread = null;
        private static string[] _Messages;
         

        ~NoticeManager()
        {
            GC.Collect();
        }

        public static bool load()
        {
            try
            {
                _Messages = null; // Clean it
                _Messages = DB.runReadColumn("SELECT message FROM notices WHERE deleted='0'", 0);


                if (_NoticeThread == null)
                {
                    _NoticeServerThread = new Thread(new ThreadStart(noticeServerLoop));
                    _NoticeServerThread.Priority = ThreadPriority.Lowest;
                    _NoticeServerThread.Start();
                }
                else
                {
                    _NoticeServerThread.Start();
                }

                if (_NoticeThread == null)
                {
                    _NoticeThread = new Thread(new ThreadStart(noticeLoop));
                    _NoticeThread.Priority = ThreadPriority.Lowest;
                    _NoticeThread.Start();
                }
                else
                {
                    _NoticeThread.Start();
                }
            }
            catch { Log.AppendText("Failed to load the notice manager!"); }
            return false;
        }

        private static void noticeServerLoop()
        {
            try
            {
                while (Structure.RunningServer)
                {
                    TimeSpan _StartTime = DateTime.Now - System.Diagnostics.Process.GetCurrentProcess().StartTime;
                    //UserManager.sendToServer(new Networking.Packets.PACKET_CHAT("Server", Networking.Packets.PACKET_CHAT.ChatType.Notice1, " NOTICE: Server has " + UserManager.getAllUsers().Count + " online players. GameServer online since " + _StartTime.Days.ToString() + " days, " + _StartTime.Hours.ToString() + " hours, " + _StartTime.Minutes.ToString() + " minutes :)", 100, "NULL"));
                    Thread.Sleep(250000);
                }
            }
            catch { }
        }


        private static void noticeLoop()
        {
            try
            {
                while (Structure.RunningServer)
                {
                    if (_Messages.Length > 0)
                    {
                        int iMessage = (new System.Random()).Next(0, _Messages.Length - 1);
                        int Random = new Random().Next(0, 1);
                        if (Random == 0) UserManager.sendToServer(new Networking.Packets.PACKET_CHAT(" NOTICE: ", Networking.Packets.PACKET_CHAT.ChatType.Notice1,"FreeWar GUI Server", 100, "NULL"));
                        if (Random == 1) UserManager.sendToServer(new Networking.Packets.PACKET_CHAT(" NOTICE: ", Networking.Packets.PACKET_CHAT.ChatType.Notice1, _Messages[iMessage], 100, "NULL"));
                    }
                    Thread.Sleep(400000);
                }
            }
            catch { }
        }
    }
}
