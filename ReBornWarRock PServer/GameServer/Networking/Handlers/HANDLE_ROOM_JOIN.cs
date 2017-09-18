using System;
using ReBornWarRock_PServer.GameServer.Managers;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.Room;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;
using ReBornWarRock_PServer.GameServer.Networking.Packets;

using System.Collections;
using System.Collections.Generic;

namespace ReBornWarRock_PServer.GameServer.Networking.Handlers
{
    class HANDLE_ROOM_JOIN : PacketHandler
    {
        public override void Handle(ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser User)
        {
            try
            {
                if (User.Room != null) return;
                int RoomID = Convert.ToInt32(getNextBlock());
                string Password = getNextBlock();
                int Side = Convert.ToInt32(getBlock(2));//FreeWar > 0 Derb/ 1 NIU/ 2 Random

                virtualRoom Room = RoomManager.getRoom(User.Channel, RoomID);

                if (Room != null)
                {
                    bool LevelLimit = true;
                    switch (Room.LevelLimit)
                    {
                        case 0: { LevelLimit = false; break; }
                        case 1: { if (LevelCalculator.getLevelforExp(User.Exp) <= 4) LevelLimit = false; break; }
                        case 2: { if (LevelCalculator.getLevelforExp(User.Exp) > 5) LevelLimit = false; break; }
                        case 3: { if (LevelCalculator.getLevelforExp(User.Exp) > 13) LevelLimit = false; break; }
                        case 4: { if (LevelCalculator.getLevelforExp(User.Exp) > 23) LevelLimit = false; break; }
                        case 5: { if (LevelCalculator.getLevelforExp(User.Exp) > 33) LevelLimit = false; break; }
                    }

                    bool canJoinPing = false;
                    switch (Room.Ping)
                    {
                        case 0: if (User.Ping < 150) canJoinPing = true; else canJoinPing = false; break;
                        case 1: if (User.Ping < 250) canJoinPing = true; else canJoinPing = false; break;
                        case 2: canJoinPing = true; break;
                    }

                    if ((Room.Players.Count >= Room.MaxPlayers || Room.RoomType == 1 && (User.ClanRank == -1 || User.ClanRank == 9) ||(Room.RoomType == 1 && (Room.getSideCount(0) > 0 && Room.getSideCount(1) > 0 && Room.isMyClan(User) == false)) || Room.RoomType == 1 && User.ClanID == -1 || canJoinPing == false && User.Rank < 2 || Room.UserLimit || Room.isJoinAble() == false || User.pingOK == false) && User.Rank < 2)
                        User.send(new PACKET_JOIN_ROOM(PACKET_JOIN_ROOM.ErrorCodes.GenericError));
                    else if (Password != "NULL" && (Password != Room.Password))
                        User.send(new PACKET_JOIN_ROOM(PACKET_JOIN_ROOM.ErrorCodes.InvalidPassword));
                    else if (LevelLimit == true)
                        User.send(new PACKET_JOIN_ROOM(PACKET_JOIN_ROOM.ErrorCodes.BadLevel));
                    else if (User.Premium == 0 && Room.PremiumOnly == 1)
                        User.send(new PACKET_JOIN_ROOM(PACKET_JOIN_ROOM.ErrorCodes.OnlyPremium));
                    else
                    {
                        if (User.Room != null) return;
                        if (Room.joinUser(User, Side) == true)
                        {
                            ArrayList tempPlayers = new ArrayList();
                            User.isReady = false;
                            User.isSpawned = false;
                            User.BackedToRoom = false;
                            User.Health = 0;
                            foreach (virtualUser RoomUser in Room.Players)
                            {
                                if (RoomUser.Equals(User) == false)
                                    tempPlayers.Add(RoomUser);
                            }
                            User.send(new PACKET_ROOM_UDP(User, tempPlayers));

                            foreach (virtualUser tempSpectator in Room.Spectators)
                                User.send(new PACKET_ROOM_UDP_SPECTATE(tempSpectator));

                           // User.send(new PACKET_JOIN_ROOM(User, Room));

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
                        }
                        else
                        {
                            User.send(new PACKET_JOIN_ROOM(PACKET_JOIN_ROOM.ErrorCodes.GenericError));
                        }
                    }
                }
                else
                {
                    User.send(new PACKET_JOIN_ROOM(PACKET_JOIN_ROOM.ErrorCodes.GenericError));
                }
            }
            catch { User.send(new PACKET_JOIN_ROOM(PACKET_JOIN_ROOM.ErrorCodes.GenericError)); }

        }
    }
}
