using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ReBornWarRock_PServer.GameServer.Networking.Packets;

namespace ReBornWarRock_PServer.GameServer.Networking.Handlers
{
    class HANDLE_COUPON_ACTIVE : PacketHandler
    {
        public override void Handle(ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser User)
        {
            //User.send(new HANDLE_SHOP_COUPON());
        }
    }
}
