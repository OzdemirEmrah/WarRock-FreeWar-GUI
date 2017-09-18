using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class SPACKET_LOGIN : Packet
    {
        public SPACKET_LOGIN()
        {
            setClient(false);
            newPacket(99990);
            addBlock(ConfigServer.SERVER_KEY_PASSWORD); // Password
            addBlock(0); // Nothing
            addBlock(ConfigServer.SERVER_NAME); // Server Name
            addBlock(ConfigServer.SERVER_IP); // IP

        }
    }
}
