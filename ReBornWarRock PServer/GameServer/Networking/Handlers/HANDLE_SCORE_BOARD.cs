using System;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.Room;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;
using ReBornWarRock_PServer.GameServer.Networking.Packets;

namespace ReBornWarRock_PServer.GameServer.Networking.Handlers
{
    class HANDLE_SCORE_BOARD : PacketHandler
    {
        public override void Handle(virtualUser User)
        {
            virtualRoom Room = User.Room;
            if (User.Room != null)
            {
                if (User.Channel != 3)
                    User.send(new PACKET_SCORE_BOARD(User));
                else 
                    User.send(new PACKET_SCORE_BOARD_AI(User));
                
            }
        }
    }
}