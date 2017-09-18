using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_ROOM_INVITE : Packet
    {
        public enum ErrorCodes
        {
            GenericError = 93020,
            IsPlaying = 93030,
            Invited = 93010
        }

        public PACKET_ROOM_INVITE(ErrorCodes ErrCode)
        {
            newPacket(29520);
            addBlock((int)ErrCode);
        }

        public PACKET_ROOM_INVITE(ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser User, string Message)
        {
            //1454817889 29520 1 0 -1 13949431 19 gn0m3x -1 -1 -1 -1 0 19 68066 0 0 LassunseineRundezusammenspielen.Kommrein 3 NULL 
            newPacket(29520);
            addBlock(1);
            addBlock(0);
            addBlock(-1);
            addBlock(User.UserID);
            addBlock(User.SessionID); // Ping ?!
            addBlock(User.Nickname);
            // Clan
            addBlock(User.ClanID);// Clan Icon
            addBlock(User.ClanIconID);
            addBlock(User.ClanName);// Clan Rank 
            addBlock(-1);// Clan Rank
            addBlock(User.Rank);// Clan Rank
            // End Clan
            addBlock(1);
            addBlock(0);
            addBlock(User.Exp);
            addBlock(0);
            addBlock(0);
            addBlock(-1);
            addBlock(Message);
            addBlock(User.Room.ID);
            addBlock(User.Room.Password);
        }
    }
}
