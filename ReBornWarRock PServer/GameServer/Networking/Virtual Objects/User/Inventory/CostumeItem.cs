using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReBornWarRock_PServer.GameServer.Virtual_Objects.User.Inventory
{
    public class CostumeItem
    {
        public string _Code;
        public long _StartTime;

        public CostumeItem(long StartTime, string Code)
        {
            _Code = Code;
            _StartTime = StartTime;
        }
    }
}
