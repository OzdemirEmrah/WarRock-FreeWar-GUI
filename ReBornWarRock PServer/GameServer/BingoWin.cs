using System;
using System.Threading;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using ReBornWarRock_PServer.GameServer.Networking.Packets;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.Room;
using ReBornWarRock_PServer.GameServer.Managers;
using ReBornWarRock_PServer.GameServer.Networking;


namespace ReBornWarRock_PServer.GameServer
{
    class BingoWin
    {
        ~BingoWin()
        {
            GC.Collect();
        }
        public static string Name = "";
        public static int now = -1;

        public static Dictionary<String, int> Bingo =
                new Dictionary<String, int>();

        public static void Extract()
        {
            int rand = new Random().Next(1, 2); ;
            int count = 0;
            foreach (virtualUser Play in UserManager.getAllUsers())
            {
                count++;
                if (Play.BingoNumber == rand)
                {
                    Name = Play.Nickname;
                    Win();
                    break;
                }
                else if (count == UserManager.UserCount)
                {
                    Lose();
                }
            }
        }
        static void Win()
        {
            UserManager.sendToServer(new Networking.Packets.PACKET_CHAT("Server", Networking.Packets.PACKET_CHAT.ChatType.Notice1, " NOTICE: " + "Bingo is winned by " + Name + "!", 100, "NULL"));
            Reset();
        }
        static void Lose()
        {
            UserManager.sendToServer(new Networking.Packets.PACKET_CHAT("Server", Networking.Packets.PACKET_CHAT.ChatType.Notice1, " NOTICE: " + "The Bingo Is Not Winned!", 100, "NULL"));
        }
        static void Reset()
        {
            foreach (virtualUser Play in UserManager.getAllUsers())
            {
                Play.BingoNumber = -1;
            }
        }
    }

}
