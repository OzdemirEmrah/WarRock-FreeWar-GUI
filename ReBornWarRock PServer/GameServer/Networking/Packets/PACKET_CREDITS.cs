using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_CREDITS : Packet
    {
        public PACKET_CREDITS(ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser User)
        {
            newPacket(30720);
            addBlock(1113);
            addBlock(1);
            addBlock(User.Cash);
        }

        public PACKET_CREDITS(ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser User, string Items)
        {
            newPacket(30720);
            addBlock(1118);
            addBlock(1);
            addBlock(User.Cash);
            addBlock(Items);
            addBlock(User.getSlots());
            addBlock(0);
            addBlock(User.Dinar);
        }
    }
}
