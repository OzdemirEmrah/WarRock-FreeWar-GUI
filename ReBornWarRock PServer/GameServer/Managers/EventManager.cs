using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace ReBornWarRock_PServer.GameServer.Managers
{
    struct EventInfo
    {
        public int ID;
        public long Startdate;
        public long EventLength;

        public int Type;
        public long ItemExpireDate;
        public string WeaponCode;
        public int MinLevel;

        public EventInfo(int ID, long Startdate, long EventLength, int Type, long Itemlength, string WeaponCode, int MinLevel)
        {
            this.ID = ID;
            this.Startdate = Startdate;
            this.EventLength = EventLength;
            this.Type = Type;
            this.ItemExpireDate = Itemlength;
            this.WeaponCode = WeaponCode;
            this.MinLevel = MinLevel;
        }
    }
    class EventManager
    {
        ~EventManager()
        {
            GC.Collect();
        }
        private static ArrayList _Events = new ArrayList();
         
        public static void Load()
        {
            try
            {
                _Events.Clear();

                int[] EventIDs = DB.runReadColumn("SELECT id FROM events WHERE expired='0'", 0, null);
                for (int I = 0; I < EventIDs.Length; I++)
                {
                    string[] EventInfo = DB.runReadRow("SELECT type, itemlength, startdate, eventlength, weaponcode, minlevel, endtime FROM events WHERE id=" + EventIDs[I].ToString());

                    long ExpireDate = long.Parse(EventInfo[2]) + long.Parse(EventInfo[6]);

                    if (Structure.currTimeStamp < ExpireDate)
                    {
                        _Events.Add(new EventInfo(EventIDs[I], long.Parse(EventInfo[2]), long.Parse(EventInfo[3]), Convert.ToInt32(EventInfo[0]), long.Parse(EventInfo[1]), EventInfo[4].ToUpper(), Convert.ToInt32(EventInfo[5])));
                    }
                }

                Log.AppendText("Event manager loaded " + _Events.Count + " events in the  system!");
            }
            catch { }
        }

        public static ArrayList getEvents()
        {
            return _Events;
        }
    }
}
