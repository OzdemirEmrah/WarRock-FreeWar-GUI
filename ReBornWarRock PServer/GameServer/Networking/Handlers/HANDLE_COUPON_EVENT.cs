using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;

namespace ReBornWarRock_PServer.GameServer.Networking.Handlers
{
    class HANDLE_COUPON_EVENT : PacketHandler
    {
        public override void Handle(virtualUser User)
        {
            User.send(new Packets.PACKET_COUPON_EVENT(User));
        }
    }
}