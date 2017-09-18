using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;
using System;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_PING : Packet
    {
        public PACKET_PING(virtualUser User)
        {
            User.LastTimeStamp = (long)Environment.TickCount;
            newPacket(25600);
            addBlock(5000);
            addBlock(User.Ping);
            addBlock(Structure.isEvent == true ? 175 : -1);
            addBlock(Structure.EventTime);
            addBlock(Structure.EXPBanner); //0 --> Neutral ~ 1 --> Exp/Dinar ~ 5 --> Random Hot Time Event ~ 6-- > Hot Clan War
            addBlock(Structure.EXPEvent);
            addBlock(Structure.DinarEvent);
            addBlock(User.PremiumTimeLeft()); // Premium Left in Seconds
        }
    }
}
