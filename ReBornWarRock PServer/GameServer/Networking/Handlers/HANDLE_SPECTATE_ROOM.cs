using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.Room;
using ReBornWarRock_PServer.GameServer.Managers;
using ReBornWarRock_PServer.GameServer.Networking.Packets;

namespace ReBornWarRock_PServer.GameServer.Networking.Handlers
{
    class HANDLE_SPECTATE_ROOM : PacketHandler
    {
        public override void Handle(virtualUser User)
        {
            if (User.Rank > 2)
            {   
                int EnterRoom = Convert.ToInt32(getNextBlock());
                int roomID = Convert.ToInt32(getNextBlock());
                if (EnterRoom == 1) // 1 == Join, 2 == Leave, 3 == UpdateGameData [30017]
                {
                    virtualRoom Room = RoomManager.getRoom(User.Channel, roomID);
                    if (Room != null)
                    {
                        if (User.isSpectating == true) return;
                        if (Room.addSpectator(User))
                        {
                            User.send(new PACKET_SPECTATE_ROOM(User, Room));
                            User.send(new PACKET_ROOM_UDP(User, Room.Players));
                            foreach (virtualUser RoomUser in Room.Players)
                            {
                                if (RoomUser.Equals(User) == false && RoomUser.isSpectating == false)
                                    RoomUser.send(new PACKET_ROOM_UDP_SPECTATE(User));
                            }
                        }
                        else
                        {
                            StringBuilder SpectatorList = new StringBuilder();
                            int Count = 0;
                            foreach (virtualUser Spectator in Room.Spectators)
                            {
                                SpectatorList.Append(Spectator.Nickname);
                                Count++;
                            }
                            SpectatorList.ToString().Remove(SpectatorList.ToString().Length - 1, 1);
                            User.send(new PACKET_CHAT("SPECTATE", PACKET_CHAT.ChatType.Room_ToAll, "SPECTATE >> There is no slot empty for this room!", 999, User.Nickname));
                        }
                    }
                }
                else
                    User.Room.removeSpectator(User);
            }
            else
                User.disconnect();
        }
    }
}