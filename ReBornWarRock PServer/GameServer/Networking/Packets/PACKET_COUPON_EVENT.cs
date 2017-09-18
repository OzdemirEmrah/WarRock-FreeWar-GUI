using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_COUPON_EVENT : Packet
    {
        public PACKET_COUPON_EVENT(int TodayCoupon, int Coupons)
        {
            newPacket(25605);
            addBlock(1);
            addBlock(TodayCoupon);
            addBlock(Coupons);
            addBlock(0);
            addBlock(1);
        }

        public PACKET_COUPON_EVENT(ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser User)
        {
            newPacket(25605);
            addBlock(0);
            addBlock(User.TodayCoupon); // Today Coupons
            addBlock(User.Coupons); // Coupons
            addBlock(0);
            addBlock(0);
            addBlock("0-0-0");
        }
    }
}
