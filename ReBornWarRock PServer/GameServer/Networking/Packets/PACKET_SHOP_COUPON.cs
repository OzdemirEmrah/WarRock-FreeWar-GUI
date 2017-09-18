using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.Room;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_SHOP_COUPON : Packet
    {
        public PACKET_SHOP_COUPON(PACKET_SHOP_COUPON.Subtype Subtype)
        {
            base.newPacket(30992);
            base.addBlock((int)Subtype);
        }

        public enum Subtype
        {
            InvalidCoupon = -12,
            unknowerrorcode = -4,
            InventoryFull = -9,
            CouponIsExpired = -0,
            //CouponIsExpired = -8,
            youmayonlyregisteronce = -7,
            Currentdinarisoverthelimit = -6,
            Idandcouponnumbersdonotmatch = -11,
            Couponnumberisincorrect = -2,
            AlreadyUsedCouponByHimself = -3,
            CouponCanNotBeUsedUnder7Days = -5,
            WrongCoupon = -0,
            AlreadyUsedCouponByOther = -1
        }
    }
}