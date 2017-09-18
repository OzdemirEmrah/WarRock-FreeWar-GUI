using ReBornWarRock_PServer.LoginServer.Virtual.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReBornWarRock_PServer.LoginServer.Packets.List_Packets
{
    class SPACKET_UPDATE_COUNT : Packet
    {
        public SPACKET_UPDATE_COUNT(Server Server)
        {
            newPacket(99991);
            addBlock(0);
            addBlock(Server.getCount());
        }
    }
}
