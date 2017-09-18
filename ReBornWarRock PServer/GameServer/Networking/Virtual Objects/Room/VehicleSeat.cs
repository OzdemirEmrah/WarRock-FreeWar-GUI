using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;

namespace ReBornWarRock_PServer.GameServer
{
    class VehicleSeat
    {
        public int ID;
        public virtualUser seatOwner = null;
        public int MainCT = -1;
        public int MainCTMag = -1;
        public int SubCT = -1;
        public int SubCTMag = -1;
        public string MainCTCode;
        public string SubCTCode;

        public bool TakeSeat(virtualUser usr)
        {
            if (seatOwner == null)
            {
                seatOwner = usr;
                return true;
            }
            return false;
        }

        public void LeaveSeat(virtualUser usr)
        {
            if (seatOwner.UserID == usr.UserID)
                seatOwner = null;
        }

        public VehicleSeat(int _ID, int _MainCT, int _MainCTMag, int _SubCT, int _SubCTMag, string _MainCTCode, string _SubCTCode)
        {
            ID = _ID;
            MainCT = _MainCT;
            MainCTMag = _MainCTMag;
            SubCT = _SubCT;
            SubCTMag = _SubCTMag;
            MainCTCode = _MainCTCode;
            SubCTCode = _SubCTCode;
        }

        public bool IsSeatCode(string code)
        {
            return (code.ToUpper() == MainCTCode.ToUpper() || code.ToUpper() == SubCTCode.ToUpper());
        }
    }
}
