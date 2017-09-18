using System;
using System.Collections;
using System.Linq;
using System.Text;

using System.Net;
using System.IO;

using ReBornWarRock_PServer.GameServer.Managers;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_ROOM_UDP : Packet
    {
        public PACKET_ROOM_UDP(virtualUser Sender, ArrayList Users)
        {
            //29952 7 3901 22 0 0 0 83 0 3 -260 Á©¾È -1 -1 NULL -1 -1 0 42 500 0 0 -1 7670 6465 87058 265628 -1 -1 ÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿ 
            newPacket(29952);
            addBlock(Users.Count);
            foreach (virtualUser Player in Users)
            {
                addBlock(Player.UserID);        // UserID
                addBlock(Player.SessionID);     // SessionID
                addBlock(Player.RoomSlot);      // currentRoom Slot
                addBlock(Player.isReady ? 1 : 0);       //currentRoom Ready State of Player(0 = not ready, 1 = ready)
                addBlock(Player.Room.getSide(Player));
                addBlock(Player.Weapon); // Weapon-ID
                addBlock(Player.Health); // ?
                addBlock(Player.Class); // ?
                //   addBlock(0); // Connection Kit ( on newer servers )
                addBlock(0); //DONT DO ANYTHING XDD
                addBlock(Player.Nickname);
                addBlock(Player.ClanID); // Clan ID
                addBlock(Player.ClanIconID); // Clan Icon
                addBlock(Player.ClanName); // Clan Name
                addBlock(Player.ClanRank); // Clan Rank (5 = Master, 2 = User)
                addBlock(1);        // 1? Send From Login
                addBlock(0);       // 0? Send From Login
                addBlock(0);   // 910 (Always)? Send From Login (910 G1, 410 NX , 300 KR , 100 PH, INVALID TW)
                addBlock(0);
                addBlock((Player.Room.SuperMaster && Player.RoomSlot == Player.Room.RoomMasterSlot) ? 0 : Player.Premium);// Premium Type
                addBlock(Player.PCItem);
                addBlock(Player.hasSmileBadge);
                addBlock(Player.Kills);         // Player Kills
                addBlock(Player.Deaths);        // Player Deaths
                addBlock(Player.lPort); // Remote Port?
                addBlock(Player.Exp); // 0 = Disgiuse Badge
                if (Player.currentVehicle == null) addBlock(-1);
                else addBlock(Player.currentVehicle.ID);
                if (Player.currentVehicle == null) addBlock(-1);
                else addBlock(Player.currentSeat.ID);
                addBlock(Player.ClassCode);
                if (Sender.nIP == Player.nIP && Sender.lIP == Player.lIP)
                    //addBlock(Player.IPToInt("127.0.0.1")); // Player it self, send dam 127.0.0.1 to fix bugs
                    addBlock(Player.IPToInt(Player.IPAddr));
                else if (Sender.nIP == Player.nIP && Sender.lIP != Player.lIP)
                    addBlock(Player.lIP);
                else
                    addBlock(Player.nIP);       // Remote IP (UDP Connection)
                addBlock(Player.nPort);     // Remote Port (UDP Connection)
                addBlock(Player.lIP);       // Local IP (From UDP)
                addBlock(Player.lPort);     // Local Port (From UDP)
                addBlock(0);
            }
        }
    }
}