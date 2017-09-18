using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_CARE_PACKAGE : Packet
    {
        public PACKET_CARE_PACKAGE()
        {
            //30272 6 900 1 DC34 5 30 DC04 15 15 DG03 20 7 DC09 30 7 DB02 30 7 900 1 DF36 5 30 DF06 15 15 CE01 20 7 DK01 30 7 DF08 30 7 900 1 DC93 5 30 DC03 15 15 CE01 20 7 DF08 30 7 DB06 30 7 900 1 DG25 5 30 DG06 15 15 DB02 20 7 DK01 30 7 DC09 30 7 900 1 DF65 5 30 DF05 15 15 DO02 20 7 DB06 30 7 DC09 30 7 3000 0 DT01 5 30 DS01 15 15 DF03 20 7 DA09 30 7 DB04 30 7  
            newPacket(30272);
            int[] Count = DB.runReadColumn("SELECT id FROM carepackage", 0, null);
            addBlock(Count.Length); // HowMuch

            //TODO: Add weapon chance foreach item
             
            for (int I = 0; I < Count.Length; I++)
            {
                string[] itemData = DB.runReadRow("SELECT itemcode, price, method, itemdays, loseitem1, loseitemdays1, loseitem2, loseitemdays2, loseitem3, loseitemdays3, loseitem4, loseitemdays4 FROM carepackage WHERE id=" + Count[I].ToString());

                addBlock(itemData[1]); // Dinars
                addBlock(itemData[2]); // Dinar / G1
                addBlock(itemData[0]); // Arma
                addBlock(-1);
                addBlock(itemData[3]);
                //
                addBlock(itemData[4]);
                addBlock(-1);
                addBlock(itemData[5]);
                //
                addBlock(itemData[6]);
                addBlock(-1);
                addBlock(itemData[7]);
                //
                addBlock(itemData[8]);
                addBlock(-1);
                addBlock(itemData[9]);
                //
                addBlock(itemData[10]);
                addBlock(-1);
                addBlock(itemData[11]);
            }
        }
    }
}
