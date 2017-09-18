using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using ReBornWarRock_PServer.GameServer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace ReBornWarRock_PServer.GameServer.Managers
{
    internal class ItemManager
    {
        public static string Items = null;
        public static Dictionary<string, Item> CollectedItems = new Dictionary<string, Item>();
        public static StringBuilder Decryptions = new StringBuilder();
        public static ArrayList combobox = new ArrayList();
        public static string MD5 = null;
        public static int XOR = 0;


        private static string GetMD5HashFromFile(string fileName)
        {
            try
            {
                FileStream fileStream = new FileStream(fileName, FileMode.Open);
                byte[] hash = new MD5CryptoServiceProvider().ComputeHash((Stream)fileStream);
                fileStream.Close();
                StringBuilder stringBuilder = new StringBuilder();
                for (int index = 0; index < hash.Length; ++index)
                    stringBuilder.Append(hash[index].ToString("x2"));
                return (stringBuilder).ToString();
            }
            catch
            {
                return null;
            }
        }

        public static void LoadItems()
        {
            if (ItemManager.Items == null)
                return;
            int num = -1;
            try
            {
                ItemManager.CollectedItems.Clear();
                ItemManager.MD5 = ItemManager.GetMD5HashFromFile("items.bin");
                string[] strArray = ItemManager.Items.Split(new char[]
        {
          '\t',
          '\n',
          '\r'
        });
                for (int index = 0; index < strArray.Length; ++index)
                {
                    try
                    {
                        if (strArray[index] == "<BASIC_INFO>")
                        {
                            string Name = strArray[index + 10];
                            string str = strArray[index + 15];
                            if(str == "EA03")
                            {
                                //Use it for breakpoint
                            }

                            bool Buyable = !(strArray[index + 47] == "FALSE");
                            string Price = strArray[index + 62];
                            string Cash = strArray[index + 67];
                            int BuyType = 1;
                            int Level = 1;
                            int Damage = 0;
                            bool Premium = false;
                            string Surface = null;
                            string Personal = null;
                            if (strArray[index + 224].Contains("PERSONAL"))
                                Personal = strArray[index + 225];
                            if (strArray[index + 221].Contains("PERSONAL"))
                                Personal = strArray[index + 222];
                            if (strArray[index + 229].Contains("SURFACE"))
                                Surface = strArray[index + 230];
                            if (strArray[index + 226].Contains("SURFACE"))
                                Surface = strArray[index + 227];
                            if (strArray[index + 76].Contains("REQ_LVL"))
                                Level = Convert.ToInt32(strArray[index + 77]);
                            if (strArray[index + 51].Contains("BUYTYPE"))
                                BuyType = Convert.ToInt32(strArray[index + 52]);
                            if (strArray[index + 123].Contains("POWER"))
                                Damage = Convert.ToInt32(strArray[index + 124]);
                            if (strArray[index + 120].Contains("POWER"))
                                Damage = Convert.ToInt32(strArray[index + 121]);
                            if (strArray[index + 118].Contains("POWER"))
                                Damage = Convert.ToInt32(strArray[index + 119]);
                            if (strArray[index + 92].Contains("APPLY_TARGET"))
                                Premium = !(strArray[index + 93] == "-1");

                            bool flag = false;

                            if (str.StartsWith("D"))
                            {
                                flag = true;
                                ++num;
                            }
                            Item obj = new Item(flag ? num : 0, str, Name, Price, Cash, Damage, BuyType, Surface, Personal, Level, Premium, Buyable);
                            combobox.Add(Name);
                            ItemManager.CollectedItems.Add(str, obj);
                        }
                    }
                    catch
                    {
                        //Log.AppendError("Error While Load Items at line" + index);
                    }
                }
               //Log.AppendText("Successfully loaded [" + ItemManager.CollectedItems.Count + "] Items");
            }
            catch
            {
                //Log.AppendText("Error loading items");
            }
        }

        public static string getItemByName(string Name)
        {
            try
            {
                Item obj = Enumerable.First<Item>(Enumerable.Where<Item>((IEnumerable<Item>)ItemManager.CollectedItems.Values, (Func<Item, bool>)(i => i.Name == Name)));
                if (obj != null)
                    return obj.Code;
            }
            catch
            {
            }
            return null;
        }

        public static string GetItemCodeByID(int id)
        {
            var item = CollectedItems.Values.Where(i => i.ID == id).First();
            if (item != null)
            {
                return item.Code;
            }
            return null;
        }

        public static string getItemByID(int id)
        {
            try
            {
                Item obj = Enumerable.First<Item>(Enumerable.Where<Item>((IEnumerable<Item>)ItemManager.CollectedItems.Values, (Func<Item, bool>)(i => i.ID == id)));
                if (obj != null)
                    return obj.Code;
            }
            catch
            {
            }
            return null;
        }

        public static int getDamage(string Code, int Type = 2)
        {
            try
            {
                if (ItemManager.CollectedItems.ContainsKey(Code))
                    return ItemManager.CollectedItems[Code].Damage;
                else
                    return 0;
            }
            catch
            {
                return 0;
            }
        }

        public static int getVehDamage(string Code, int Type = 1)
        {
            try
            {
                if (!ItemManager.CollectedItems.ContainsKey(Code))
                    return 0;
                Item obj = ItemManager.CollectedItems[Code];
                if (obj.Surface == null)
                    return obj.Damage;
                int num = Convert.ToInt32(obj.Surface.Split(',')[Type]);
                return Convert.ToInt32(Math.Truncate((double)(obj.Damage * num / 100)).ToString());
            }
            catch
            {
                return 0;
            }
        }

        public static Item getItem(string Code)
        {
            Code = Code.ToUpper();
            if (ItemManager.CollectedItems.ContainsKey(Code))
                return ItemManager.CollectedItems[Code];
            else
                return (Item)null;
        }

        public static string DecryptBinRaw(byte[] Raw)
        {
            try
            {
                return DecryptBinRaw(System.Text.Encoding.UTF8.GetString(Raw));
            }
            catch (Exception ex) { return null; }
        }

        public static int GetHexVal(char hex)
        {
            return (int)(checked(hex - ((hex < ':') ? '0' : '7')));
        }

        public static int checkXor(string firstChar)
        {
            int num = 1;
            checked
            {
                int num3 = -1;
                while (true)
                {
                    bool flag = Operators.CompareString(Conversions.ToString(Strings.Chr(num ^ Conversions.ToInteger(firstChar))), "<", false) == 0;
                    if (flag)
                    {
                        break;
                    }
                    num++;
                    int arg_75_0 = num;
                    int num2 = 255;
                    if (arg_75_0 > num2)
                    {
                        return num3;
                    }
                }
                num3 = num;
                //MyProject.Forms.Form1.keyTxtbox.Text = Conversions.ToString(num3);
                //MessageBox.Show("Forced XOR: " + Conversions.ToString(num3), "BinEditor", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return num3;
            }
        }

        public static string DecryptBinRaw(string Raw)
        {
            try
            {
                bool flag = false;
                byte[] databuffer = Encoding.UTF8.GetBytes(Raw);

                string str = Encoding.Default.GetString(databuffer);
                XOR = checkXor(Conversions.ToString((byte)((GetHexVal(str[0]) << 4) + GetHexVal(str[1]))));

                byte[] binData = new byte[Raw.Length / 2];

                for (int i = 0; i < binData.Length; i++)
                {
                    binData[i] = (byte)((GetHexVal(str[i << 1]) << 4) + GetHexVal(str[(i << 1) + 1]));
                    Decryptions.Append(Strings.Chr(XOR ^ (int)binData[i]));
                }
                return Decryptions.ToString();
            }
            catch (Exception ex) { return null; }
        }

        public static string DecryptBinFile(string filename)
        {
            try
            {
                System.IO.StreamReader mReader = new System.IO.StreamReader(System.IO.File.Open(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read));
                string str = ItemManager.DecryptBinRaw(mReader.ReadToEnd());
                mReader.Close();
                FormCalling.frm11.searchableRichTextBox1.Text = Decryptions.ToString();
                return ItemManager.Items = str;
            }
            catch
            {
                Log.AppendText("Failed to decrypt " + filename);
                return null;
            }
        }
    }
}
