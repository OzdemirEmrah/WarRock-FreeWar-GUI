using System;
using System.Collections;
using System.Data;
using System.Text;

namespace ReBornWarRock_PServer.LoginServer.Docs
{
    #region BanData Struct
    struct BanData
    {
        private int _ID;
        private int _UserID;
        private String _IPAddr;
        private String _Hostname;

        public String Address { get { return _IPAddr; } }
        public String Hostname { get { return _Hostname; } }
        public int ID { get { return this._ID; } }
        public int UserID { get { return _UserID; } }

        public BanData(int iID, int iUserID, String ipAddr, String sHostname)
        {
            this._ID = iID;
            this._UserID = iUserID;
            this._IPAddr = ipAddr;
            this._Hostname = sHostname;
        }
    }
    #endregion

    class BanManager
    {
        private static ArrayList _BanList = new ArrayList();

        public static void load()
        {
            /*_BanList.Clear();
            DataTable IDs = DB.runRead("SELECT id FROM bans WHERE deleted='0' AND expiredate = -1 OR expiredate > " + Structure.UnixTimestamp);
            DataRow row = IDs.Rows[0];
            for (int I = 0; I < IDs.Rows.Count; I++)
            {
                DataTable QueryData = DB.runRead("SELECT userid, ipAddr, Hostname FROM bans WHERE id=" + IDs[I].ToString());
                DataRow rows = QueryData.Rows[0];
                _BanList.Add(new BanData(IDs[I], int.Parse(rows["userid"].ToString()), rows["ipaddr"].ToString(), rows["Hostname"].ToString()));
                //row["username"].ToString()
            }
            Message.WriteLine("Caricati " + _BanList.Count + " Profili Bannati");*/
        }

        public static void add(int ID, int UserID, String IPAdr, String Host)
        {
            _BanList.Add(new BanData(ID, UserID, IPAdr, Host));
        }

        public static void remove(int ID)
        {
            for (int I = 0; I < _BanList.Count; I++)
            {
                BanData Obj = (BanData)_BanList[I];
                if (Obj.ID == ID) { _BanList.Remove(I); break; }
            }
        }

        public static bool isBlocked(int ID)
        {
            foreach (BanData BanInfo in _BanList)
            {
                if (BanInfo.UserID == -1) continue;
                if (BanInfo.UserID == ID) return true;
            }
            return false;
        }

        public static bool isBanned(String Address, String Hostname)
        {
            foreach (BanData BanInfo in _BanList)
            {
                if (BanInfo.Address == Address || BanInfo.Hostname.ToLower() == Hostname.ToLower()) return true;
            }
            return false;
        }
    }
}
