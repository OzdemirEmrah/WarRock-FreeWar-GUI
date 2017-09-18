using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_CHAT : Packet
    {
        public enum ChatType : int
        {
            Notice1 = 1,
            Notice2,
            Lobby_ToChannel,
            Room_ToAll,
            Room_ToTeam,
            Whisper,
            Lobby_ToAll = 8,
            RadioChat = 9,
            Clan = 10
        }

        public enum ErrorCodes : int
        {
            ErrorUser = 95040
        }

        public PACKET_CHAT(string Name, ChatType Type, string Message, long TargetID, string TargetName)
        {

            newPacket(29696);
            addBlock(1);
            addBlock(0);
            addBlock(Name);
            addBlock((int)Type);
            addBlock(TargetID);
            addBlock(TargetName);
            addBlock(Message);
        }

        public PACKET_CHAT(ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser Client, ChatType Type, string Message, long TargetID, string TargetName)
        {

            newPacket(29696);
            addBlock(1);
            addBlock(Client.SessionID);
            addBlock(Client.Nickname);
            addBlock((int)Type);
            addBlock(TargetID);
            addBlock(TargetName);
            addBlock(Message);
        }

        public PACKET_CHAT(ErrorCodes ErrCode, params object[] Params)
        {
            newPacket(29696);
            addBlock((int)ErrCode);
            for (int i = 0; i < Params.Length; i++)
            {
                addBlock(Params[i]);
            }
        }
    }
}
