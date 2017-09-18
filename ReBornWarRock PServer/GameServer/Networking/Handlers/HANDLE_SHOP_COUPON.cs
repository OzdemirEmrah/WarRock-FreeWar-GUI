using System;
using ReBornWarRock_PServer.GameServer.Networking.Packets;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;
namespace ReBornWarRock_PServer.GameServer.Networking.Handlers
{
    class HANDLE_SHOP_COUPON : PacketHandler
    {
        public override void Handle(virtualUser User)
        {
            bool Found = false;
            string CouponCode = getBlock(0);
            int[] array = DB.runReadColumn("SELECT id FROM coupons", 0, null);
            string[] CouponArray = DB.runReadRow(string.Concat("SELECT id, coupon, code, days, amount, active, item, usedby, public, expiredate FROM coupons"));
            if ((int)CouponArray.Length <= 0)
            {
                User.send(new PACKET_SHOP_COUPON(PACKET_SHOP_COUPON.Subtype.WrongCoupon));
                return;
            }
            for (int I = 0; I < array.Length; I++)
            {
                string[] Coupon = DB.runReadRow("SELECT coupon FROM coupons WHERE id=" + array[I].ToString());
                if (Coupon[0] == CouponCode) Found = true;
            }
            if (Found == false)
            {
                User.send(new PACKET_SHOP_COUPON(PACKET_SHOP_COUPON.Subtype.WrongCoupon));
                return;
            }
            int Actived = Convert.ToInt32(CouponArray[5]);
                string[] UsedBy = CouponArray[7].Split(',');
                int Public = Convert.ToInt32(CouponArray[8]);

                DateTime dateTime = DateTime.ParseExact(CouponArray[9], "dd-MM-yyyy", null);
                string str = dateTime.ToString("yyMMdd");
                DateTime now = DateTime.Now;
                int num = Convert.ToInt32(string.Format("{0:yyMMdd}", now));
                if (UsedBy.Length > 1 && Actived == '1' && Public == '0')
                {
                    User.send(new PACKET_SHOP_COUPON(PACKET_SHOP_COUPON.Subtype.AlreadyUsedCouponByOther));
                    return;
                }
            virtualUser user = User;
                int UserID = user.UserID;
            for (int N = 0; N < UsedBy.Length; N++)
            {
                if (UsedBy[N] == User.UserID.ToString())
                {
                    User.send(new PACKET_SHOP_COUPON(PACKET_SHOP_COUPON.Subtype.AlreadyUsedCouponByHimself));
                    return;
                }
            }
            if (num >= Convert.ToInt32(str))
                {
                    User.send(new PACKET_SHOP_COUPON(PACKET_SHOP_COUPON.Subtype.CouponIsExpired));
                    return;
                }

                int InventorySlot = User.InventorySlots;
                if (InventorySlot == 0)
                {
                    User.send(new PACKET_SHOP_COUPON(PACKET_SHOP_COUPON.Subtype.InventoryFull));
                    return;
                }
                string str1 = CouponArray[2];
                User.AddOutBoxItem(str1, Convert.ToInt32(CouponArray[3]), Convert.ToInt32(CouponArray[4]));
                if (Public == 1) Actived = 1;
                DB.runQuery("UPDATE coupons SET active = "+ Actived +" ',' usedby = "+ user.UserID +""+","+" WHERE id = "+ CouponArray[9] +"");
                Log.AppendText(User.Nickname + " enter ingamepromocode: " + CouponArray[2]);
            
        }

    }
}