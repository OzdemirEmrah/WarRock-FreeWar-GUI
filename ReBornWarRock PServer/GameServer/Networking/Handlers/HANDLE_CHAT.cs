using System;

using ReBornWarRock_PServer.GameServer.Managers;
using ReBornWarRock_PServer.GameServer.Networking.Packets;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;

namespace ReBornWarRock_PServer.GameServer.Networking.Handlers
{
    class HANDLE_CHAT : PacketHandler
    {
        public override void Handle(ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser User)
        {
            try
            {
                int ChatType = Convert.ToInt32(getBlock(0));
                int TargetID = Convert.ToInt32(getBlock(1));

               string TargetName = getBlock(2);
                string exMessage = getBlock(3);
                string Message = WordManager.GetBadWord(exMessage);

                string sMessage = Message.Split(new string[] { ">>" + Convert.ToChar(0x1D).ToString() }, StringSplitOptions.None)[1].Replace(Convert.ToChar(0x1D), Convert.ToChar(0x20));
                if (User.isCommand(sMessage)) return;

                int MuteTime = 60;
                long ChatTimeCheck = Structure.currTimeStamp;
                if (User.isMuted == true && User.MutedTime > ChatTimeCheck)
                {
                    User.send(new PACKET_CHAT("SERVER", PACKET_CHAT.ChatType.Whisper, "SERVER >> You are muted, please try it later again!", User.SessionID, User.Nickname));
                    return;
                }
                else
                {
                    User.isMuted = false; User.MutedTime = 0;
                }
                DB.runQuery("INSERT INTO chatlogs (`userid`, `roomid`, `message`, `timestamp`) VALUES ('" + User.UserID + "', '" + ((User.Room == null) ? -1 : User.Room.ID) + "', '" + DB.Stripslash(sMessage) + "', '" + Structure.currTimeStamp + "')");
                if (User.LastChatTick == 0)
                    User.LastChatTick = Environment.TickCount;

                long ChatTime = Environment.TickCount - User.LastChatTick;

                if (ChatTime != 0 && ChatTime < 1000)
                {
                    User.ChatWarnings++;
                    if (User.ChatWarnings >= 5 && User.Rank < 3)
                    {
                        User.MutedTime = Structure.currTimeStamp + MuteTime;
                        User.send(new PACKET_CHAT("SERVER", PACKET_CHAT.ChatType.Whisper, "SERVER >> You have been muted!", User.SessionID, User.Nickname));
                        User.isMuted = true;
                        return;
                    }
                }

                User.LastChatTick = Environment.TickCount;

                switch (ChatType)
                {
                    case 2:
                        {
                            ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser Target = UserManager.getUser(TargetName);
                            if (Target is ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser && Target.SessionID > 0)
                            {
                                User.send(new PACKET_CHAT(User, PACKET_CHAT.ChatType.Whisper, Message, Target.SessionID, Target.Nickname));
                                if (!User.Equals(Target))
                                    Target.send(new PACKET_CHAT(User, PACKET_CHAT.ChatType.Whisper, Message, Target.SessionID, Target.Nickname));
                            }
                            else
                                User.send(new PACKET_CHAT(PACKET_CHAT.ErrorCodes.ErrorUser, TargetName + Convert.ToChar(0x1D)));
                            break;
                        }
                    case 3: // Lobby2Channel
                        {
                            if (User.ColorChat == System.Drawing.Color.Empty || User.Rank < 2)
                            {
                                if (User.Rank > 2) TargetID = 999;
                                UserManager.sendToChannel(User.Channel, false, new PACKET_CHAT(User, PACKET_CHAT.ChatType.Lobby_ToChannel, Message, (User.Rank > 4) ? 999 : TargetID, TargetName));
                            }
                            else
                            {
                                UserManager.sendToChannel(User.Channel, false, new PACKET_CHAT_COLOR(Message, PACKET_CHAT_COLOR.ChatType.Normal, User.ColorChat));
                                //UserManager.sendToChannel(usr.channel, false, new SP_ColoredChat(message, SP_ColoredChat.ChatType.Normal, usr.chatColor));
                            }
                            break;
                        }  
                    case 4: // Room2All
                        {
                            if (User.Room != null)
                            {
                                if (User.ColorChat != System.Drawing.Color.Empty && !User.Room.GameActive && User.Rank >= 2)
                                {
                                    User.Room.send(new PACKET_CHAT_COLOR(Message, PACKET_CHAT_COLOR.ChatType.Normal, User.ColorChat));
                                }
                                else
                                {
                                    if (User.hasItem("CC02") && User.Room.RoomStatus == 1 && User.RoomSlot == User.Room.RoomMasterSlot && User.Rank < 3)
                                    TargetID = 998;
                                else if (User.Rank > 2)
                                    TargetID = 999;

                                User.Room.send(new PACKET_CHAT(User, PACKET_CHAT.ChatType.Room_ToAll, Message, TargetID, TargetName));
                                }
                            }
                            else
                                User.disconnect();
                            break;
                        }
                    case 5: // Room2Team
                        {
                            if (User.Room != null)
                            {
                                foreach (ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser RoomUser in User.Room.Players)
                                {
                                    if (User.Room.getSide(User) == User.Room.getSide(RoomUser))
                                        RoomUser.send(new PACKET_CHAT(User, PACKET_CHAT.ChatType.Room_ToTeam, Message, (User.Rank > 2) ? 999 : TargetID, TargetName));
                                }
                                foreach (ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser SpectatorUser in User.Room.Spectators)
                                    SpectatorUser.send(new PACKET_CHAT(User, PACKET_CHAT.ChatType.Room_ToTeam, Message, (User.Rank > 2) ? 999 : TargetID, TargetName));
                            }
                            else
                                User.disconnect();
                            break;
                        }
                    case 6: // Whisper
                        {
                            ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser Target = UserManager.getUser(TargetName);
                            if (Target is ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser && Target.SessionID > 0 && Target.GMMode == false)
                            {
                                User.send(new PACKET_CHAT(User, PACKET_CHAT.ChatType.Whisper, Message, Target.SessionID, Target.Nickname));
                                if (!User.Equals(Target))
                                    Target.send(new PACKET_CHAT(User, PACKET_CHAT.ChatType.Whisper, Message, Target.SessionID, Target.Nickname));
                            }
                            else
                                User.send(new PACKET_CHAT(PACKET_CHAT.ErrorCodes.ErrorUser, TargetName + Convert.ToChar(0x1D)));
                            break;
                        }
                    case 8: //Lobby2AllChannels
                        {
                            UserManager.sendToChannel(1, false, new PACKET_CHAT(User, PACKET_CHAT.ChatType.Lobby_ToChannel, Message, TargetID, TargetName));
                            UserManager.sendToChannel(2, false, new PACKET_CHAT(User, PACKET_CHAT.ChatType.Lobby_ToChannel, Message, TargetID, TargetName));
                            UserManager.sendToChannel(3, false, new PACKET_CHAT(User, PACKET_CHAT.ChatType.Lobby_ToChannel, Message, TargetID, TargetName));
                            break;
                        }
                    case 9: //RadioChat
                            {
                                if (User.Room == null) return;

                                //byte[] buffer = (new PACKET_CHAT(User, PACKET_CHAT.ChatType.RadioChat, Message, TargetID, TargetName)).GetBytes();

                                int mySide = User.Room.getSide(User);

                                foreach (virtualUser u in User.Room.Players)
                                {
                                if (User.Room.getSide(User) == User.Room.getSide(u))
                                    u.send(new PACKET_CHAT(User, PACKET_CHAT.ChatType.RadioChat, Message, (User.Rank > 2) ? 999 : TargetID, TargetName));

                                // u.send(buffer);
                            }

                            break;
                            }
                    case 10: // Clan
                        {
                            if(User.ClanID != -1 && User.ClanRank != -1)
                                ClanManager.sendToClan(User, new PACKET_CHAT(User, PACKET_CHAT.ChatType.Clan, Message, TargetID, TargetName));
                            else
                                User.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Whisper, "SYSTEM >> Chat available after creating or get accepted from a clan", User.SessionID, User.Nickname));
                            break;
                        }
                   default:
                        {
                            Log.WriteDebug("New unknow operation for chat: " + ChatType);
                            Log.WriteDebug("Blocks: " + string.Join(" ", getAllBlocks()));
                            break;
                        }
                }
                string[] args = sMessage.Split(Convert.ToChar(0x20));
                if (args.Length >= 2)
                {
                    if (args[0] == "I" && args[1] == "Hate" && args[2] == "WarRock")
                    {
                        if (User.CheckForEvent(5) == false)
                        {
                            string ItemCode = "CB27";
                            User.AddItem(ItemCode, -1, 1);
                            User.send(new PACKET_CHAT_EVENT(User, ItemCode));
                            DB.runQuery("INSERT INTO users_events (userid, eventid) VALUES ('" + User.UserID + "', '5')");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.AppendError(ex.Message);
            }
        }
    }
}
