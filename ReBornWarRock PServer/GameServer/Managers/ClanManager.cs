using System;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;
using ReBornWarRock_PServer.GameServer.Networking.Packets;
using ReBornWarRock_PServer.GameServer.Managers;
using System.Collections;

namespace ReBornWarRock_PServer.GameServer
{
    public class virtualClan
    {
        ~virtualClan()
        {
            GC.Collect();
        }
        public int clanID = 0;
        public string clanName = null;
        public long clanIconID;
        public int maxUsers = 0;
        public int clanWarWin = 0;
        public int clanWarLose = 0;
        public int clanEXP = 0;
        public virtualClan(int ID, string Name, long iconID, int Max, int Win, int Lose, int EXP)
        {
            clanID = ID;
            clanName = Name;
            clanIconID = iconID;
            maxUsers = Max;
            clanWarWin = Win;
            clanWarLose = Lose;
            clanEXP = EXP;
        }
    }
    public class virtualClanWars
    {
        public string vsName, Score;
        public bool Won;
        public virtualClanWars(string _vsName, string _Score, int _Won)
        {
            vsName = _vsName;
            Score = _Score;
            if (_Won == 1)
                Won = true;
            else
                Won = false;
        }
    }
    class ClanManager
    {
        ~ClanManager()
        {
            GC.Collect();
        }
        public static ArrayList Clans = new ArrayList();
         
        public static void Load() // Load all clans
        {
            Clans.Clear(); // Clear previous table
            int[] clanID = DB.runReadColumn("SELECT id FROM clans", 0, null);
            for (int I = 0; I < clanID.Length; I++)
            {
                // For each clan in the table add's it in the arraylist
                string[] clanData = DB.runReadRow("SELECT clanname, maxusers, win, lose, exp, iconid FROM clans WHERE id=" + clanID[I].ToString());
                virtualClan Clan = new virtualClan(clanID[I], clanData[0], long.Parse(clanData[5]), Convert.ToInt32(clanData[1]), Convert.ToInt32(clanData[2]), Convert.ToInt32(clanData[3]), Convert.ToInt32(clanData[4]));
                Clans.Add(Clan);
                int[] Users = DB.runReadColumn("SELECT * FROM users WHERE clanid='" + clanID[I].ToString() + "'", 0, null);
                if (Users.Length <= 0)
                {
                    DB.runQuery("DELETE FROM clans WHERE id='" + clanID[I].ToString() + "'");
                    DB.runQuery("UPDATE users SET clanid='-1', clanrank='1' WHERE clanid='" + clanID[I].ToString() + "'");
                    Clans.Remove(Clan);
                }
            }
            // Finaly if we doesn't get error clans are loaded
            //Log.AppendText("Loaded " + Clans.Count + " clans!");
        }

        public static int getClanRank()
        {
            int RankID = 0;
            foreach (virtualClan Clan in Clans)
            {

            }
            return RankID;
        }

        public static string getMasterNickname(int ClanID)
        {
            string[] checkData = DB.runReadRow("SELECT nickname FROM users WHERE clanid='" + ClanID + "' AND clanrank='2'");
            return checkData[0];
        }

        public static int getClanIDByName(string Name)
        {
            string[] nDB = DB.runReadRow("SELECT id FROM clans WHERE clanname='" + Name + "'");
            if (nDB.Length > 0)
                return Convert.ToInt32(nDB[0]);
            return -1;
        }

        public static ArrayList getClanUsers(int ID)
        {
            ArrayList Users = new ArrayList();
            foreach (virtualUser User in UserManager.getAllUsers())
            {
                if (User.ClanID == ID)
                    Users.Add(User);
            }
            return Users;
        }

        public static void sendToClan(virtualUser User, Packet p)
        {
            foreach (virtualUser ServerUser in UserManager.getAllUsers())
            {
                if (ServerUser.ClanID == User.ClanID && ServerUser.ClanRank != -1) // Check for clan id
                {
                    ServerUser.send(p);
                }
            }
        }

        public static int getClanMembersCount(int ClanID)
        {
            // Get the count of clan members
            int[] ID = DB.runReadColumn("SELECT * FROM users WHERE clanid='" + ClanID + "' AND clanrank != '-1' AND clanrank != '9'", 0, null);
            return ID.Length;
        }

