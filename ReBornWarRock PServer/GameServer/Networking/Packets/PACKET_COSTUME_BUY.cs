using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_COSTUME_BUY : Packet
    {
        public PACKET_COSTUME_BUY(ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser User, string WeaponString)
        {
            newPacket(30209);
            addBlock(1);
            addBlock(1118);
            addBlock(-1);
            addBlock(1);
            addBlock(0);
            addBlock(WeaponString);
            addBlock(0);
            addBlock(User.Dinar);
        }
    }
}
