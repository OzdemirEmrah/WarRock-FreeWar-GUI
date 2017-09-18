using ReBornWarRock_PServer.GameServer.Networking.Packets;

using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;
using System;
namespace ReBornWarRock_PServer.GameServer.Networking.Handlers
{
    class HANDLE_ROOM_LIST : PacketHandler
    {
        public override void Handle(ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser User)
        {
            try
            {
                User.Page = Convert.ToInt32(getNextBlock());
                User.send(new PACKET_ROOM_LIST(User, User.Page));
            }
            catch { }
        }
    }
}
