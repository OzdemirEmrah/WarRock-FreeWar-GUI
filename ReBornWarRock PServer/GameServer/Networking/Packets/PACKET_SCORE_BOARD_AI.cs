using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.Room;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_SCORE_BOARD_AI : Packet
    {
        public PACKET_SCORE_BOARD_AI(virtualUser User)
        {
            virtualRoom Room = User.Room;
            newPacket(30032);
            addBlock(1);
            addBlock(0);
            addBlock(0);
            addBlock(0);
            addBlock(0);
            addBlock(Room.PlayerCount);

            foreach (virtualUser Player in Room.Players)
            {
                addBlock(Player.RoomSlot);
                addBlock(Player.rKills);
                addBlock(Player.rDeaths);
                addBlock(0);
                addBlock(Player.rPoints);
                addBlock(0);
                addBlock(0);
            }
        }
    }
        class PACKET_SCORE_BOARD_AI_TIMEATTACK : Packet
        {
            public PACKET_SCORE_BOARD_AI_TIMEATTACK(virtualRoom Room, virtualUser User, long Time)
            {
                newPacket(30053);
                addBlock(3);
                if (Room.Mode == 11)
                {
                    addBlock(Time);
                    switch (Room.Stage)
                    {
                        case 0:
                            {
                                var v = Room.UsersDic.Values.OrderByDescending(u => u.Kills).Take(2);
                            if (v.Count() == 1)
                            {
                                addBlock(User.RoomSlot);
                                addBlock(User.rKills > Room.TimeZombie ? Room.TimeZombie : User.rKills);
                                addBlock(-1);
                                addBlock(0);
                            }
                            else
                            {
                                foreach (virtualUser usr in v)
                                {
                                    addBlock(usr.RoomSlot);
                                    addBlock((usr.rKills > Room.TimeZombie ? Room.TimeZombie : usr.rKills));
                                }
                            }
                           
                                break;
                            }
                        case 1:
                            {
                                var v = Room.UsersDic.Values.OrderByDescending(u => u.Kills).Take(2);
                            if (v.Count() == 1)
                            {
                                addBlock(User.RoomSlot);
                                addBlock(User.hackPercentage);
                                addBlock(-1);
                                addBlock(0);
                            }
                            else
                            {
                                foreach (virtualUser usr in v)
                                {
                                    addBlock(usr.RoomSlot);
                                    addBlock(usr.hackPercentage);
                                }
                            } 
                                break;
                            }
                        case 2:
                            {
                                var v = Room.UsersDic.Values.OrderByDescending(u => u.Kills).Take(2);
                            if (v.Count() == 1)
                            {
                                addBlock(User.RoomSlot);
                                addBlock(User.DoorDamageTime);
                                addBlock(-1);
                                addBlock(0);
                            }
                            else
                            {
                                foreach (virtualUser usr in v)
                                {
                                    addBlock(usr.RoomSlot);
                                    addBlock(usr.DoorDamageTime);
                                }
                            }
                                break;
                            }
                    case 3:
                        {
                            var v = Room.UsersDic.Values.OrderByDescending(u => u.BossDamage).Take(2);
                            if (v.Count() == 1)
                            {
                                addBlock(User.RoomSlot);
                                addBlock(User.BossDamage);
                                addBlock(-1);
                                addBlock(0);
                            }
                            else
                            {
                                foreach (virtualUser usr in v)
                                {
                                    addBlock(usr.RoomSlot);
                                    addBlock(usr.BossDamage);
                                }
                            }
                            break;
                        }
                    default:
                            {
                                addBlock(0);
                                addBlock(0);
                                addBlock(0);
                                addBlock(0);
                                break;
                            }
                    }
                }
            }
        }
    }

