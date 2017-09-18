using System;
using ReBornWarRock_PServer.GameServer.Managers;

using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;

namespace ReBornWarRock_PServer.GameServer.Networking.Handlers
{
    class PacketUnknown : Packet
    {
        public PacketUnknown(int packetId, params object[] objs)
        {
            newPacket(packetId);
            foreach (object p in objs)
            {
                addBlock(p);
            }
        }
    }
    class HANDLE_MESSENGER : PacketHandler
    {
        enum Subtype
        {
            FriendList = 5606,
            SendMessage = 5608,
            AvaialbleToChat = 5609,
            FriendAccept = 5610,
            DeleteFriend = 5611,
            BlockUnblock = 5612,
            FriendRequest = 5615,
            FriendDecline = 5616,
        }

        public override void Handle(virtualUser User)
        {
            try
            {
                int Operation = Convert.ToInt32(getBlock(0));
                Subtype sub = (Subtype)Operation;
                switch (sub)
                {
                    case Subtype.FriendList:
                        {
                            //32256 1 5606 3 <- [Count] 0 FrostyPrime 1 0 1 SiroSick 1 0 -1 tishina 1 4
                            User.LoadFriends();
                            User.send(new SP_MESSENGER_FRIENDS(User));
                            break;
                        }
                    case Subtype.SendMessage:
                        {
                            string nickname = getBlock(1), message = "";
                            if (nickname.Length > 0 && nickname.Length <= 32)
                            {
                                message = getBlock(2).Trim(); // Remove unused spaces. (Just incase)

                                virtualUser friend = UserManager.getUser(nickname);
                                if (friend != null)
                                {
                                    virtualMessenger vm = User.getFriend(nickname);

                                    if (vm != null && (vm.Status == 0 || vm.Status == 1))
                                    {
                                        User.send(new SP_MESSENGER_MESSAGE(User.Nickname, friend.Nickname, message));
                                        friend.send(new SP_MESSENGER_MESSAGE(User.Nickname, friend.Nickname, message));
                                    }
                                }
                            }
                            break;
                        }
                    case Subtype.AvaialbleToChat:
                        {
                            string nickname = getBlock(1);
                            if (nickname.Length > 0 && nickname.Length <= 32)
                            {
                                virtualMessenger vm = User.getFriend(nickname);
                                virtualUser tempClient = UserManager.getUser(nickname);
                                bool ChatAble = false;
                                if (tempClient != null)
                                    ChatAble = true;
                                if (vm != null && (vm.Status == 0 || vm.Status == 1) && ChatAble)
                                {
                                    User.send(new PacketUnknown(32256, 1, 5609, vm.Nickname, 0));
                                }
                            }
                            break;
                        }
                    case Subtype.BlockUnblock:
                        {
                            string nickname = getBlock(1);
                            if (nickname.Length > 0 && nickname.Length <= 32)
                            {
                                int FriendID = Convert.ToInt32(DB.runReadRow("SELECT id FROM users WHERE nickname='" + DB.Stripslash(nickname) + "'")[0]);
                                if (FriendID > 0)
                                {
                                    virtualMessenger v = User.getFriend(FriendID);

                                    if (v.Status == 1)
                                        v.Status = 2;
                                    else if (v.Status == 2)
                                        v.Status = 1;

                                    DB.runQuery("UPDATE friends SET status='" + v.Status + "' WHERE id1='" + FriendID + "' AND id2='" + User.UserID + "' OR id1='" + User.UserID + "' AND id2='" + FriendID + "'");

                                    virtualUser friend = UserManager.getUser(FriendID);
                                    if (friend != null)
                                    {
                                        friend.send(new SP_MESSENGER_FRIENDS(friend));

                                        virtualMessenger u2 = User.getFriend(FriendID);
                                        if (u2 != null)
                                        {
                                            u2.Status = v.Status;
                                        }
                                    }

                                    User.send(new SP_MESSENGER_FRIENDS(User));
                                }
                            }
                            break;
                        }
                    case Subtype.DeleteFriend:
                        {
                            string Nickname = getBlock(1);
                            string[] Query = DB.runReadRow("SELECT id FROM users WHERE nickname='" + Nickname + "'");
                            if (Query.Length > 0)
                            {
                                int FriendID = Convert.ToInt32(Query[0]);
                                if (FriendID > 0)
                                {
                                    DB.runQuery("DELETE FROM friends WHERE id1='" + FriendID + "' AND id2='" + User.UserID + "' OR id1='" + User.UserID + "' AND id2='" + FriendID + "'");

                                    virtualUser friend = UserManager.getUser(FriendID);
                                    if (friend != null)
                                    {
                                        friend.Friends.Remove(User.UserID);
                                        friend.send(new SP_MESSENGER_FRIENDS(User));
                                    }

                                    User.Friends.Remove(FriendID);
                                    User.send(new SP_MESSENGER_FRIENDS(User));
                                }
                            }
                            break;
                        }
                    //case Subtype.FriendRequest:
                    //    {
                    //        string Nickname = getBlock(1);
                    //        string[] SearchUser = DB.runReadRow("SELECT id FROM users WHERE nickname='" + Nickname + "'");
                    //        if (SearchUser.Length > 0 && SearchUser[0] != User.UserID.ToString())
                    //        {
                    //            if (User.UserID == -1 || SearchUser[0] == "-1") return;
                    //            DB.runQuery("INSERT INTO friends (id1, id2, requesterid, status) VALUES ('" + User.UserID + "', '" + SearchUser[0] + "', '" + User.UserID + "', '5')");
                    //            virtualUser Friend = Managers.UserManager.getUser(Nickname);

                    //            User.send(new SP_FRIEND_REQUEST(User.Nickname, Nickname));

                    //            if (Friend != null)
                    //            {
                    //                User.send(new SP_FRIEND_REQUEST(User.Nickname, Nickname));
                    //            }
                    //            User.LoadFriends();
                    //            User.send(new SP_MESSENGER_FRIENDS(User));
                    //        }
                    //        else
                    //        {
                    //            User.send(new PacketUnknown(32256, -11));
                    //            //User.send(new SP_WARROCK_MESSENGER(SP_WARROCK_MESSENGER.Subtype.InvalidNickname));
                    //        }
                    //        break;
                    //    }
                    case Subtype.FriendRequest:
                        {
                            string Nickname = getBlock(1);
                            string[] SearchUser = DB.runReadRow("SELECT id FROM users WHERE nickname='" + Nickname + "'");
                            if (SearchUser.Length > 0 && SearchUser[0] != User.UserID.ToString())
                            {
                                DB.runQuery("INSERT INTO friends (id1, id2, requesterid, status) VALUES ('" + User.UserID + "', '" + SearchUser[0] + "', '" + User.UserID + "', '5')");
                                virtualUser Friend = Managers.UserManager.getUser(Nickname);

                                User.send(new SP_FRIEND_REQUEST(User.Nickname, Nickname));

                                if (Friend != null)
                                {
                                    User.send(new SP_FRIEND_REQUEST(User.Nickname, Nickname));
                                }
                                User.LoadFriends();
                                User.send(new SP_MESSENGER_FRIENDS(User));
                            }
                            else
                            {
                                User.send(new PacketUnknown(32256, -11));
                            }
                            break;
                        }
                    //case Subtype.FriendRequest:
                    //    {
                    //        //32256 1 5615 xK1llSt3am Realiity <- Server
                    //        string Nickname = getBlock(1);
                    //        string[] SearchUser = DB.runReadRow("SELECT id, nickname FROM users WHERE nickname='" + Nickname + "'");
                    //        if (SearchUser.Length > 0 && SearchUser[0] != User.UserID.ToString())
                    //        {
                    //            int FriendID = Convert.ToInt32(SearchUser[0]);
                    //            if (FriendID > 0)
                    //            {
                    //                if (User.UserID == -1 || FriendID <= 0) return;
                    //                DB.runQuery("INSERT INTO friends (id1, id2, requesterid, status, nick1, nick2) VALUES ('" + User.UserID + "', '" + SearchUser[0] + "', '" + User.UserID + "', '5', '" + User.Nickname + "', '" + SearchUser[1] + "')");
                    //                virtualUser Friend = UserManager.getUser(Convert.ToInt32(SearchUser[0]));

                    //                User.send(new SP_FRIEND_REQUEST(User.Nickname, Nickname));
                    //                if (Friend != null)
                    //                {
                    //                    Friend.AddFriend(User.UserID, User.UserID, User.Nickname);
                    //                    Friend.send(new SP_FRIEND_REQUEST(User.Nickname, Nickname));
                    //                    Friend.send(new SP_MESSENGER_FRIENDS(User));
                    //                }

                    //                User.AddFriend(Convert.ToInt32(SearchUser[0]), User.UserID, SearchUser[1]);
                    //                User.send(new SP_FRIEND_REQUEST(User.Nickname, Nickname));
                    //                User.LoadFriends();
                    //                User.send(new SP_MESSENGER_FRIENDS(User));
                    //            }
                    //        }
                    //        else
                    //        {
                    //            User.send(new PacketUnknown(32256, -11));
                    //        }
                    //        break;
                    //    }
                    case Subtype.FriendAccept:
                        {
                            //5610 xK1llSt3am 0  <- Client
                            string Nickname = getBlock(1).Trim();
                            User.LoadFriends();
                            if (Nickname.Length > 0 && Nickname.Length <= 32) // Check for the nickname length for safety
                            {
                                string[] SearchUser = DB.runReadRow("SELECT id FROM users WHERE nickname='" + Nickname + "'"); // Search the user in the database
                                if (SearchUser.Length > 0 && SearchUser[0] != User.UserID.ToString()) // If the user is in the database
                                {
                                    int FriendID = Convert.ToInt32(SearchUser[0]);
                                    if (FriendID > 0)
                                    {
                                        DB.runQuery("UPDATE friends SET requesterid='-1', status='1' WHERE id1='" + FriendID + "' AND id2='" + User.UserID + "' OR id1='" + User.UserID + "' AND id2='" + FriendID + "'");

                                        virtualUser f = UserManager.getUser(FriendID);
                                        if (f != null)
                                        {
                                            virtualMessenger u = f.getFriend(User.UserID);
                                            if (u != null)
                                            {
                                                u.Status = 1;
                                            }
                                            f.send(new SP_MESSENGER_FRIENDS(f));
                                        }

                                        virtualMessenger u2 = User.getFriend(FriendID);
                                        if (u2 != null)
                                        {
                                            u2.Status = 1;
                                        }
                                        User.send(new SP_MESSENGER_FRIENDS(User));
                                        break;
                                    }
                                }
                            }

                            break;
                        }
                    case Subtype.FriendDecline:
                        {
                            string Nickname = getBlock(1).Trim();
                            if (Nickname.Length > 0 && Nickname.Length <= 32) // Check for the nickname length for safety
                            {
                                string[] SearchUser = DB.runReadRow("SELECT id FROM users WHERE nickname='" + Nickname + "'"); // Search the user in the database
                                if (SearchUser.Length > 0) // If the user is in the database
                                {
                                    int FriendID = Convert.ToInt32(SearchUser[0]);
                                    if (FriendID > 0)
                                    {
                                        DB.runQuery("DELETE FROM friends WHERE id1='" + FriendID + "' AND id2='" + User.UserID + "' OR id1='" + User.UserID + "' AND id2='" + FriendID + "'");
                                        virtualUser f = UserManager.getUser(FriendID);
                                        if (f != null)
                                        {
                                            User.RemoveFriend(User.UserID);
                                            f.send(new SP_MESSENGER_FRIENDS(f));
                                        }

                                        User.RemoveFriend(FriendID);
                                        User.LoadFriends();
                                        User.send(new SP_MESSENGER_FRIENDS(User));
                                        break;
                                    }
                                }
                            }
                            break;
                        }
                }
            }
            catch { Log.AppendError("Error on the Messenger: " + User.Nickname + " - " + string.Join(" ", getAllBlocks())); }
        }
    }

