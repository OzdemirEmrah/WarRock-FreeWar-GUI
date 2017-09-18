using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ReBornWarRock_PServer.GameServer.Managers
{
    class WordManager
    {
        ~WordManager()
        {
            GC.Collect();
        }
        public static ArrayList WordFilter = new ArrayList();
        public static ArrayList WordReplace = new ArrayList();

        public static void Load()
        {     
                 
            int[] WordFilterIDs = DB.runReadColumn("SELECT id FROM wordfilter;", 0, null);

                for (int I = 0; I < WordFilterIDs.Length; I++)
                {
                string[] asd = DB.runReadRow("SELECT * FROM `wordfilter` WHERE id=" + WordFilterIDs[I]);
                    WordFilter.Add(asd[1]);
                    WordReplace.Add(asd[2]);
                }
            Log.AppendText("Loaded [" + WordFilterIDs.Length + "] Bad Words");
        }

        public static string GetBadWord(string smsg)
        {
            string[] splitted = smsg.Split(new char[] { '\u001d' });
            for (int i = 0; i < splitted.Length; i++)
            {
                for (int id = 0; id < WordFilter.Count; id++)
                {
                    string value = WordFilter[id] as string;
                    if (splitted[i] == value)
                    {
                        string value1 = WordReplace[id] as string;
                        splitted[i] = value1;
                    }
                }
            }
            return string.Join("\u001d", splitted);
        }
    }
}
