using System;
using System.Collections;
namespace ReBornWarRock_PServer.GameServer
{
    internal class virtualMapData
    {
        public int MapID;
        public string Name;
        public int Flags;
        public int flag1;
        public int flag2;
        public string VehicleString;
        public static ArrayList MapDatas = new ArrayList();

        public static void Load()
        {
            virtualMapData.MapDatas.Clear();
            int[] array = DB.runReadColumn("SELECT id FROM maps", 0, null);
            for (int i = 0; i < array.Length; i++)
            {
                try
                {
                    string[] array2 = DB.runReadRow("SELECT mapid, name, flags, defaultflags, vehicles FROM maps WHERE id=" + array[i].ToString());
                    int mapID = Convert.ToInt32(array2[0]);
                    string name = array2[1];
                    int flags = Convert.ToInt32(array2[2]);
                    string[] defaultflags = array2[3].Split(new char[]{'|'});
                    int flag1 = Convert.ToInt32(defaultflags[0]);
                    int nIU = Convert.ToInt32(defaultflags[1]);
                    string vehString = array2[4];
                    new virtualMapData(mapID, name, flags, flag1, nIU, vehString);
                }
                catch
                {
                }
            }
            Log.AppendText("Successfully loaded [" + array.Length + "] MapDatas");
        }
        public virtualMapData(int _MapID, string _Name, int _Flags, int _flag1, int _flag2, string _VehString)
        {
            this.MapID = _MapID;
            this.Name = _Name;
            this.Flags = _Flags;
            this.flag1 = _flag1;
            this.flag2 = _flag2;
            this.VehicleString = _VehString;
            virtualMapData.MapDatas.Add(this);
        }
        public static virtualMapData GetMapByID(int MapID)
        {
            foreach (virtualMapData Map in virtualMapData.MapDatas)
            {
                if (Map.MapID == MapID)
                {
                    return Map;
                }
            }
            return null;
        }
    }
}