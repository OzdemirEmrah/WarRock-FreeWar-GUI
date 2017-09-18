using System;

namespace ReBornWarRock_PServer
{
    class ConfigServer
    {
        public static string SERVER_KEY_PASSWORD = "";
        public static string SERVER_NAME = "";
        public static string SERVER_IP = "";
        public static int SERVER_PORT = 5040;
        public static int SERVER_ID = 0;
        public static int ClientVersion = -1;
        public static int KillEvent = 0;
        public static int Debug = 0;
        public static double EXPDinar = 1;
        public static int Rates = 1;
        public static bool HomePageEnabled = true;
        public static int HomePagePort = 44555;
        public static int OnTakeFlag = 3; // Points gave for each flag
        public static int MaxFlags = 30; // Numbers of flags that give points, like if i took 34 flags, i get only points from 30
        public static double DinarRate = 1;
        public static double ExpRate = 1.0;
        public static bool CQC = true;
        public static bool BG = true;
        public static bool AI = true;
    }
}
