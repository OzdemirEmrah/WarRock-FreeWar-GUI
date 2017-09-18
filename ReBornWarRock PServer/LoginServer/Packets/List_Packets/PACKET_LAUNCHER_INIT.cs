using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReBornWarRock_PServer.LoginServer.Packets.List_Packets
{
    class PACKET_LAUNCHER_INIT : Packet
    {
        public PACKET_LAUNCHER_INIT(string Nickname, int Dinar, int Cash, bool Status, int Premium, int Rank)
        {
            newPacket(8924);
            addBlock(1);
            addBlock(Nickname);
            addBlock(Dinar);
            addBlock(Cash);
            addBlock(Status ? 1 : 0);
            addBlock(Premium);
            addBlock(Rank);
        }
    }
}
