using System;
using System.Collections.Generic;
using System.Linq;

namespace ReBornWarRock_PServer.GameServer
{
    class VehicleManager
    {
        public static List<VehicleManager> CollectedVehicles = new List<VehicleManager>();
        public string Code;
        public string Name;
        public int MaxHealth;
        public int RespawnTime;
        public string Seats;
        public bool isJoinable;

        public static void Load()
        {
            CollectedVehicles.Clear();
            int[] numArray = DB.runReadColumn("SELECT id FROM vehicles", 0, null);
            for (int key = 0; key < numArray.Length; ++key)
            {
                try
                {
                    string[] strArray = DB.runReadRow("SELECT code, name, maxhealth, respawntime, seats, joinable, map FROM vehicles WHERE id=" + numArray[key].ToString());
                    string Code = strArray[0];
                    if (Code == "EN01")
                    { 
                    string[] test = strArray;
                    }
                    string Name = strArray[1];
                    int MaxHealth = Convert.ToInt32(strArray[2]);
                    int RespawnTime = Convert.ToInt32(strArray[3]);
                    string Seats = null;
                    if (strArray[4] != "-1")
                        Seats = strArray[4];
                    bool isJoinable = strArray[5] == "1";
                    VehicleManager vehicleManager = new VehicleManager(Code, Name, MaxHealth, RespawnTime, Seats, isJoinable);
                    CollectedVehicles.Add(vehicleManager);
                }
                catch
                {
                }
            }
            Log.AppendText("Successfully loaded [" + numArray.Length + "] Vehicle Informations");
        }

        public static VehicleManager getVehicleInfoByCode(string Code)
        {
            foreach (VehicleManager VehicleInfo in CollectedVehicles)
            {
                if (VehicleInfo.Code == Code)
                { return VehicleInfo; }
            }
            return null;
        }

        public VehicleManager(string Code, string Name, int MaxHealth, int RespawnTime, string Seats, bool isJoinable)
        {
            this.Code = Code;
            this.Name = Name;
            this.MaxHealth = MaxHealth;
            this.RespawnTime = RespawnTime;
            this.Seats = Seats;
            this.isJoinable = isJoinable;
        }
    }
}






//}