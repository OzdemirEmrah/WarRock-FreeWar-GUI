using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_CLAN_WAR : Packet
    {
        public enum ClanCodes
        {
            AlreadyFinished = 0x153a,
            BadServer = 0x1539,
            CantStartWhileEqualPlayers = 0x153e,
            CantStartWhileNotAllReady = 0x153f,
            ClanWarFinised = 0x1541,
            ClanWarInitalized = 0x1540,
            ClanWarStarted = 0x153c,
            GameTerminated = 0x153d,
            currentRoomCreated = 0x153b
        }

        public PACKET_CLAN_WAR(ClanCodes Code)
        {
            newPacket(25344);
            addBlock(765);
            addBlock((int)Code);
        }
    }
}
