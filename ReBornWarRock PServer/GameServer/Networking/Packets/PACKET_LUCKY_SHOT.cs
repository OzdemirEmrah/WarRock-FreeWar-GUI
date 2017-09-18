using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_LUCKY_SHOT : Packet
    {
        public enum ErrorCode
        {
            NotEnoughDinar = 97040
        }
        public PACKET_LUCKY_SHOT(string Item, int Amount)
        {
            newPacket(30256);
            addBlock(Item);
            addBlock(Amount);
        }
        public PACKET_LUCKY_SHOT()
        {
            newPacket(30257);
            int[] checkCostumeIDs = DB.runReadColumn("SELECT id FROM luckyshot", 0, null);
            addBlock(checkCostumeIDs.Length); // HowMuch
            for (int I = 0; I < checkCostumeIDs.Length; I++)
            {
                string[] itemData = DB.runReadRow("SELECT itemcode, amount, price, method FROM luckyshot WHERE id=" + checkCostumeIDs[I].ToString());
                addBlock(Convert.ToInt32(itemData[3])); // Dinar / G1
                addBlock(itemData[0]); // Arma
                addBlock(Convert.ToInt32(itemData[1])); // Dinars
                addBlock(Convert.ToInt32(itemData[2])); // Quantity
            }
        }
        public PACKET_LUCKY_SHOT(ErrorCode ErrCode)
        {
            newPacket(30258);
            addBlock(-1);
            addBlock((int)ErrCode);
        }
        public PACKET_LUCKY_SHOT(ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser User, int Bets, string Item, int WinType, bool Dinar)
        {
            // 736644874 30258 1 0 0 DF05 3 DF05-1-0-12050509-0-0-0-0-0,DB12-1-0-12042717-0-0-0-0-0,DG07-1-0-12042717-0-0-0-0-0,DB02-1-0-12051313-0-0-0-0-0,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^ 277243 F,F,F,F
            //1077463421 30258 1 1 0 DA09 7 DC64-3-0-22020609-0-0-0-0-0-9999-9999,D901-3-0-12071114-0-0-0-0-0-9999-9999,DA09-1-1-12050809-0-0-0-0-0,DJ03-1-1-12050413-0-0-0-0-0,DS01-3-0-22013014-0-0-0-0-0-9999-9999,DO01-3-0-22013014-0-0-0-0-0-9999-9999,DG01-1-3-12051506-0-0-0-0-0,D902-3-0-12091217-0-0-0-0-0-9999-9999,DU02-3-3-12050214-0-0-0-0-0-0-0,DF05-1-3-12051912-0-0-0-0-0,^,DV01-3-0-22043014-0-0-0-0-0-9999-9999,DF04-3-0-22030914-0-0-0-0-0-9999-9999,DC31-3-0-22021412-0-0-0-0-0-9999-9999,^,CA04-3-0-16061015-0-0-0-0-0-9999-9999,D501-3-0-13022615-0-0-0-0-0-9999-9999,D701-3-0-13032815-0-0-0-0-0-9999-9999,^,^,DU01-3-0-22031115-0-0-0-0-0-9999-9999,CC05-3-0-12051214-0-0-0-0-0-9999-9999,DG40-3-0-12050818-0-0-0-0-0-9999-9999,D602-3-0-13070218-0-0-0-0-0-9999-9999,CD01-3-0-12051214-0-0-0-0-0-9999-9999,CD02-3-0-12051214-0-0-0-0-0-9999-9999,^,^,^,^ 101033 T,T,F,T 
            // Build Inventory //
            newPacket(30258);
            addBlock(1);
            addBlock(WinType);
            addBlock(Dinar ? 0 : 1);
            addBlock(Item);
            addBlock(Bets);
            addBlock(User.rebuildWeaponList());
            if (Dinar)
                addBlock(User.Dinar);
            else
                addBlock(User.Cash);
            addBlock(User.getSlots()); //Slots Enable
        }
    }
}
