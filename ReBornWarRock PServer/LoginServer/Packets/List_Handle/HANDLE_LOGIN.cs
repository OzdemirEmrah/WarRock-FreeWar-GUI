using System;
using System.Collections.Generic;
using System.Text;

using ReBornWarRock_PServer.LoginServer.Docs;
using ReBornWarRock_PServer.LoginServer.Packets.List_Packets;
using System.Data;
using ReBornWarRock_PServer.LoginServer.Virtual.User;

namespace ReBornWarRock_PServer.LoginServer.Packets.List_Handle
{
    class HANDLE_LOGIN : PacketHandler
    {
        private enum LoginState { Success = 1, UnknownUser, InvalidPassword, AlreadyLoggedIn, Banned, Unknown };

        public override void Handle(LoginServer.Virtual.User.User Connection)
        {
            DateTime current = DateTime.Now;
            int StartTime = int.Parse(String.Format("{0:yyMMddHH}", current));
            Connection.Username = getBlock(2).ToLower();
            int UserID = 0;
            string Password = getBlock(3);
            try
            {
                try
                {
                    UserID = int.Parse(MYSQL.runReadOnce("id", "SELECT * FROM users WHERE username='" + Connection.Username + "'").ToString());
                }
                catch { UserID = 0; }
                if (UserID > 0)
                {
                        MYSQL.runQuery("UPDATE users SET lastipaddress='" + Connection.IPAddress + "' WHERE id='" + UserID + "'");
                    DataTable dt = MYSQL.runRead("SELECT id, username, password, salt, online, nickname, rank, firstlogin, bantime, clanid, clanrank, banned, bantime FROM users WHERE id=" + UserID.ToString());
                    DataRow row = dt.Rows[0];

                    foreach (string UserLogged in Structure.LogFromLauncher.Keys)
                    {
                        if (UserLogged == Connection.IPAddress.ToString())
                        {
                            Connection.UserID = int.Parse(row["id"].ToString());
                            Connection.Username = row["username"].ToString();
                            Connection.Nickname = row["nickname"].ToString();
                            Connection.Rank = int.Parse(row["rank"].ToString());
                            Connection.ClanID = int.Parse(row["clanid"].ToString());
                            Connection.Banned = int.Parse(row["banned"].ToString());
                            Connection.ClanRank = int.Parse(row["clanrank"].ToString());
                            bool Online = (row["online"].ToString() == "1");
                            Connection.FirstLogin = (row["firstlogin"].ToString() == "0");
                            bool banned = (Connection.Rank == 0 || row["banned"].ToString() == "1");
                            string bantime = row["bantime"].ToString();
                            Connection.send(new PACKET_SERVER_LIST(Connection));
                        }

                    }

                    string Salt = row["salt"].ToString();
                    string md5Password = Structure.convertToMD5(Structure.convertToMD5(Password) + Structure.convertToMD5(Salt));                    

                    if (row["password"].ToString() == md5Password)
                    {
                        Connection.UserID = int.Parse(row["id"].ToString());
                        Connection.Username = row["username"].ToString();
                        Connection.Nickname = row["nickname"].ToString();
                        Connection.Rank = int.Parse(row["rank"].ToString());
                        Connection.ClanID = int.Parse(row["clanid"].ToString());
                        Connection.Banned = int.Parse(row["banned"].ToString());
                        Connection.ClanRank = int.Parse(row["clanrank"].ToString());
                        bool Online = (row["online"].ToString() == "1");
                        Connection.FirstLogin = (row["firstlogin"].ToString() == "0");
                        bool banned = (Connection.Rank == 0 || row["banned"].ToString() == "1");
                        string bantime = row["bantime"].ToString();

                        if (Connection.Rank > 0)
                        {
                            if (Online)
                            {
                                Connection.send(new PACKET_SERVER_LIST(PACKET_SERVER_LIST.errorCodes.AlreadyLoggedIn));
                                Log.AppendText("Player " + Connection.Username + ", logged in with ip: " + Connection.IPAddress + " but the user is already online.");
                            }
                            else
                            {
                                if (Connection.FirstLogin)
                                {
                                    Connection.UserID = UserID;
                                    Connection.send(new PACKET_SERVER_LIST(PACKET_SERVER_LIST.errorCodes.Nickname));
                                    Log.AppendText("Connection from " + Connection.IPAddress + " logged succesfull in as new username ( " + Connection.Username + " )");

                                }
                                else
                                {
                                    if (Connection.ClanID != -1)
                                    {
                                        DataTable mydt = MYSQL.runRead("SELECT clanname, iconid FROM clans WHERE id='" + Connection.ClanID + "'");
                                        if (mydt.Rows.Count > 0)
                                        {
                                            DataRow myrow = mydt.Rows[0];
                                            Connection.ClanName = myrow["clanname"].ToString();
                                            Connection.ClanIconID = long.Parse(myrow["iconid"].ToString());
                                        }
                                    }

                                    Connection.send(new PACKET_SERVER_LIST(Connection));
                                    Log.AppendText("Player " + Connection.Nickname + ", logged in!");
                                }
                                Structure._AcceptedLogins++;
                            }
                        }
                        else
                        {
                            if (int.Parse(bantime) > StartTime || Connection.Banned == 1)
                            {
                                Connection.send(new PACKET_SERVER_LIST(PACKET_SERVER_LIST.errorCodes.Banned));
                                Log.AppendError("Connection from " + Connection.IPAddress + " failed to login because the account " + Connection.Username + " is disabled/banned.");
                            }
                            else
                            {
                                DB.runQuery("UPDATE users SET rank='1', bantime='-1' WHERE id='" + Connection.UserID + "'");
                                Connection.send(new PACKET_SERVER_LIST(Connection));
                                Log.AppendText("Connection from " + Connection.IPAddress + " logged succesfull in as " + Connection.Nickname + ".");
                            }
                        }
                    }
                    else
                    {
                        Connection.send(new PACKET_SERVER_LIST(PACKET_SERVER_LIST.errorCodes.WrongPW));
                        Log.AppendError("Connection from " + Connection.IPAddress + " failed to login on the account " + Connection.Username + ".");
                    }
                }
                else
                {
                    UserID = -1;
                    Connection.send(new PACKET_SERVER_LIST(PACKET_SERVER_LIST.errorCodes.WrongUser));
                    Log.AppendError("Connection from " + Connection.IPAddress + " logged in with an invalid username: " + Connection.Username + ".");
                }
            }
            catch (Exception ex)
            {
                Connection.disconnect();
                Log.AppendError(ex.Message);
            }
        }
    }
}
