using System;
using System.Collections;
using ReBornWarRock_PServer.LoginServer.Packets;
using ReBornWarRock_PServer.LoginServer.Connection;
using ReBornWarRock_PServer;

namespace ReBornWarRock_PServer.LoginServer
{
    class Protection
    {
        private static ArrayList Collections;
        public string IP;
        public int Connections = 0;
        public int BannedRoutine = 0;
        public int BannedTimes = 0;

        static Protection()
        {
            Protection.Collections = new ArrayList();
        }

        public Protection(string _IP)
        {
            this.IP = _IP;
            this.Connections = 0;
            this.BannedRoutine = 0;
            this.BannedTimes = 0;
            Protection.Collections.Add(this);
        }

        public static void clearConnections()
        {
            NetworkSocket.BannedIPs.Clear();
            foreach (Protection collection in Protection.Collections)
            {
                int num = 0;
                int num1 = num;
                collection.BannedTimes = num;
                int num2 = num1;
                num1 = num2;
                collection.BannedRoutine = num2;
                collection.Connections = num1;
            }
        }


        public static Protection getProtectionByIP(string IP)
        {
            Protection _Protection;
            foreach (Protection collection in Protection.Collections)
            {
                if (collection.IP == IP)
                {
                    _Protection = collection;
                    return _Protection;
                }
            }
            _Protection = null;
            return _Protection;
        }

        public static void RunCheck()
        {
            bool flag;
            try
            {
                foreach (Protection bannedIP in NetworkSocket.BannedIPs)
                {
                    flag = (bannedIP.BannedRoutine < 30 ? true : bannedIP.BannedTimes >= 5);
                    if (flag)
                    {
                        Protection bannedRoutine = bannedIP;
                        bannedRoutine.BannedRoutine = bannedRoutine.BannedRoutine + 1;
                    }
                    else
                    {
                        NetworkSocket.BannedIPs.Remove(bannedIP);
                        int num = 0;
                        int num1 = num;
                        bannedIP.Connections = num;
                        bannedIP.BannedRoutine = num1;
                    }
                }
            }
            catch
            {
            }
        }
    }
}
