using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_COUPON : Packet
    {
        public PACKET_COUPON(string CouponCode)
        {
            newPacket(33024);
            addBlock(CouponCode);
        }
    }
}
