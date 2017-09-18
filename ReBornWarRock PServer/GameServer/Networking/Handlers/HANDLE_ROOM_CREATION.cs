using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ReBornWarRock_PServer.GameServer.Virtual_Objects.Room;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;
using ReBornWarRock_PServer.GameServer.Managers;
using ReBornWarRock_PServer.GameServer.Networking.Packets;

namespace ReBornWarRock_PServer.GameServer.Networking.Handlers
{
    class HANDLE_ROOM_CREATION : PacketHandler
    {
        public override void Handle(ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser User)
        {
            try
            {
                int ResultID = RoomManager.getOpenID(User.Channel);

                if (User.Channel != 3)
                {
                    if (ResultID >= 0 && (User.Channel >= 1 && User.Channel <= 3))
                    {
                        if (User.Room == null)
                        {
                            virtualRoom Room = new virtualRoom();
                            /*ToXiiC's Noob Stuff*/
                            Room.ID = ResultID;
                            Room.Channel = User.Channel;
                            Room.Name = getBlock(0);
                            Room.EnablePassword = Convert.ToInt32(getBlock(1));
                            Room.Password = getBlock(2);
                            Room.MaxPlayers = (Convert.ToInt32(getBlock(3)) + 1);
                            Room.MapID = Convert.ToInt32(getBlock(4));
                            Room.SuperMaster = User.hasItem("CC02");

                            switch (Room.Channel)
                            {
                                case 1:
                                    {
                                        switch (Room.MaxPlayers)
                                        {
                                            case 1: Room.MaxPlayers = 8; break;
                                            case 2: Room.MaxPlayers = 16; break;
                                            case 3: Room.MaxPlayers = 20; break;
                                            case 4: Room.MaxPlayers = 24; break;
                                        }
                                        break;
                                    }
                                case 2:
                                    {
                                        switch (Room.MaxPlayers)
                                        {
                                            case 1: Room.MaxPlayers = 8; break;
                                            case 2: Room.MaxPlayers = 16; break;
                                            case 3: Room.MaxPlayers = 20; break;
                                            case 4: Room.MaxPlayers = 24; break;
                                            case 5: Room.MaxPlayers = 32; break;
                                        }
                                        break;
                                    }
                            }
                            if (Room.MapID == 66) Room.MaxPlayers = 32;
                            Room.Mode = Convert.ToInt32(getBlock(5));
                            Room.RoomType = Convert.ToInt32(getBlock(7));

                            if (Room.Name.StartsWith("E|") && User.Rank > 2 && Room.Name.Length > 2)
                            {
                                Room.Name = Room.Name.Substring(2);
                                Room.RoomType = 3;
                            }
                            Room.LevelLimit = Convert.ToInt32(getBlock(8));
                            Room.PremiumOnly = Convert.ToInt32(getBlock(9));
                            Room.VoteKick = Convert.ToInt32(getBlock(10));
                            if (Room.Mode == 0)
                                Room.Rounds = Convert.ToInt32(getBlock(12));
                            else
                                Room.Rounds = Convert.ToInt32(getBlock(11));
                            Room.TimeLimit = Convert.ToInt32(getBlock(14));
                            Room.Ping = Convert.ToInt32(getBlock(15));
                            
                            int isAutoStart = Convert.ToInt32(getBlock(16));
                            if (isAutoStart == 1)
                                Room.AutoStart = true;
                            if (Room.RoomType == 1 && Room.Mode == 1 && Room.Channel == 1)
                                Room.Mode = 0;
                            Room.NewMode = Convert.ToInt32(getBlock(17));
                            Room.SubNewMode = Convert.ToInt32(getBlock(18));
                            if (Room.joinUser(User, 0))
                            {
                                if (RoomManager.addRoom(Room.Channel, Room.ID, Room))
                                {
                                    User.isReady = User.isSpawned = false;
                                    User.RoomSlot = 0;
                                    User.send(new PACKET_CREATE_ROOM(Room));
                                    Room.UserLimitCount = Room.MaxPlayers;
                                    foreach (virtualUser _User in UserManager.getUsersInChannel(Room.Channel, false))
                                    {
                                        if (_User.Page == Math.Floor(Convert.ToDecimal(Room.ID / 14)))
                                        {
                                            _User.send(new PACKET_ROOMLIST_UPDATE(Room, 0));
                                            _User.send(new PACKET_ROOMLIST_UPDATE(Room));
                                        }
                                    }
                                }
                                else
                                {
                                    Log.AppendError("Couldn't Add Room" + Room.ID + ":" + Room.Name + " To the Room Pool");
                                    //Error Joining Room?
                                    User.disconnect();
                                }
                            }
                            else
                            {
                                Log.AppendError("Couldn't Join Room" + Room.ID + ":" + Room.Name + " As Master");
                                //Error Joining Room?
                                User.disconnect();
                            }
                        }
                    }
                }
                else
                {
                    if (ResultID >= 0 && User.Room == null)
                    {
                        virtualRoom Room = new virtualRoom()
                        {
                            ID = ResultID,
                            Channel = User.Channel,
                            Name = getNextBlock(),
                            EnablePassword = Convert.ToInt32(getNextBlock()),
                            Password = getNextBlock(),
                            MaxPlayers = 4,
                            MapID = Convert.ToInt32(getBlock(4)),
                        };

                        Room.Mode = Convert.ToInt32(getBlock(5));
                        Room.LevelLimit = Convert.ToInt32(getBlock(8));
                        Room.ZombieDifficulty = Convert.ToInt32(getBlock(13));
                        Room.Ping = Convert.ToInt32(getBlock(15));
                        if (Room.Channel == 3 && Room.Mode != 12)
                        {
                            for (int i = 0; i < 26; i++)
                            {
                                Room.Zombies.Add(i + 4, new virtualZombie((i + 4), 0, 0, 0));
                            }
                        }
                        int isAutoStart = Convert.ToInt32(getBlock(16));
                        if (isAutoStart == 1)
                            Room.AutoStart = true;

                        if (Room.joinUser(User, 0))
                        {
                            if (RoomManager.addRoom(Room.Channel, Room.ID, Room))
                            {
                                User.send(new PACKET_CREATE_ROOM(Room));
                                Room.UserLimitCount = Room.MaxPlayers;
                                foreach (virtualUser _User in UserManager.getUsersInChannel(Room.Channel, false))
                                {
                                    if (_User.Page == Math.Floor(Convert.ToDecimal(Room.ID / 14)))
                                    {
                                        _User.send(new PACKET_ROOMLIST_UPDATE(Room, 0));
                                        _User.send(new PACKET_ROOMLIST_UPDATE(Room));
                                    }
                                }
                            }
                            else
                            {
                                Log.AppendError("Couldn't Add Room" + Room.ID + ":" + Room.Name + " To the Room Pool");
                                User.disconnect();
                            }
                        }
                        else
                        {
                            Log.AppendError("Couldn't Join Room" + Room.ID + ":" + Room.Name + " As Master");
                            User.disconnect();
                        }
                    }
                }
            }
            catch (Exception ex) { Log.AppendError(ex.Message); }
        }
    }
}