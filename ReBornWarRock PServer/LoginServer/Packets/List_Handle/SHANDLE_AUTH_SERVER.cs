using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReBornWarRock_PServer.LoginServer.Docs;
using ReBornWarRock_PServer.LoginServer.Packets.List_Packets;
using ReBornWarRock_PServer.LoginServer.Packets;
using LoginServer.Packets.List_Packets;
using ReBornWarRock_PServer.LoginServer.Virtual.Server;
using ReBornWarRock_PServer;

namespace LoginServer.Packets.List_Handle
{
    class SHANDLE_AUTH_SERVER : PacketHandler
    {
        public override void Handle(Server Server)
        {
            string Password = getNextBlock();
            if (Password == Structure.ServerKey)
            {
                string nullObj = getNextBlock();
                string strName = getNextBlock();
                string strIP = getNextBlock();

                Server.setup(strName, strIP);

                if (ServerManager.addServer(Server))
                {
                    Server.send(new SPACKET_AUTH_SERVER(Server));
                }
            }
            else
            {
                Server.send(new SPACKET_AUTH_SERVER());
                Server.disconnect();
            }
        }
    }
}
