using System;

using ReBornWarRock_PServer.GameServer.Networking.Packets;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.User.Inventory;

namespace ReBornWarRock_PServer.GameServer.Networking.Handlers
{
    class HANDLE_CLAN : PacketHandler
    {
        public override void Handle(virtualUser User)
        {
            int Operation = Convert.ToInt32(getBlock(0));

            if (Operation == 0) // Check duplicate
            {
                string[] clanData = DB.runReadRow("SELECT * FROM clans WHERE clanname='" + getBlock(1) + "'");
                if (clanData.Length > 0 && getBlock(1).Length > 0)
                    User.send(new Packets.PACKET_CLAN(Packets.PACKET_CLAN.ErrorCodes.Exist, Operation));
                else
                    User.send(new Packets.PACKET_CLAN(Packets.PACKET_CLAN.ErrorCodes.NotExist, Operation));
            }
            else if (Operation == 1) // Create clan
            {
                ClanManager.AddClan(User, getBlock(1), getBlock(2));
                // ClanManager.AddClan(User, getBlock(1));
                return;
            }
            else if (Operation == 2) // Apply Clan
            {
                int ClanID = Convert.ToInt32(getBlock(1));
                string ActualTime = DateTime.Now.ToString("yyyy.MM.dd");
                string[] checkForAlreadyRequest = DB.runReadRow("SELECT * FROM clans_invite WHERE userid='" + User.UserID + "'");
                DB.runQuery("UPDATE users SET clanrank='9', clanid='" + ClanID + "' WHERE id='" + User.UserID + "'");
                if (checkForAlreadyRequest.Length > 0)
                    DB.runQuery("UPDATE clans_invite SET clanid='" + ClanID + "' WHERE userid='" + User.UserID + "'");
                else
                    DB.runQuery("INSERT INTO clans_invite (userid, clanid) VALUES ('" + User.UserID + "', '" + ClanID + "')");
                User.ClanID = ClanID;
                User.ClanRank = 9;
                User.Clan = ClanManager.getClan(ClanID);
                User.send(new PACKET_CLAN(-1));
                return;
            }
            else if (Operation == 3) // Leave Clan
            {
                if (User.ClanRank == 2)
                {
                    DB.runQuery("DELETE * FROM clans WHERE id='" + User.ClanID + "'");
                    DB.runQuery("UPDATE users SET clanid='-1', clanrank='-1' WHERE clanid='" + User.ClanID + "'");
                    foreach (virtualUser Player in Managers.UserManager.getAllUsers())
                        if (Player.ClanID == User.ClanID)
                            Player.ClanID = Player.ClanRank = -1;
                }
                DB.runQuery("UPDATE users SET clanid='-1', clanrank='-1' WHERE id='" + User.UserID + "'");
                User.ClanID = User.ClanRank = -1;
                User.send(new PACKET_CLAN_PENDING_USERS());
            }
            else if (Operation == 4) // Open 'My Clan'
            {
                if (User.ClanRank == -1 || User.ClanRank == 9)
                {
                    User.send(new PACKET_CHAT(User, PACKET_CHAT.ChatType.Whisper, "You need to be accepted before.", User.SessionID, User.Nickname));
                }
                User.send(new Packets.PACKET_CLAN(User));
                User.send(new Packets.PACKET_CLAN(Packets.PACKET_CLAN.ClanCodes.Open));
                return;
            }
            else if (Operation == 5) // Member Information's
            {
                int Page = Convert.ToInt32(getBlock(1));
                if (Page == 1)
                    User.send(new PACKET_CLAN_USERLIST(User));
                else if (Page == 2 && (User.ClanRank == 1 || User.ClanRank == 2))
                    User.send(new PACKET_CLAN_PENDING_USERS(User));
            }
            else if (Operation == 6) // Search clan
            {
                //Log.WriteDebug(string.Join(" ", getAllBlocks()));
                int Subtype = Convert.ToInt32(getBlock(1));
                switch (Subtype)
                {
                    case 0:
                        {
                            string ClanName = getBlock(2);
                            string[] ClanData = DB.runReadRow("SELECT * FROM clans WHERE clanname='" + ClanName + "'");
                            if (ClanData.Length > 0)
                                User.send(new PACKET_CLAN_USERLIST(User, ClanName));
                            else
                                User.send(new PACKET_CLAN(PACKET_CLAN.ErrorCodes.NotFound, Operation));
                            break;
                        }
                    default:
                        {
                            User.send(new PACKET_CLAN(PACKET_CLAN.ErrorCodes.NotFound, Operation));
                            break;
                        }
                }
            }
            else if (Operation == 7) // Clan Info
            {
                int ClanID = Convert.ToInt32(getBlock(1));
                User.send(new PACKET_CLAN_USERLIST(ClanID));
            }
            else if (Operation == 8) // Change announcment
            {
                string Check = (getBlock(1) == "1" ? "announcment" : "description");
                DB.runQuery("UPDATE clans SET " + Check + "='" + getBlock(2) + "' WHERE id='" + User.ClanID + "'");
            }
            else if (Operation == 9) // Accept/defuse clan join
            {
                int Connection = Convert.ToInt32(getBlock(1));
                int UserID = Convert.ToInt32(getBlock(2));
                if (Connection == 0) // Accept Join
                {
                    if (ClanManager.getClanMembersCount(User.ClanID) >= ClanManager.getClanMembersMaxCount(User.ClanID))
                    {
                        User.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Whisper, "SYSTEM >> No more slot available for the clan, please expand if is possible", User.SessionID, User.Nickname));
                        return;
                    }
                    string ActualTime = DateTime.Now.ToString("yyyy.MM.dd");
                    foreach (virtualUser Player in Managers.UserManager.getAllUsers())
                    {
                        if (Player.UserID == UserID)
                        {
                            Player.ClanRank = 0;
                        }
                    }
                    DB.runQuery("DELETE FROM clans_invite WHERE userid='" + UserID + "'");
                    DB.runQuery("UPDATE users SET clanid='" + User.ClanID + "', clanrank='0', clanjoindate='" + ActualTime + "' WHERE id='" + UserID + "'");
                }
                else if (Connection == 1) // Refuse Join
                {
                    DB.runQuery("DELETE FROM clans_invite WHERE userid='" + UserID + "'");
                    DB.runQuery("UPDATE users SET clanid='-1' WHERE id='" + UserID + "'");
                    string ActualTime = DateTime.Now.ToString("yyyy.MM.dd");
                    foreach (virtualUser Player in Managers.UserManager.getAllUsers())
                    {
                        if (Player.UserID == UserID)
                        {
                            Player.ClanRank = -1;
                            Player.ClanID = -1;
                            Player.ClanIconID = 0;
                        }
                    }
                }
                User.send(new PACKET_CLAN_PENDING_USERS(Connection, UserID));
            }
            else if (Operation == 10) // Promote / degrade / remove
            {
                int SubType = Convert.ToInt32(getBlock(1));
                int UserID = Convert.ToInt32(getBlock(2));
                if (SubType == 0)
                {
                    DB.runQuery("UPDATE users SET clanrank=clanrank+1 WHERE id='" + UserID + "'");
                }
                else if (SubType == 1)
                {
                    DB.runQuery("UPDATE users SET clanrank=clanrank-1 WHERE id='" + UserID + "'");
                }
                else if (SubType == 2)
                {
                    DB.runQuery("UPDATE users SET clanid='-1', clanrank='-1' WHERE id='" + UserID + "'");
                    foreach (virtualUser Player in Managers.UserManager.getAllUsers())
                        if (Player.UserID == UserID)
                            Player.ClanID = Player.ClanRank = -1;
                }
                User.send(new PACKET_CLAN_CHANGE(SubType, UserID));
            }
            else if (Operation == 11) // Promote master
            {
                //26384 11 23343041
                int UserID = Convert.ToInt32(getBlock(1));
                DB.runQuery("UPDATE users SET clanrank='0' WHERE id='" + User.UserID + "'");
                DB.runQuery("UPDATE users SET clanrank='2' WHERE id='" + UserID + "'");
                User.ClanRank = 0;
                foreach (virtualUser Player in Managers.UserManager.getAllUsers())
                    if (Player.UserID == UserID)
                        Player.ClanRank = 2;
                User.send(new PACKET_CLAN_CHANGE());
            }
            else if (Operation == 12) // Clan change nick
            {
                string newNick = getBlock(1);
                string[] CheckForLength = DB.runReadRow("SELECT * FROM clans WHERE clanname='" + newNick + "'");
                if (CheckForLength.Length > 0)
                {
                    User.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Whisper, "SYSTEM >> A clan has already this name, please choose another one", User.SessionID, User.Nickname));
                }
                else
                {
                    DB.runQuery("UPDATE clans SET clanname='" + newNick + "' WHERE id='" + User.ClanID + "'");
                    DB.runQuery("DELETE FROM inventory WHERE ownerid='" + User.UserID + "' AND itemcode='CB02'");
                    User.Inventory = new InventoryItem[105];
                    User.LoadItems();
                    User.send(new PACKET_CLAN_CHANGE(User, true));
                }
            }
            else if (Operation == 14) // Clan Mark Change
            {
                int iconID = Convert.ToInt32(getBlock(1));
                DB.runQuery("DELETE FROM inventory WHERE ownerid='" + User.UserID + "' AND itemcode='CB54'");
                User.Inventory = new InventoryItem[105];
                User.LoadItems();
                DB.runQuery("UPDATE clans SET iconid='" + iconID + "' WHERE id='" + User.ClanID + "'");
                foreach (virtualUser Player in Managers.UserManager.getAllUsers())
                    if (Player.ClanID == User.ClanID)
                        Player.ClanIconID = iconID;
                User.send(new PACKET_CLAN_CHANGE(User, false));
            }
            else if (Operation == 16) // Disband Clan
            {
                ClanManager.RemoveClan(User);
            }
            else
            {
                Log.AppendError("Unknown Operation for ClanSystem: " + Operation);
                Log.AppendError("Blocks: " + string.Join(" ", getAllBlocks()));
            }
        }
    }
}