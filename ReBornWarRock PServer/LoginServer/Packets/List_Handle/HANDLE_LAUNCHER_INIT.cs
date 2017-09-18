using ReBornWarRock_PServer.GameServer;
using ReBornWarRock_PServer.LoginServer.Packets.List_Packets;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReBornWarRock_PServer.LoginServer.Packets.List_Handle
{
    class HANDLE_LAUNCHER_INIT : PacketHandler
    {
        public override void Handle(LoginServer.Virtual.User.User Connection)
        {
            int ClientVersion = int.Parse(getBlock(0));
            int LauncherVersion = int.Parse(getBlock(1));
            int Type = int.Parse(getBlock(2));
            int UserID = 0;
            if (Type == 1) //FreeWar Account Status
            {
                string Username = getBlock(3);
                string Password = getBlock(4);
                try
                {
                    UserID = int.Parse(MYSQL.runReadOnce("id", "SELECT * FROM users WHERE username='" + Username + "'").ToString());
                }
                catch { UserID = 0; }
                if (UserID > 0)
                {
                    MYSQL.runQuery("UPDATE users SET lastipaddress='" + Connection.IPAddress + "' WHERE id='" + UserID + "'");
                    DataTable dt = MYSQL.runRead("SELECT id, username, password, salt, nickname, dinar, cash, rank, premium, banned FROM users WHERE id=" + UserID.ToString());
                    DataRow row = dt.Rows[0];

                    string Salt = row["salt"].ToString();
                    string md5Password = Structure.convertToMD5(Structure.convertToMD5(Password) + Structure.convertToMD5(Salt));

                    if (row["password"].ToString() == md5Password)
                    {
                        string Nickname = row["nickname"].ToString();
                        int Dinar = int.Parse(row["dinar"].ToString());
                        int Cash = int.Parse(row["cash"].ToString());
                        int Premium = int.Parse(row["premium"].ToString());
                        int Rank = int.Parse(row["rank"].ToString());
                        bool Status = true;
                        Structure.LogFromLauncher.TryAdd(Connection.IPAddress.ToString(), Connection);
                            Connection.send(new PACKET_LAUNCHER_INIT(Nickname, Dinar, Cash, Status, Premium, Rank));
                    }
                }
            }
        }
    }
}
