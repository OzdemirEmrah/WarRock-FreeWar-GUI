using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class virtualClanUsers
    {
        public int ID, EXP, ClanRank, ServerID;
        public string Nickname, ClanJoinDate;
        ~virtualClanUsers()
        {
            GC.Collect();
        }
    }

    class PACKET_CLAN_USERLIST : Packet
    {
        ~PACKET_CLAN_USERLIST()
        {
            GC.Collect();
        }
        ArrayList Users = new ArrayList();
        public void ClanUsers(int ClanID)
        {
            Users.Clear();
            int[] clanIDs = DB.runReadColumn("SELECT id FROM users WHERE clanid='" + ClanID + "'", 0, null);
            for (int I = 0; I < clanIDs.Length; I++)
            {
                string[] userData = DB.runReadRow("SELECT exp, nickname, clanrank, clanjoindate FROM users WHERE id='" + clanIDs[I].ToString() + "'");
                virtualClanUsers User = new virtualClanUsers();
                User.ID = clanIDs[I];
                User.EXP = Convert.ToInt32(userData[0]);
                User.Nickname = userData[1];
                User.ClanRank = Convert.ToInt32(userData[2]);
                User.ClanJoinDate = userData[3];
                User.ServerID = 36;
                Users.Add(User);
            }
        }

        public PACKET_CLAN_USERLIST(int ClanID)
        {
            /*26384 7 19538 
            26384 7 1 19538 Tokkesik2ke ToXiiC 1589017 0 1 sdffdfds 1001001 */
            //26384 7 1 19538 MontanaWarRock iSkyKinqz 1589017 0 1 sdffdfds 1025003
            string[] ClanData = DB.runReadRow("SELECT clanname, announcment, description, iconid FROM clans WHERE id='" + ClanID + "'");
            string Nickname = ClanManager.getMasterNickname(ClanID);
            string[] EXP = DB.runReadRow("SELECT exp FROM users WHERE nickname='" + Nickname + "'");
            int Count = (ClanManager.getClanMembersMaxCount(ClanID) / 20) - 1;
            newPacket(26384);
            addBlock(7);
            addBlock(1);
            addBlock(ClanID);
            addBlock(ClanData[0]);
            addBlock(Nickname);
            addBlock(EXP[0]);
            addBlock(Count);
            addBlock(ClanManager.getClanMembersCount(ClanID));
            addBlock(ClanData[2]);
            addBlock(ClanData[3]);
        }

        public PACKET_CLAN_USERLIST(ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser User, string ClanName)
        {
            //26384 6 1 1 19538 ToXiiC 1589017 Tokkesik2ke 0 1 1001001
            string[] ClanData = DB.runReadRow("SELECT id, description, iconid FROM clans WHERE clanname='" + ClanName + "'");
            int ClanID = Convert.ToInt32(ClanData[0]);
            string Nickname = ClanManager.getMasterNickname(ClanID);
            string[] EXP = DB.runReadRow("SELECT exp FROM users WHERE nickname='" + Nickname + "'");
            int Count = (ClanManager.getClanMembersMaxCount(ClanID) / 20) - 1;
            newPacket(26384);
            addBlock(6); // OPCode
            addBlock(1);
            addBlock(1); // Clan count
            addBlock(ClanData[0]);
            addBlock(Nickname);
            addBlock(EXP[0]);
            addBlock(ClanName);
            addBlock(Count);
            addBlock(ClanManager.getClanMembersCount(ClanID));
            addBlock(ClanData[2]);
        }

        public PACKET_CLAN_USERLIST(ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser Player)
        {
            ClanUsers(Player.ClanID);
            int PendingUsers = 0;
            foreach (virtualClanUsers User in Users)
                if (User.ClanRank == 9)
                    PendingUsers++;
            //26384 5 1 6 15351850 1 9776577 ateyooftw 2013.04.22 2013.04.22 0 23338906 1 29609 Exothebest 2013.01.30 2013.01.30 0 23430589 1 266 abobbetteeee 2013.04.21 2013.04.21 0 23323522 2 1576021 ToXiiC 2013.01.27 2013.01.18 203 23346580 1 5437 Maist0 2013.01.30 2013.01.30 0 23412876 1 6964 NoCeilings 2013.04.05 2013.04.05 0 
            newPacket(26384);
            addBlock(5); // OPCode
            addBlock(1);
            addBlock(Users.Count - PendingUsers);
            foreach (virtualClanUsers User in Users)
            {
                if (User.ClanRank != 9)
                {
                    addBlock(User.ID);
                    addBlock(User.ClanRank);
                    addBlock(User.EXP);
                    addBlock(User.Nickname);
                    addBlock(User.ClanJoinDate);
                    addBlock(User.ClanJoinDate);
                    addBlock(User.ServerID);
                }
            }
        }
    }

    class PACKET_CLAN : Packet
    {
        public enum ErrorCodes
        {
            NotExist = 1,
            Exist = 62003,
            NotFound = 63001
        }

        public enum ClanCodes
        {
            CreateClan = 1,
            ApplyClan = 2,
            LeaveClan = 3,
            Open = 4,
            MemberList = 5,
            SearchClan = 6
        }

        public PACKET_CLAN(ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser User)
        {
            //26384 4 1 2 TheSattoHTeam iSkyKinqz 1576021 0 6 0 0 0 0 0 0 SaTToHTeam Sucatelaminchiahardomfg! 1001001 3 Identity d?H 0 BkBd d?H 0 KurtlarSofras? d?H 0
            //26384 4 1 2 Tokkesik2ke ToXiiC 1589017 0 1 1 0 0 0 0 0 sdffdfds dffddffd 1001001 0 
            string[] Clan = DB.runReadRow("SELECT clanname, announcment, description FROM clans WHERE id='" + User.ClanID + "'");
            int Count = (ClanManager.getClanMembersMaxCount(User.ClanID) / 20) - 1;
            string Nickname = ClanManager.getMasterNickname(User.ClanID);
            string[] EXP = DB.runReadRow("SELECT exp FROM users WHERE nickname='" + Nickname + "'");
            string[] isRealyNewRequest = DB.runReadRow("SELECT * FROM clans_invite WHERE clanid='" + User.ClanID + "'");
            //int[] Ranking = DB.runReadColumn("SELECT id FROM clans WHERE exp >= " + User.Clan.clanEXP, 0, null);
            int[] clanWarIDs = DB.runReadColumn("SELECT id FROM clans_clanwars WHERE clanname1='" + User.ClanName +"' OR clanname2='" + User.ClanName +"' ORDER BY date DESC", 0, null);
            int clanWarWonCount = 0;
            int clanRank = 0;
            int[] clanRanking = DB.runReadColumn("SELECT id FROM clans ORDER BY exp DESC", 0, null);
            for (int I = 0; I < clanRanking.Length; I++)
            {
                if (User.ClanID == clanRanking[I])
                {
                    clanRank = I + 1; // Because clanrank starts from 0
                }
            }
            newPacket(26384);
            addBlock(4); // OPCode
            addBlock(1);
            addBlock(User.ClanRank);
            addBlock(Clan[0]);
            addBlock(Nickname);
            addBlock(EXP[0]);
            addBlock(Count);
            addBlock(ClanManager.getClanMembersCount(User.ClanID));
            addBlock(isRealyNewRequest.Length > 0 ? 1 : 0);
            addBlock(User.Clan.clanWarWin); // Clan Win
            addBlock(User.Clan.clanWarLose); // Clan Lose
            addBlock(User.Clan.clanEXP); // Clan EXP
            addBlock(clanRank); // Clan Ranking
            addBlock(0);
            addBlock(Clan[2]);
            addBlock(Clan[1]);
            addBlock(User.ClanIconID);
            addBlock(clanWarIDs.Length);
            for (int I = 0; I < clanWarIDs.Length; I++)
            {
                string[] clanWarData = DB.runReadRow("SELECT clanname1, clanname2, score, clanwon FROM clans_clanwars WHERE id=" + clanWarIDs[I].ToString());
                string vsClan = "NULL";
                if (clanWarData[0] != User.ClanName)
                    vsClan = clanWarData[0];
                else if (clanWarData[1] != User.ClanName)
                        vsClan = clanWarData[1];
                if (clanWarWonCount < 3)
                {
                    addBlock(vsClan);
                    addBlock(clanWarData[2]);
                    addBlock(clanWarData[3] == User.ClanName ? 1 : 0);
                }
            }
            /*addBlock(ClanWars.Count);
            foreach (virtualClanWars ClanWar in ClanWars)
            {
                addBlock(ClanWar.vsName);
                addBlock(ClanWar.Score);
                addBlock(ClanWar.Won ? 1 : 0);
            }*/
            /*
            addBlock(WonClanWars.Count);
            foreach (WonClanWars ClanWar in ClanManager.getClan(User.ClanID).ClanWars)
            {
                addBlock(ClanWar.VersusClan);
                addBlock(ClanWar.Score);
                addBlock(0); // 0 = Lose / 1 = Win
            }
            */
        }

        public PACKET_CLAN(string Name, int clanID)
        {
            newPacket(26384);
            addBlock(1);
            addBlock(1);
            addBlock(clanID);
            addBlock(2);
            addBlock(Name);
        }

        public PACKET_CLAN(int ClanID = -1)
        {
            newPacket(26384);
            addBlock(2);
            addBlock(1);
        }

        public PACKET_CLAN()
        {
            newPacket(26384);
            addBlock(16);
            addBlock(1);
        }

        public PACKET_CLAN(ErrorCodes Err, int Operation)
        {
            newPacket(26384);
            addBlock(Operation);
            addBlock((int)Err);
        }

        public PACKET_CLAN(ClanCodes cC)
        {
            newPacket(26384);
            addBlock((int)cC);
        }
    }
}
