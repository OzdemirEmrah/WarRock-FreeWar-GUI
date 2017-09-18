using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;

namespace ReBornWarRock_PServer.GameServer.Virtual_Objects.User.Inventory
{
    public class InventoryItem
    {
        public string _Code;
        public long _StartTime;
        public int _Count;


        public InventoryItem(long StartTime, string Code, int Count)
        {
            _Code = Code;
            _StartTime = StartTime;
            _Count = Count;
        }
    }
}
