using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ReBornWarRock_PServer.GameServer.Networking.Packets;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;

namespace ReBornWarRock_PServer.GameServer.Networking.Handlers
{
    class HANDLE_COUPON_BUY : PacketHandler
    {
        public override void Handle(virtualUser User)
        {
            int ID = Convert.ToInt32(getBlock(0));
            int Days = 0;
            int CouponToRemove = 0;
            string WeaponToBuy = null;
            #region Calculate
            switch (ID)
            {
                case 0: WeaponToBuy = "DS01"; Days = 3; CouponToRemove = 5; break;
                case 1: WeaponToBuy = "DS10"; Days = 3; CouponToRemove = 10; break;
                case 2: WeaponToBuy = "DS03"; Days = 3; CouponToRemove = 10; break;
                case 3: WeaponToBuy = "DF15"; Days = 3; CouponToRemove = 20; break;
                case 4: WeaponToBuy = "DG40"; Days = 3; CouponToRemove = 25; break;
                case 5: WeaponToBuy = "DJ35"; Days = 3; CouponToRemove = 30; break;
                case 6: WeaponToBuy = "CC41"; Days = 1; CouponToRemove = 30; break;
                case 7: WeaponToBuy = "DG22"; Days = 3; CouponToRemove = 35; break;
                case 8: WeaponToBuy = "DC77"; Days = 7; CouponToRemove = 45; break;
                default: User.disconnect(); break;
            }
            #endregion
            if (User.Coupons >= CouponToRemove)
            {
                int InventorySlot = User.InventorySlots;
                if (InventorySlot > 0)
                {
                    User.Coupons -= CouponToRemove;
                    DB.runQuery("UPDATE users SET coupons='" + User.Coupons + "' WHERE id='" + User.UserID + "'");
                    if (WeaponToBuy == null) User.disconnect();
                    User.AddItem(WeaponToBuy, Days, 1);
                    User.send(new PACKET_COUPON_BUY(WeaponToBuy, User));
                    User.send(new PACKET_COUPON_EVENT(User));
                    User.reloadCash();
                }
                else
                {
                    User.send(new PACKET_ITEMSHOP(PACKET_ITEMSHOP.ErrorCodes.InventoryFull, "NULL"));
                }
            }
            else
            {
                User.send(new PACKET_COUPON_BUY(PACKET_COUPON_BUY.ErrorCode.NotEnoughCoupons));
            }
        }
    }
}