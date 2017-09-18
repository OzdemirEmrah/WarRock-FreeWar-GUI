using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using ReBornWarRock_PServer.GameServer.Managers;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.Room;
using ReBornWarRock_PServer.GameServer.Networking.Packets;

namespace ReBornWarRock_PServer.GameServer.Networking.Handlers
{
    class HANDLE_QUICK_JOIN : PacketHandler
    {
        public override void Handle(ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser User)
        {
            try
            {
                ArrayList Rooms = RoomManager.getRoomsInChannel(User.Channel, new Random().Next(0, RoomManager.RoomToPageCount(User.Channel)));
                foreach (virtualRoom Room in Rooms)
                {
                    if (User.Room != null) return;
                    if (Room.Players.Count >= Room.MaxPlayers)
                        return;
                    if (Room.EnablePassword == 1)
                        return;
                    if (Room.UserLimit == true)
                        return;
                    if (Room.LevelLimit > 0)
                        return;
                    if (Room.RoomType == 1)
                        return;

                    int Side = Convert.ToInt32(getBlock(2));
                    if (Room.joinUser(User, Side) == true)
                    {
                        ArrayList tempPlayers = new ArrayList();
                        User.isReady = false;
                        User.isSpawned = false;
                        User.Health = 0;
                        User.BackedToRoom = false;
                        foreach (virtualUser RoomUser in Room.Players)
                        {
                            if (RoomUser.Equals(User) == false)
                                tempPlayers.Add(RoomUser);
                        }
                        User.send(new PACKET_ROOM_UDP(User, tempPlayers));

                        foreach (virtualUser tempSpectator in Room.Spectators)
                            User.send(new PACKET_ROOM_UDP_SPECTATE(tempSpectator));

                        User.send(new PACKET_JOIN_ROOM(User, Room));

                        tempPlayers.Clear();
                        tempPlayers.Add(User);

                        foreach (virtualUser RoomUser in Room.Players)
                            RoomUser.send(new PACKET_ROOM_UDP(RoomUser, tempPlayers));

                        foreach (virtualUser SpectatorUser in Room.Spectators)
                            SpectatorUser.send(new PACKET_ROOM_UDP(SpectatorUser, tempPlayers));

                        /* Send out changed Room Data */

                        foreach (virtualUser _User in UserManager.getUsersInChannel(Room.Channel, false))
                        {
                            if (_User.Page == Math.Floor(Convert.ToDecimal(Room.ID / 14)))
                            {
                                _User.send(new PACKET_ROOMLIST_UPDATE(Room));
                            }
                        }
                        return;
                    }
                }
            }
            catch(Exception ex)
            {
                Log.AppendError("Error at QuickJoin: " + ex.Message);
            }
        }
    }
}
