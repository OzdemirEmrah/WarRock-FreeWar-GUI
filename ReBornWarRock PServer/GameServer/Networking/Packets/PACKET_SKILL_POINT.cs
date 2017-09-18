using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_SKILL_POINT : Packet
    {
        public PACKET_SKILL_POINT(ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser User)
        {
            newPacket(31945);
            addBlock(User.RoomSlot);
            addBlock(User.rSkillPoints);
        }
    }
}
