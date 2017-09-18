using ReBornWarRock_PServer.GameServer.Networking.Packets;
using System;

namespace ReBornWarRock_PServer.GameServer.Networking.Handlers
{
    class HANDLE_CHANNEL_SWITCH : PacketHandler
    {
        public override void Handle(ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser User)
        {
            int TargetChannel = Convert.ToInt32(getNextBlock());

            if (TargetChannel == 1 && ConfigServer.CQC)
            {
                User.Channel = TargetChannel;
                User.Page = 0;
                User.send(new PACKET_CHANGE_CHANNEL(User));
                User.send(new PACKET_ROOM_LIST(User, User.Page));
                return;
            }

            if (TargetChannel == 2 && ConfigServer.BG)
            {
                User.Channel = TargetChannel;
                User.Page = 0;
                User.send(new PACKET_CHANGE_CHANNEL(User));
                User.send(new PACKET_ROOM_LIST(User, User.Page));
                return;
            }

            if (TargetChannel == 3 && ConfigServer.AI)
            {
                User.Channel = TargetChannel;
                User.Page = 0;
                User.send(new PACKET_CHANGE_CHANNEL(User));
                User.send(new PACKET_ROOM_LIST(User, User.Page));
                return;
            }

            else if (TargetChannel == 1 && !ConfigServer.CQC && User.Rank > 4 || TargetChannel == 2 && !ConfigServer.BG && User.Rank > 4 || TargetChannel == 3 && !ConfigServer.AI && User.Rank > 4)
                {
                    User.Channel = TargetChannel;
                    User.Page = 0;
                    User.send(new PACKET_CHANGE_CHANNEL(User));
                    User.send(new PACKET_ROOM_LIST(User, User.Page));
                    return;
                }

            else
                {
                    User.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> This Channel is not avaible yet, but we're working on it!!", 999, "NULL"));
                    return;
                }
        }
    }
}