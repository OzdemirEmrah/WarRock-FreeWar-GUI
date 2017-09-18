using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class virtualPendingClanUsers
    {
        public int ID, EXP, ClanRank, ServerID;
        public string Nickname, ClanJoinDate;
        ~virtualPendingClanUsers()
        {
            GC.Collect();
        }
    }
    class PACKET_CLAN_PENDING_USERS : Packet
    {
        ArrayList sPendingUsers = new ArrayList();
        public void PendingUsers(int ClanID)
        {
            sPendingUsers.Clear();
            int[] clanIDs = DB.runReadColumn("SELECT userid FROM clans_invite WHERE clanid='" + ClanID + "'", 0, null);
            for (int I = 0; I < clanIDs.Length; I++)
            {
                string[] userData = DB.runReadRow("SELECT exp, nickname, clanrank, clanjoindate FROM users WHERE id='" + clanIDs[I].ToString() + "'");
                virtualPendingClanUsers User = new virtualPendingClanUsers();
                User.ID = clanIDs[I];
                User.EXP = Convert.ToInt32(userData[0]);
                User.Nickname = userData[1];
                User.ClanRank = Convert.ToInt32(userData[2]);
                User.ClanJoinDate = userData[3];
                User.ServerID = 36;
                sPendingUsers.Add(User);
            }
        }

        public PACKET_CLAN_PENDING_USERS()
        {
            newPacket(26384);
            addBlock(3);
            addBlock(1);
        }

        public PACKET_CLAN_PENDING_USERS(int Subtype, int ClanID)
        {
            newPacket(26384);
            addBlock(9);
            addBlock(Subtype);
            addBlock(0);
            addBlock(ClanID);
        }
        public PACKET_CLAN_PENDING_USERS(Virtual_Objects.User.virtualUser Player)
        {
            PendingUsers(Player.ClanID);
            //26384 5 1 6 15351850 1 9776577 ateyooftw 2013.04.22 2013.04.22 0 23338906 1 29609 Exothebest 2013.01.30 2013.01.30 0 23430589 1 266 abobbetteeee 2013.04.21 2013.04.21 0 23323522 2 1576021 ToXiiC 2013.01.27 2013.01.18 203 23346580 1 5437 Maist0 2013.01.30 2013.01.30 0 23412876 1 6964 NoCeilings 2013.04.05 2013.04.05 0 
            newPacket(26384);
            addBlock(5); // OPCode
            addBlock(1);
            addBlock(sPendingUsers.Count);
            foreach (virtualPendingClanUsers User in sPendingUsers)
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
