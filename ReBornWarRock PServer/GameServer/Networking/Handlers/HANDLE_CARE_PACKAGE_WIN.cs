using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ReBornWarRock_PServer.GameServer.Networking.Packets;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;

namespace ReBornWarRock_PServer.GameServer.Networking.Handlers
{
    class HANDLE_CARE_PACKAGE_WIN : PacketHandler
    {
        public override void Handle(GameServer.Virtual_Objects.User.virtualUser User)
        {
            int ItemID = Convert.ToInt32(getBlock(0));
            string[] theItem = DB.runReadRow("SELECT price, method, itemcode, itemdays FROM carepackage WHERE id='" + ItemID + "'");
            string ItemCode = theItem[2];
            int Days = Convert.ToInt32(theItem[3]);
            bool isDinar = (theItem[1] == "0" ? true : false);

            bool Win = false;

            int Rand = new Random().Next(0, 4);
            if (Rand == 0)
            {
                ItemCode = theItem[2];
                Win = true;
            }
            else
            {
                string[] loseItem = DB.runReadRow("SELECT loseitem" + Rand.ToString() + ", loseitemdays" + Rand.ToString() + " FROM carepackage WHERE id='" + ItemID + "'");
                ItemCode = loseItem[0];
                Days = Convert.ToInt32(loseItem[1]);
                Win = false;
            }

            int Price = Convert.ToInt32(theItem[0]);

            if (isDinar == true)
            {
                User.Dinar -= Price;
                DB.runQuery("UPDATE users SET dinar='" + User.Dinar + "' WHERE id='" + User.UserID + "'");
            }
            else
            {
                User.Cash -= Price;
                DB.runQuery("UPDATE users SET cash='" + User.Cash + "' WHERE id='" + User.UserID + "'");
            }

            User.AddItem(ItemCode, Days, 1);

            User.send(new PACKET_CARE_PACKAGE_WIN(User, ItemCode, Days, isDinar, Win));
        }
    }
}
