using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_ROOM_UPDATE : Packet
    {
        public PACKET_ROOM_UPDATE(Virtual_Objects.Room.virtualRoom currentRoom)
        {
            newPacket(29201);
            addBlock(currentRoom.ID);
            addBlock(currentRoom.Name);
            addBlock(currentRoom.EnablePassword);
            addBlock(currentRoom.Password);
            addBlock(currentRoom.MaxPlayers);
            addBlock(currentRoom.Ping);
            addBlock(currentRoom.LevelLimit);
            addBlock(currentRoom.Rounds);
            addBlock(currentRoom.ZombieDifficulty);
            addBlock(currentRoom.Rounds);
            addBlock(currentRoom.TimeLimit);
            addBlock(currentRoom.Ping);
            addBlock(currentRoom.AutoStart);
            addBlock(currentRoom.MapID);
            addBlock(currentRoom.Mode);
        }
    }
}
