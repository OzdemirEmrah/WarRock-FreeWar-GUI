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
    class PACKET_ROOM_UDP_SPECTATE : Packet
    {
        public PACKET_ROOM_UDP_SPECTATE(ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser User, Virtual_Objects.Room.virtualRoom Room) // Leave UDP
        {
            newPacket(29953);
            addBlock(1);
            addBlock(0);
            addBlock(User.SpectatorID);
            addBlock(User.UserID);
            addBlock(User.SessionID);
            addBlock(User.Nickname);
            addBlock(999);
        }

        public PACKET_ROOM_UDP_SPECTATE(ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser User) // Join UDP
        {
            newPacket(29953);
            addBlock(1);
            addBlock(1);
            addBlock(User.SpectatorID);
            addBlock(User.UserID);
            addBlock(User.SessionID);
            addBlock("0");
            addBlock(999);
            addBlock(User.nIP);
            addBlock(User.nPort);
            addBlock(User.lIP);
            addBlock(User.lPort);
            addBlock(0);
        }
    }
}