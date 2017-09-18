using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading;
using ReBornWarRock_PServer.GameServer.Managers;
using ReBornWarRock_PServer.GameServer.Networking.Packets;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.Room;

using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;
namespace ReBornWarRock_PServer.GameServer.Networking.Handlers
{
    internal class HANDLE_GUN_SMITH : PacketHandler
    {
        public override void Handle(virtualUser User)
        {
            int num1 = int.Parse(this.getBlock(0));
            HANDLE_GUN_SMITH.Type type = (HANDLE_GUN_SMITH.Type)int.Parse(this.getBlock(1));
            if (type == HANDLE_GUN_SMITH.Type.Ticket && !User.hasItem("CZ75"))
                return;
            string[] GunSmithArray = DB.runReadRow("SELECT gameid, item, rare, required_items, lose_items, required_materials, cost FROM gunsmith WHERE gameid='" + num1 + "'");
            if (GunSmithArray.Length <= 0)
                return;
            string ItemCode = GunSmithArray[1];
            int num2 = int.Parse(GunSmithArray[6]);
            string str = GunSmithArray[2];
            int eaItems1 = User.getEAItems("CZ83");
            int eaItems2 = User.getEAItems("CZ84");
            int eaItems3 = User.getEAItems("CZ85");
            string[] strArray2 = (GunSmithArray[5]).ToString().Split(',');
            if (type == HANDLE_GUN_SMITH.Type.Ticket)
                num2 = 0;
            if (num2 == -1)
                return;
            int[] numArray = new int[strArray2.Length];
            for (int index = 0; index < numArray.Length; ++index)
                int.TryParse(strArray2[index], out numArray[index]);
            if (numArray[0] > eaItems3 || numArray[1] > eaItems2 || numArray[2] > eaItems1)
                return;
            string[] strArray3 = (GunSmithArray[3]).ToString().Split(',');
            if (!User.hasItem(strArray3[0]) || !User.hasItem(strArray3[1]) || !User.hasItem(strArray3[2]))
                return;
            User.DeleteItem(strArray3[0], 0);
            User.DeleteItem(strArray3[1], 0);
            User.DeleteItem(strArray3[2], 0);
            string[] strArray4 = (GunSmithArray[4]).ToString().Split(',');
            int num3 = new Random().Next(0, 50);
            if (type == HANDLE_GUN_SMITH.Type.Ticket)
                num3 += 10;
            int index1 = new Random().Next(0, strArray4.Length);
            int Days = 30;
            if (type == HANDLE_GUN_SMITH.Type.Dinar)
                User.Dinar -= num2;
            else
                User.DeleteItem("CZ75", 1);
            User.DeleteItem("CZ83", numArray[0]);
            User.DeleteItem("CZ84", numArray[1]);
            User.DeleteItem("CZ85", numArray[2]);
            int calcPerc = (type == Type.Dinar ? 10 : 25);
            if (num3 == 17)
            {
                ItemCode = str;
                User.send((Packet)new PACKET_GUN_SMITH(User, ItemCode, PACKET_GUN_SMITH.WonType.Rare));
            }
            else if (num3 > calcPerc)
            {
                User.send((Packet)new PACKET_GUN_SMITH(User, ItemCode, PACKET_GUN_SMITH.WonType.Normal));
            }
            else
            {
                ItemCode = strArray4[index1];
                Days = new Random().Next(1, 15);
                User.send((Packet)new PACKET_GUN_SMITH(User, ItemCode, PACKET_GUN_SMITH.WonType.Lose));
            }
            User.AddOutBoxItem(ItemCode, Days, 1);
        }

        private enum Type
        {
            Dinar,
            Ticket,
        }
    }
}
