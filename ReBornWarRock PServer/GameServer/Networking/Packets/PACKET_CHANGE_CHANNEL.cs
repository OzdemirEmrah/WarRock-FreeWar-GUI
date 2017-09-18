using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_CHANGE_CHANNEL : Packet
    {
        public PACKET_CHANGE_CHANNEL(ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser User)
        {
            newPacket(28673);
            addBlock(1);
            addBlock(User.Channel);
            //User.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.currentRoom_ToAll, "SYSTEM >> Welcome on WarRocK Beta!", 999, "NULL"));
        }
    }
}
