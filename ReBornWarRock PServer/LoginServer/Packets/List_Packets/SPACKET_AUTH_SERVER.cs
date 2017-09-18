using ReBornWarRock_PServer.LoginServer.Packets;
using ReBornWarRock_PServer.LoginServer.Virtual.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoginServer.Packets.List_Packets
{
    class SPACKET_AUTH_SERVER : Packet
    {
        public SPACKET_AUTH_SERVER(Server Server)
        {
            newPacket(99990);
            addBlock(1);
            addBlock(Server.getID());
        }

        public SPACKET_AUTH_SERVER()
        {
            newPacket(99990);
            addBlock(0);
        }
    }
}