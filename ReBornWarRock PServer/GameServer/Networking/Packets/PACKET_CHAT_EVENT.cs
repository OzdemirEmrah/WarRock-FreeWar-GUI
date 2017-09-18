using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_CHAT_EVENT : Packet
    {
        public PACKET_CHAT_EVENT(ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser User, string ItemCode)
        {
            newPacket(30775);
            addBlock(1);
            addBlock(ItemCode);
            addBlock(0);
            addBlock(User.rebuildWeaponList());
        }
    }
}
