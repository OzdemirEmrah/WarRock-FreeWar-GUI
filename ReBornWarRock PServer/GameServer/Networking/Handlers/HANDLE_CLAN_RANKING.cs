using System;
using System.Linq;

namespace ReBornWarRock_PServer.GameServer.Networking.Handlers
{
    class PACKET_CLAN_RANKING : Packet
    {
        public PACKET_CLAN_RANKING()
        {
            var clans = ClanManager.Clans.Cast<virtualClan>().Where(u => u != null).OrderByDescending(c => c.clanEXP).Take(30);
            newPacket(26464);
            addBlock(1);
            addBlock(DateTime.Now.Hour);
            addBlock(clans.Count());
            foreach (virtualClan c in clans)
            {
                if (c != null)
                {
                    int users = DB.runReadColumn("SELECT * FROM users WHERE clanid='" + c.clanID + "'", 0, null).Count();
                    addBlock(c.clanIconID);
                    addBlock(c.clanName);
                    addBlock(c.clanEXP);
                    addBlock(users);
                    addBlock(c.maxUsers);
                }
            }
        }
    }

    class HANDLE_CLAN_RANKING : PacketHandler
    {
        public override void Handle(Virtual_Objects.User.virtualUser User)
        {
            if (User.Room != null) return;

            User.send(new PACKET_CLAN_RANKING());
        }
    }
}
