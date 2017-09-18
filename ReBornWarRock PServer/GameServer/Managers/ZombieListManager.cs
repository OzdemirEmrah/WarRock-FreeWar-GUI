using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ReBornWarRock_PServer.GameServer.Managers
{
    class ZombieListManager
    {
        public static int ToWaveCaller (int Wave)
        {
            int toWave;
            XmlDocument xmlDocument = new XmlDocument();
            string path = IO.workingDirectory + "/ZombieList.xml";
            
                xmlDocument.Load(IO.workingDirectory + "/ZombieList.xml");
            
            switch (Wave)
            {
                case 0: { toWave = int.Parse(xmlDocument.SelectSingleNode("ZombieList/Waves/One").InnerText); return toWave; }  // Wave 1
                case 1: { toWave = int.Parse(xmlDocument.SelectSingleNode("ZombieList/Waves/Two").InnerText); return toWave; }  // Wave 2
                case 2: { toWave = int.Parse(xmlDocument.SelectSingleNode("ZombieList/Waves/Three").InnerText); return toWave; }  // Wave 3
                case 3: { toWave = int.Parse(xmlDocument.SelectSingleNode("ZombieList/Waves/Four").InnerText); return toWave; }  // Wave 4
                case 4: { toWave = int.Parse(xmlDocument.SelectSingleNode("ZombieList/Waves/Five").InnerText); return toWave; }  // Wave 5
                case 5: { toWave = int.Parse(xmlDocument.SelectSingleNode("ZombieList/Waves/Six").InnerText); return toWave; }  // Wave 6
                case 6: { toWave = int.Parse(xmlDocument.SelectSingleNode("ZombieList/Waves/Seven").InnerText); return toWave; }  // Wave 7
                case 7: { toWave = int.Parse(xmlDocument.SelectSingleNode("ZombieList/Waves/Eight").InnerText); return toWave; }  // Wave 8
                case 8: { toWave = int.Parse(xmlDocument.SelectSingleNode("ZombieList/Waves/Nine").InnerText); return toWave; }  // Wave 9
                case 9: { toWave = int.Parse(xmlDocument.SelectSingleNode("ZombieList/Waves/Ten").InnerText); return toWave; }  // Wave 10
                case 10: { toWave = int.Parse(xmlDocument.SelectSingleNode("ZombieList/Waves/Eleven").InnerText); return toWave; }  // Wave 11
                case 11: { toWave = int.Parse(xmlDocument.SelectSingleNode("ZombieList/Waves/Twelve").InnerText); return toWave; }  // Wave 12
                case 12: { toWave = int.Parse(xmlDocument.SelectSingleNode("ZombieList/Waves/Theerteen").InnerText); return toWave; }  // Wave 13
                case 13: { toWave = int.Parse(xmlDocument.SelectSingleNode("ZombieList/Waves/Fourteen").InnerText); return toWave; }  // Wave 14
                case 14: { toWave = int.Parse(xmlDocument.SelectSingleNode("ZombieList/Waves/Fiveteen").InnerText); return toWave; }  // Wave 15
                case 15: { toWave = int.Parse(xmlDocument.SelectSingleNode("ZombieList/Waves/Sixteen").InnerText); return toWave; }  // Wave 16
                case 16: { toWave = int.Parse(xmlDocument.SelectSingleNode("ZombieList/Waves/Seventeen").InnerText); return toWave; }  // Wave 17
                case 17: { toWave = int.Parse(xmlDocument.SelectSingleNode("ZombieList/Waves/Eighteen").InnerText); return toWave; }  // Wave 18
                case 18: { toWave = int.Parse(xmlDocument.SelectSingleNode("ZombieList/Waves/Nineteen").InnerText); return toWave; }  // Wave 19
                case 19: { toWave = int.Parse(xmlDocument.SelectSingleNode("ZombieList/Waves/Twentyone").InnerText); return toWave; }  // Wave 20
                case 20: { toWave = int.Parse(xmlDocument.SelectSingleNode("ZombieList/Waves/Twentytwo").InnerText); return toWave; }  // Wave 21
            }
            return -1;
        }
        public static void ZombieListing(Virtual_Objects.Room.virtualRoom Room)
        {
            //Room.spa
        }
    }
}
