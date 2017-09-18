using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ReBornWarRock_PServer.LoginServer.Docs;
using ReBornWarRock_PServer.LoginServer.Packets.List_Packets;
using ReBornWarRock_PServer.LoginServer.Packets;
using ReBornWarRock_PServer.LoginServer.Virtual.Server;
using ReBornWarRock_PServer;

namespace LoginServer.Packets.List_Handle
{
    class SHANDLE_BAN_PLAYER : PacketHandler
    {
        public override void Handle(Server Server)
        {
            int BanID = int.Parse(getNextBlock());
            int UserID = int.Parse(getNextBlock());
            string IP = getNextBlock();
            long ExpireDate = long.Parse(getNextBlock());

            BanManager.add(BanID, UserID, IP, "");

            foreach (Server _Server in ServerManager.getServers())
            {
                _Server.send(new SPACKET_BAN_PLAYER(BanID, UserID, IP, ""));
            }

            Log.AppendText("Added Ban " + BanID + " to the Server Ban Manager");
        }
    }
}