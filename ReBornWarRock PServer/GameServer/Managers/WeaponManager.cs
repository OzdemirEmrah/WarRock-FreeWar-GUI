using System;
using System.Threading;
using System.Collections;
using System.Linq;
using System.Text;

namespace ReBornWarRock_PServer.GameServer
{
    public class ItemData
    {
        public ItemData(int dmg, bool buyable, string itname)
        {
            this.damage = dmg;
            this.buyable = buyable; // i want also read prices lol for make checks :')
            this.code = itname;
        }
        public string code;
        public int damage;
        public bool buyable;
    }
    public class WeaponManager
    {
        ~WeaponManager()
        {
            GC.Collect();
        }
        public static Hashtable Items = new Hashtable();
        public static Hashtable Weapons = new Hashtable();
         

        public static ItemData getItem(string Item)
        {
            ItemData Data = (ItemData)Items[Item];

            if (Data != null)
                return Data;

            return (ItemData)null;
        }
        public static Hashtable LoadItems(string itemsbinfilename)
        {
            Hashtable itemtable = new Hashtable(300);
            Hashtable hextable = new Hashtable(255);

            string[] hexy = { "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "0A", "0B", "0C", "0D", "0E", "0F", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "1A", "1B", "1C", "1D", "1E", "1F", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "2A", "2B", "2C", "2D", "2E", "2F", "30", "31", "32", "33", "34", "35", "36", "37", "38", "39", "3A", "3B", "3C", "3D", "3E", "3F", "40", "41", "42", "43", "44", "45", "46", "47", "48", "49", "4A", "4B", "4C", "4D", "4E", "4F", "50", "51", "52", "53", "54", "55", "56", "57", "58", "59", "5A", "5B", "5C", "5D", "5E", "5F", "60", "61", "62", "63", "64", "65", "66", "67", "68", "69", "6A", "6B", "6C", "6D", "6E", "6F", "70", "71", "72", "73", "74", "75", "76", "77", "78", "79", "7A", "7B", "7C", "7D", "7E", "7F", "80", "81", "82", "83", "84", "85", "86", "87", "88", "89", "8A", "8B", "8C", "8D", "8E", "8F", "90", "91", "92", "93", "94", "95", "96", "97", "98", "99", "9A", "9B", "9C", "9D", "9E", "9F", "A0", "A1", "A2", "A3", "A4", "A5", "A6", "A7", "A8", "A9", "AA", "AB", "AC", "AD", "AE", "AF", "B0", "B1", "B2", "B3", "B4", "B5", "B6", "B7", "B8", "B9", "BA", "BB", "BC", "BD", "BE", "BF", "C0", "C1", "C2", "C3", "C4", "C5", "C6", "C7", "C8", "C9", "CA", "CB", "CC", "CD", "CE", "CF", "D0", "D1", "D2", "D3", "D4", "D5", "D6", "D7", "D8", "D9", "DA", "DB", "DC", "DD", "DE", "DF", "E0", "E1", "E2", "E3", "E4", "E5", "E6", "E7", "E8", "E9", "EA", "EB", "EC", "ED", "EE", "EF", "F0", "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "FA", "FB", "FC", "FD", "FE", "FF" };
            for (int i = 0; i <= 255; i++)
                hextable.Add(hexy[i], (byte)(i ^ 0xD7));


            try
            {

                System.IO.StreamReader streamReader = new System.IO.StreamReader(itemsbinfilename);
                string plik = streamReader.ReadToEnd();
                streamReader.Close();

                int datalen = plik.Length / 2;
                byte[] decoded = new byte[datalen + 1];

                for (int i = 0; i < plik.Length - 1; i += 2)
                {
                    string hex2 = plik[i].ToString() + plik[i + 1].ToString();
                    byte x = (byte)hextable[hex2];
                    decoded[i / 2] = x;
                }

                plik = System.Text.Encoding.UTF8.GetString(decoded);

                int pos1 = plik.IndexOf("[ITEM DATA]");
                pos1 = plik.IndexOf("[WEAPON]", pos1);
                int epos = plik.IndexOf("[/WEAPON]", pos1);
                string codewe = "CODE                        =	";
                string buyabl = "BUYABLE                     =	";
                string powewe = "POWER                       =	";

                int counter = 0;
                while (true)
                {
                    int fndat = plik.IndexOf(codewe, pos1);

                    if (fndat < epos)
                    {
                        string code = plik.Substring(fndat + codewe.Length, 4);
                        pos1 = fndat + 1;
                        string babl = plik.Substring(plik.IndexOf(buyabl, pos1) + buyabl.Length, 4);

                        bool buyable = false;
                        if (babl == "TRUE")
                            buyable = true;

                        int power = Int32.Parse(plik.Substring(plik.IndexOf(powewe, pos1) + codewe.Length, 4));

                        itemtable.Add(code, new ItemData(power, buyable, code));
                        Weapons.Add(counter, new ItemData(power, buyable, code));
                        counter++;
                    }
                    else
                        break;

                };

                itemtable.Add("ED01", new ItemData(650, false, "ED01"));
                itemtable.Add("EA01", new ItemData(650, false, "EA01"));
                Log.AppendText("Loaded Damage for " + counter.ToString() + " Weapons!");

            }
            catch (Exception xd)
            {
                Log.AppendError("Error reading " + itemsbinfilename + " : " + xd.GetBaseException() + "=" + xd.Message + ":" + xd.StackTrace);
            }
            return itemtable;
        }
    }
}
