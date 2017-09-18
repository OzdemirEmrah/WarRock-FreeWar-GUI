using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_CHAT_COLOR : Packet
    {
        internal enum ChatType : int
        {
            Normal = 0,
            Clan = 2
        }

        public PACKET_CHAT_COLOR(string Message, ChatType type, System.Drawing.Color color)
        {
            newPacket(29697);
            addBlock(1);
            addBlock(Message);
            addBlock((int)type);
            addBlock((int)color.R);
            addBlock((int)color.G);
            addBlock((int)color.B);
        }
    }
}
