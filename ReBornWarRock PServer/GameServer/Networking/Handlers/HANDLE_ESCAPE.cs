using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading;
using ReBornWarRock_PServer.GameServer.Managers;
using ReBornWarRock_PServer.GameServer.Networking.Packets;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.Room;

using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;
namespace ReBornWarRock_PServer.GameServer.Networking.Handlers
{
    class HANDLE_ESCAPE_MODE : PacketHandler
    {
        public override void Handle(ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser User)
        {
            virtualRoom currentRoom = User.Room;
            int num1 = Convert.ToInt32(getBlock(0));
            int num2 = Convert.ToInt32(getBlock(1));
            int num3 = Convert.ToInt32(getBlock(2));
            int num4 = Convert.ToInt32(getBlock(3));
            int num5 = Convert.ToInt32(getBlock(4));
            int num6 = Convert.ToInt32(getBlock(5));
            int ThisTimeStamp = 0;

            if (num2 == 1 && num3 == 6)
            {
                User.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> Vai CAzzo ;)!", 999, "NULL"));
                User.send(new PACKET_3(num4, currentRoom));
            }
            if (num1 == 0 && num2 == 0 && num3 == 0 || num3 == 1 && num5 != 0)
                ThisTimeStamp = Structure.timestamp;
            {
                if (num5 == 0 && ThisTimeStamp >= Structure.timestamp)
                {
                    currentRoom.InHacking = true;
                    User.send(new PACKET_HACKING_ESCAPE(0, currentRoom.EscapeHack, 0));
                    currentRoom.EscapeHack += 10;
                    ThisTimeStamp += 2;
                    User.send(new PACKET_HACKING_ESCAPE(2, currentRoom.EscapeHack, 0));
                }
                else if (num5 == currentRoom.EscapeHack)
                {
                    if (currentRoom.HackingPause % 3 == 0) currentRoom.EscapeHack += 10;
                    User.send(new PACKET_HACKING_ESCAPE(2, currentRoom.EscapeHack, 0));
                }

            }
        }
    }
}
