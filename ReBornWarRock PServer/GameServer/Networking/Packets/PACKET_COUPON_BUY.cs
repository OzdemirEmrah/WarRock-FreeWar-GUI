using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_COUPON_BUY : Packet
    {
        public enum ErrorCode
        {
            NotEnoughCoupons = 1
        }
        public PACKET_COUPON_BUY(ErrorCode ErrCode)
        {
            newPacket(25606);
            addBlock((int)ErrCode);
        }
        public PACKET_COUPON_BUY(string WeaponCode, ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser User)
        {
            newPacket(25606);
            addBlock(0);
            addBlock(User.getSlots()); // Slot Enabled
            addBlock(User.rebuildWeaponList());
            addBlock(User.Dinar);
            addBlock(0);
            addBlock(User.Coupons);
        }
    }
}
