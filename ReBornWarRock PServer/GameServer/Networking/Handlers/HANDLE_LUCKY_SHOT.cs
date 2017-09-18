using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReBornWarRock_PServer.GameServer.Networking.Handlers
{
    class HANDLE_LUCKY_SHOT : PacketHandler
    {
        public override void Handle(ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser User)
        {
            User.reloadCash();
            User.send(new Packets.PACKET_LUCKY_SHOT());
        }
    }
}