        public static int getClanMembersMaxCount(int ClanID)
        {
            // Get the max count of clan members
            string[] ID = DB.runReadRow("SELECT maxusers FROM clans WHERE id='" + ClanID + "'");
            return Convert.ToInt32(ID[0]);
        }

        public static virtualClan getClan(int ID)
        {
            // Get clan by the ID
            virtualClan TargetClan = null;
            foreach (virtualClan Clan in Clans)
            {
                if (Clan.clanID == ID)
                {
                    return TargetClan = Clan; // If the clan exists return it
                }
            }
            return null; // If no clan is found return null
        }

        public static void ChangeName(virtualUser User, string oldName, string newName)
        {
            string[] GetCheck = DB.runReadRow("SELECT * FROM clans WHERE clanname='" + newName + "'");
            if (GetCheck.Length == 0) // Prevent double clan name!
            {
                if (User.Cash - 5000 >= 0)
                {
                    if (User.ClanRank == 5) // Check if user is master
                    {
                        User.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Whisper, "SYSTEM >> You've changed clan name for 5,000 cash!", User.SessionID, User.Nickname));
                        foreach (virtualUser ClanUser in getClanUsers(User.ClanID))
                        {
                            ClanUser.ClanName = newName;
                            ClanUser.send(new PACKET_CHAT("ClanSystem", PACKET_CHAT.ChatType.Clan, "ClanSystem >> Clan name was changed from: " + oldName + " to: " + newName, ClanUser.ClanID, ClanUser.Nickname));
                        }
                        User.Cash -= 5000;
                        DB.runQuery("UPDATE users SET cash=cash-2500 WHERE id='" + User.UserID + "'");
                        DB.runQuery("UPDATE clans SET clanname='" + newName + "' WHERE id='" + User.ClanID + "'");
                    }
                    else
                    {
                        User.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Whisper, "SYSTEM >> You cannot change the clan name because you're not the master!", User.SessionID, User.Nickname));
                    }
                }
                else
                {
                    User.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Whisper, "SYSTEM >> Not enough cash!", User.SessionID, User.Nickname));
                }
            }
            else
            {
                User.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Whisper, "SYSTEM >> This clan name is already in use!", User.SessionID, User.Nickname));
            }
        }

        public static void AddClan(virtualUser User, string Name, string Description)
        {
            string[] GetCheck = DB.runReadRow("SELECT * FROM clans WHERE clanname='" + Name + "'");
            if (GetCheck.Length == 0)
            {
                //if (User.Rank < 1)
                //{
                //    User.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Whisper, "SYSTEM >> Cannot create the clan " + Name + " because is available only for donators!", User.SessionID, User.Nickname));
                //    return;
                //}
                if (User.ClanID == -1) // If user hasn't a clan
                {
                    string ActualTime = DateTime.Now.ToString("yyyy.MM.dd");

                    if (Description != "") DB.runQuery("INSERT INTO clans (clanjoindate, clanname, maxusers, win, lose, description, announcment, iconid) VALUES('" + ActualTime + "', '" + Name + "', '20', '0', '0', '" + Description + "', 'Insert here The Announcement!', '1001001')");

                    if (Description == "") DB.runQuery("INSERT INTO clans (clanjoindate, clanname, maxusers, win, lose, description, announcment, iconid) VALUES('" + ActualTime + "', '" + Name + "', '20', '0', '0', 'Insert Here The Introduction!', 'Insert here The Announcement!', '1001001')");

                    int ClanID = getClanIDByName(Name);
                    virtualClan Clan = new virtualClan(ClanID, Name, 1001001, 20, 0, 0, 0); // Add the new clan for live update
                    Clans.Add(Clan); // Add it in the arraylist
                    User.ClanID = ClanID; // Set clan id to the master
                    User.ClanName = Name; // Set clan name to the maste
                    User.ClanRank = 2; // Set clan rank as master
                    User.ClanIconID = 1001001;
                    User.Clan = getClan(ClanID);
                    User.Dinar -= 10000;
                    /* Execute query for the database */
                    DB.runQuery("UPDATE users SET dinar='" + User.Dinar + "', clanid='" + ClanID + "', clanrank='2', clanjoindate='" + ActualTime + "' WHERE id='" + User.UserID + "'");
                    // Finaly result
                    User.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Whisper, "SYSTEM >> Successfully created the clan (" + Name + ")!", User.SessionID, User.Nickname));
                    User.send(new PACKET_CLAN(Name, ClanID));
                }
                else // else if user has already a clan
                {
                    string Rank = (User.ClanRank == 2 ? "own" : "are in"); // Calculate if is a master or an normal clan member
                    User.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Whisper, "SYSTEM >> Cannot create the clan because you " + Rank + " a clan!", User.SessionID, User.Nickname));
                }
            }
            else
            {
                User.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Whisper, "SYSTEM >> Cannot create the clan because this clan name is already in use!", User.SessionID, User.Nickname));
            }
        }

        
        public static void RemoveClan(virtualUser User)
        {
            if (User.ClanID != -1) // If user has a clan
            {
                if (User.ClanRank == 2) // If user is the master
                {
                    /* Execute query for the database */
                    DB.runQuery("DELETE FROM clans WHERE clanname='" + User.ClanName + "'");
                    DB.runQuery("DELETE FROM clans_clanwars WHERE clanname1='" + User.ClanName + "' OR clanname2='" + User.ClanName + "'");
                    DB.runQuery("UPDATE users SET clanid='-1', clanrank='-1' WHERE clanid='" + User.ClanID + "'");
                    foreach (virtualUser Players in UserManager.getAllUsers())
                    {
                        if (Players.ClanID == User.ClanID)
                        {
                            // If user is in the same clan of master updates live!
                            Players.send(new PACKET_CHAT("ClanSystem", PACKET_CHAT.ChatType.Clan, "ClanSystem >> " + User.Nickname + " disbanded the clan :(", Players.ClanID, "NULL"));
                            Players.ClanID = -1;
                            Players.ClanRank = -1;
                            Players.ClanIconID = -1;
                        }
                    }
                    // Remove the clan
                    Clans.Remove(getClan(User.ClanID));
                    User.send(new PACKET_CLAN());
                    return;
                }
                else // If user has clan but isn't the master
                {
                    User.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Whisper, "SYSTEM >> Cannot delete the clan because you're not the master!", User.SessionID, User.Nickname));
                }
            }
        }

        public static void UpgradeClan(virtualUser User)
        {
            if (User.ClanID == -1)
            {
                User.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Whisper, "SYSTEM >> You doesn't own a clan!", User.SessionID, User.Nickname));
            }
            else if (User.ClanRank != 2)
            {
                User.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Whisper, "SYSTEM >> You're not the owner of the clan!", User.SessionID, User.Nickname));
            }
            else if (User.Cash - 10000 < 0)
            {
                User.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Whisper, "SYSTEM >> Not enough money!", User.SessionID, User.Nickname));
            }
            else if (getClan(User.ClanID).maxUsers >= 100)
            {
                User.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Whisper, "SYSTEM >> Your clan cannot be extended more!!", User.SessionID, User.Nickname));
            }
            else
            {
                int MaxUsers = getClan(User.ClanID).maxUsers;
                getClan(User.ClanID).maxUsers += 20;
                int NewMaxUsers = getClan(User.ClanID).maxUsers;
                foreach (virtualUser ClanUser in getClanUsers(User.ClanID))
                    ClanUser.send(new PACKET_CHAT("ClanSystem", PACKET_CHAT.ChatType.Clan, "ClanSystem >> " + User.Nickname + " has upgraded clan slots from: " + MaxUsers + " to: " + NewMaxUsers + ":)!", User.ClanID, "NULL"));
                DB.runQuery("UPDATE clans SET maxusers=maxusers+20 WHERE id='" + User.ClanID + "'");
            }
        }

        public static void SetMemberRank(virtualUser User, string Nickname, string Rank)
        {
            bool UserFound = true;
            // Set a user to leader / operator / member
            foreach (virtualUser Client in UserManager.getAllUsers())
            {
                if (Client.Nickname.ToLower().Equals(Nickname.ToLower()) || Client.Username.ToLower().Equals(Nickname.ToLower()))
                {
                    if (Client.UserID == User.UserID || User.ClanID == -1 && User.ClanRank != 2) return;
                    if (Client.ClanID != User.ClanID) // If user is the same
                    {
                        User.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Whisper, "SYSTEM >> " + Client.Nickname + " is not in your clan!", User.SessionID, User.Nickname));
                    }
                    else if (Client.ClanID == User.ClanID && User.ClanRank == 2) // If client has same clan of the user
                    {
                        if (Rank == "leutnant") // Check if there exist another leutnant
                        {
                            string[] CheckData = DB.runReadRow("SELECT nickname FROM users WHERE clanid='" + User.ClanID + "' AND clanrank='13'");
                            if (CheckData.Length > 0)
                            {
                                User.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Whisper, "SYSTEM >> " + CheckData[0] + " is leader of the clan, leaders cannot be more than one!", User.SessionID, User.Nickname));
                                return;
                            }
                        }
                        else if (Rank == "operator")
                        {
                            int[] operatorLength = DB.runReadColumn("SELECT nickname FROM users WHERE clanid='" + User.ClanID + "' AND clanrank='15'", 0, null);
                            if (operatorLength.Length >= 5)
                            {
                                User.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Whisper, "SYSTEM >> There are already 5 (max) operators!", User.SessionID, User.Nickname));
                                return;
                            }
                        }
                        /*Calculate rank*/
                        int ClanRank = 1;
                        if (Rank == "leutnant") ClanRank = 13;
                        else if (Rank == "operator") ClanRank = 15;
                        else if (Rank == "member") ClanRank = 1;
                        else
                        {
                            User.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Whisper, "SYSTEM >> Invalid rank! Please choose one of those: member, operator, leutnant!", User.SessionID, User.Nickname));
                            return;
                        }

                        if (Client.ClanRank == ClanRank)
                        {
                            User.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Whisper, "SYSTEM >> " + Client.Nickname + " has already this clan rank!", User.SessionID, User.Nickname));
                            return;
                        }

                        Rank = char.ToUpper(Rank[0]) + Rank.Substring(1);

                        foreach (virtualUser ClanUser in getClanUsers(User.ClanID))
                        {
                            ClanUser.send(new PACKET_CHAT("ClanSystem", PACKET_CHAT.ChatType.Clan, "ClanSystem >> " + Client.Nickname + " rank has been changed to " + Rank + " :)!", User.ClanID, "NULL"));
                        }
                        Client.ClanRank = ClanRank;
                        DB.runQuery("UPDATE users SET clanrank='" + ClanRank + "' WHERE id='" + Client.UserID + "'");
                        return;
                    }
                }
                else
                {
                    UserFound = false;
                }
            }
            if (UserFound == false)
            {
                User.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Whisper, "SYSTEM >> The user " + Nickname + " isn't online or doesn't exist!", User.SessionID, User.Nickname));
            }
        }

        public static void InviteMember(virtualUser User, string Nickname)
        {
            // Invite a member
            bool UserFound = false;
            foreach (virtualUser Client in UserManager.getAllUsers())
            {
                if (Client.Nickname.ToLower().Equals(Nickname.ToLower()) || Client.Username.ToLower().Equals(Nickname.ToLower()))
                {
                    if (Client.UserID == User.UserID) return;
                    if (Client.ClanID == User.ClanID) // If user is the same
                    {
                        User.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Whisper, "SYSTEM >> " + Client.Nickname + " is already in your clan!", User.SessionID, User.Nickname));
                    }
                    else if (User.ClanRank != 2) // If user isn't master
                    {
                        User.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Whisper, "SYSTEM >> You cannot invite a member because you're not clan master / leader / operator !", User.SessionID, User.Nickname));
                    }
                    else if (Client.ClanID != -1) // If user has a clan
                    {
                        User.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Whisper, "SYSTEM >> " + Client.Nickname + " has already a clan!", User.SessionID, User.Nickname));
                    }
                    else if (getClanMembersCount(User.ClanID) >= getClanMembersMaxCount(User.ClanID)) // If clan is full
                    {
                        User.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Whisper, "SYSTEM >> There is no free empty slot for the clan!", User.SessionID, User.Nickname));
                    }
                    else if (Client.InvitationBy != -1) // If user has already invited
                    {
                        User.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Whisper, "SYSTEM >> Someone has already invited this user!", User.SessionID, User.Nickname));
                    }
                    else // Else do nigga work
                    {
                        User.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Whisper, "SYSTEM >> Successfully sent invitation to: " + Client.Nickname + "!", User.SessionID, User.Nickname));
                        Client.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Whisper, "SYSTEM >> " + User.Nickname + " has invited you to join in the clan: " + User.ClanName + "! Write /accept to accept or /decline to decline", Client.SessionID, Client.Nickname));
                        Client.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Whisper, "SYSTEM >> Remember: if you relogin, the invite will be automaticaly declined.", Client.SessionID, Client.Nickname));
                        Client.InvitationBy = User.ClanID;
                    }
                    return;
                }
                else
                {
                    UserFound = false;
                }
            }
            if (UserFound == false)
            {
                User.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Whisper, "SYSTEM >> The user " + Nickname + " isn't online or doesn't exist!", User.SessionID, User.Nickname));
            }
        }

        public static void JoinClan(virtualUser User, int ClanID)
        {
            if (User.ClanID == -1 && ClanID != -1) // If user hasn't a clan
            {
                /* Execute query for the database */
                string ActualTime = DateTime.Now.ToString("yyyy.MM.dd");
                DB.runQuery("UPDATE users SET clanid='" + ClanID + "', clanrank='0', clanjoindate='" + ActualTime + "' WHERE id='" + User.UserID + "'");
                User.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Whisper, "SYSTEM >> Successfull joined in the clan: " + getClan(ClanID).clanName + "!!", User.SessionID, User.Nickname));
                User.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Whisper, "SYSTEM >> Please re-login for take the effects of clan!!", User.SessionID, User.Nickname));
                User.ClanID = ClanID;
                User.ClanRank = 0;
                foreach (virtualUser Players in getClanUsers(User.ClanID))
                {
                    Players.send(new PACKET_CHAT("ClanSystem", PACKET_CHAT.ChatType.Clan, "ClanSystem >> " + User.Nickname + " has joined the clan :)!", User.ClanID, "NULL"));
                }
            }
        }

        public static void KickClan(virtualUser User, string Nickname)
        {
            // Kick a member from the clan
            bool UserFound = false;
            if (User.ClanID != 1 && User.ClanRank == 5 && Nickname.ToLower() != User.Nickname.ToLower())
            {
                string[] selectedUser = DB.runReadRow("SELECT id, clanrank FROM users WHERE nickname='" + Nickname.ToLower() + "' AND clanid='" + User.ClanID + "'");
                if (selectedUser.Length > 0)
                {
                    if (selectedUser[1] == "5") return;
                    DB.runQuery("UPDATE users SET clanid='-1', clanrank='1' WHERE id='" + selectedUser[0] + "'");
                    foreach (virtualUser Players in getClanUsers(User.ClanID))
                    {
                        Players.send(new PACKET_CHAT("ClanSystem", PACKET_CHAT.ChatType.Clan, "ClanSystem >> " + Nickname + " got kicked from the clan by" + User.Nickname + " :/!", User.ClanID, "NULL"));
                    }
                    return;
                }
                else UserFound = false;
                if (UserFound == false)
                {
                    User.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Whisper, "SYSTEM >> The user " + Nickname + " is not in your clan or doesn't exist!", User.SessionID, User.Nickname));
                }
            }
        }
        public static void LeaveClan(virtualUser User)
        {
            if (User.ClanRank == 5)
            {
                User.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Whisper, "SYSTEM >> Cannot leave the clan because you're master, disband the clan if you want to leave!!", User.SessionID, User.Nickname));
            }
            else if (User.ClanID != -1)
            {
                DB.runQuery("UPDATE users SET clanid='-1', clanrank='1' WHERE id='" + User.UserID + "'");
                User.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Whisper, "SYSTEM >> Sucessfully left the clan " + User.ClanName + "!!", User.SessionID, User.Nickname));
                User.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Whisper, "SYSTEM >> Please re-login for take the effects!!", User.SessionID, User.Nickname));
                int SaveID = User.ClanID;
                User.ClanID = User.ClanRank = -1;
                foreach (virtualUser Players in getClanUsers(SaveID))
                {
                    Players.send(new PACKET_CHAT("ClanSystem", PACKET_CHAT.ChatType.Clan, "ClanSystem >> " + User.Nickname + " has left the clan :(!", SaveID, "NULL"));
                }
                if (getClanMembersCount(SaveID) <= 0)
                    DB.runQuery("DELETE FROM clans WHERE id='" + SaveID + "'");
            }
        }
    }
}