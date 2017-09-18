using ReBornWarRock_PServer.GameServer.Networking;
using System;
using System.Collections;
namespace ReBornWarRock_PServer.GameServer
{
    public class virtualProtection
    {
        private static ArrayList Collections = new ArrayList();
        public string IP;
        public int Connections;
        public int BannedRoutine;
        public int BannedTimes;
        ~virtualProtection()
        {
            GC.Collect();
        }
        public static void clearConnections()
        {
            NetworkSocket.BannedIPs.Clear();
            foreach (virtualProtection Protection in Collections)
            {
                Protection.Connections = Protection.BannedRoutine = Protection.BannedTimes = 0;
            }
            Log.WriteDoss("All banned IPs cleared. [15 minute routine check]");
        }
        public static void RunCheck()
        {
            try
            {
                foreach (virtualProtection Protection in NetworkSocket.BannedIPs)
                {
                    if (Protection.BannedRoutine >= 30 && Protection.BannedTimes < 5)
                    {
                        Log.WriteDoss(Protection.IP + " has been unbanned!");
                        NetworkSocket.BannedIPs.Remove(Protection);
                        Protection.BannedRoutine = Protection.Connections = 0;
                    }
                    else
                    {
                        Protection.BannedRoutine++;
                    }
                }
            }
            catch
            {
            }
        }
        public static virtualProtection getProtectionByIP(string IP)
        {
            foreach (virtualProtection Protection in Collections)
            {
                if (Protection.IP == IP)
                {
                    return Protection;
                }
            }
            return null;
        }
        public virtualProtection(string _IP)
        {
            this.IP = _IP;
            this.Connections = 0;
            this.BannedRoutine = 0;
            this.BannedTimes = 0;
            Collections.Add(this);
        }
    }
}
