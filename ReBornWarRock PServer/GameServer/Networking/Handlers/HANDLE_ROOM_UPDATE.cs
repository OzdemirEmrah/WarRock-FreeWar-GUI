using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReBornWarRock_PServer.GameServer.Managers;

using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;

namespace ReBornWarRock_PServer.GameServer.Networking.Handlers
{
    class HANDLE_ROOM_UPDATE : PacketHandler
    {
        public override void Handle(virtualUser User)
        {
            int currentRoomID = Convert.ToInt32(getBlock(0));
            Virtual_Objects.Room.virtualRoom currentRoom = User.Room;
            if (currentRoom.RoomStatus == 1)
            {
                currentRoom.Name = getBlock(1);
                if (currentRoom.Name.StartsWith("E|") && User.Rank > 2 && currentRoom.RoomType == 3)
                    currentRoom.Name = currentRoom.Name.Substring(2);
                currentRoom.EnablePassword = Convert.ToInt32(getBlock(2));
                currentRoom.Password = getBlock(3);
                currentRoom.MaxPlayers = Convert.ToInt32(getBlock(6));
                currentRoom.ZombieDifficulty = Convert.ToInt32(getBlock(8));
                if (currentRoom.Mode == 0)
                    currentRoom.Rounds = Convert.ToInt32(getBlock(7));
                else
                    currentRoom.Rounds = Convert.ToInt32(getBlock(9));
                currentRoom.TimeLimit = Convert.ToInt32(getBlock(10));
                currentRoom.Ping = Convert.ToInt32(getBlock(11));
                currentRoom.MapID = Convert.ToInt32(getBlock(13));
                currentRoom.LevelLimit = Convert.ToInt32(getBlock(5));
                currentRoom.NewMode = Convert.ToInt32(getBlock(15));
                currentRoom.SubNewMode = Convert.ToInt32(getBlock(16));
                currentRoom.send(new Packets.PACKET_ROOM_UPDATE(User.Room));
            }
            foreach (virtualUser _User in UserManager.getUsersInChannel(currentRoom.Channel, false))
            {
                if (_User.Page == Math.Floor(Convert.ToDecimal(currentRoom.ID / 14)))
                {
                    _User.send(new Packets.PACKET_ROOMLIST_UPDATE(currentRoom));
                }
            }
        }
    }
}
