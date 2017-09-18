using System;
using System.Collections;
using System.Linq;
using System.Text;

using ReBornWarRock_PServer.LoginServer.Docs;
using ReBornWarRock_PServer.LoginServer.Virtual;
using System.Data;
using ReBornWarRock_PServer.LoginServer.Virtual.User;
using System.Net;

namespace ReBornWarRock_PServer.LoginServer.Packets.List_Packets
{
    class PACKET_SERVER_LIST : Packet
    {
        public enum errorCodes : int
        {
            Nickname = 72000,
            WrongUser = 72010,
            WrongPW = 72020,
            AlreadyLoggedIn = 72030,
            Banned = 73050,
            BannedTime = 73020,
            AlreadyUsedNick = 74070
        }

        public static int ServerSlots(int slots)
        {
            int count = 0;
            int.TryParse((Math.Truncate((double)(2500 / slots)).ToString()), out count);
            return count;
        }

        public static int getOnlinePlayers(int srvid)
        {
            DataTable dt = MYSQL.runRead("SELECT * FROM users WHERE online='1' AND serverid='" + srvid + "'");
            return dt.Rows.Count;
        }

        public static string GetIpFromDNS(string hostname)
        {
            try
            {
                IPAddress[] ips;

                ips = Dns.GetHostAddresses(hostname);

                Console.WriteLine("GetHostAddresses({0}) returns:", hostname);

                foreach (IPAddress ip in ips)
                {
                    return ip.ToString();
                }
            }
            catch
            {
                return null;
            }
            return null;
        }

        public PACKET_SERVER_LIST(User User)
        {
            base.newPacket(4352); // PacketID
            base.addBlock(1); // Error Code - 1 = Success
            base.addBlock(User.UserID); // UserID
            base.addBlock(0); // ?
            base.addBlock(User.Username); // Username
            base.addBlock("???"); // Password
            base.addBlock(User.Nickname); // Nickname
            base.addBlock(1); // Unique ID 1 - Default = 1
            base.addBlock(0); // Unique ID 2 - Default = 0
            base.addBlock(User.SessionID); // SessionID
            base.addBlock(User.Rank > 2 ? 999 : 0); // AccessLevel
            base.addBlock("ReBornWarRock");
            if (User.ClanID != -1)
            {
                base.addBlock(User.ClanID);
                base.addBlock(User.ClanName);
                base.addBlock(User.ClanRank);
                base.addBlock(User.ClanIconID);
            }
            else
            {
                addBlock(-1);
                addBlock("NULL");
                Fill(-1, 2); // Clan blocks
            }
            Fill(0, 4);

            if(FormCalling.frm1.checkBox1.Checked)
            {
                addBlock(1);
                addBlock(1); // ServerID
                addBlock("FreeWar GUI"); // Server Name
                addBlock(FormCalling.frm1.label8.Text);
                addBlock(5340);
                addBlock(getOnlinePlayers(1) * ServerSlots(30)); // Server Playercount
                addBlock(0); // Flag - 0=All - 1=Adult 2=Clan(Hidden)
            }
            else
            {
                var servers = ServersInformations.collected.Values.Where(s => s.minrank <= User.Rank);
                addBlock(servers.Count());
                foreach (Servers s in servers)
                {
                    addBlock(s.id); // ServerID
                    addBlock(s.name); // Server Name
                    addBlock(s.ip);
                    addBlock(5340);
                    addBlock(getOnlinePlayers(s.id) * ServerSlots(s.slot)); // Server Playercount
                    addBlock(s.flag); // Flag - 0=All - 1=Adult 2=Clan(Hidden)
                }
            }
            /*base.addBlock("1"); // ServerID
            base.addBlock("VIP Server"); // Server Name
            base.addBlock("192.168.1.109"); // Server IP
            base.addBlock(5340);
            base.addBlock(getOnlinePlayers(1) * ServerSlots(30)); // Server Playercount
            base.addBlock(1); // Flag - 0=All - 1=Adult 2=Clan(Hidden)*/


            //
            //End Clan :(TODO)
            Fill(0, 2);
        }  

        public PACKET_SERVER_LIST(errorCodes errCode, params object[] oParams)
        {
            base.newPacket(4352);
            base.addBlock(((int)errCode));

            foreach (object tParam in oParams)
            {
                base.addBlock(tParam);
            }
        }
    }
}