    /* Old messenger - new one (toxiic core) is not compatible with this gameserver */

    class SP_MESSENGER_FRIENDS : Packet
    {
        public SP_MESSENGER_FRIENDS(virtualUser User)
        {
            newPacket(32256);
            addBlock(1);
            addBlock(5606);
            addBlock(User.Friends.Count);
            foreach (virtualMessenger Friend in User.Friends.Values) //virtualMessenger Friend in User.Friends.Values
            {
                virtualUser oFriend = Managers.UserManager.getUser(Friend.Nickname);
                addBlock(1);
                addBlock(Friend.Nickname);
                addBlock(oFriend == null ? 0 : 1);
                addBlock((Friend.RequesterID == User.UserID && Friend.Status == 5) ? 4 : Friend.Status);
            }
        }
    }

    class SP_MESSENGER_MESSAGE : Packet
    {
        public SP_MESSENGER_MESSAGE(string User, string Friend, string Message)
        {
            newPacket(32256);
            addBlock(1);
            addBlock(5608);
            addBlock(User);
            addBlock(Friend);
            addBlock(Message);
        }
    }
    class SP_FRIEND_REQUEST : Packet
    {
        public SP_FRIEND_REQUEST(string User, string Friend)
        {
            newPacket(32956);
            addBlock(1);
            addBlock(5615);
            addBlock(User);
            addBlock(Friend);
        }
    }
    //class SP_FRIEND_REQUEST : Packet
    //{
    //    public SP_FRIEND_REQUEST(string User, string Friend)
    //    {
    //        newPacket(32956);
    //        addBlock(1);
    //        addBlock(5615);
    //        addBlock(User);
    //        addBlock(Friend);
    //    }
    //}

    class SP_WARROCK_MESSENGER : Packet
    {
        public enum Subtype
        {
            InvalidNickname = -11
        }

        public SP_WARROCK_MESSENGER(Subtype Subtype)
        {
            newPacket(36256);
            addBlock((int)Subtype);
        }
    }
}
