using ReBornWarRock_PServer.GameServer;
using ReBornWarRock_PServer.GameServer.Networking.Packets;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.Room;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;

namespace ReBornWarRock_PServer.GameServer.Managers
{
    internal class UserManager
    {
        private static Thread _pingThread = (Thread)null;
        private static ArrayList _Users = new ArrayList();
         

        public static IPEndPoint[] _LoggedPlayers { get; set; }

        public static int UserCount
        {
            get
            {
                return UserManager._Users.Count;
            }
        }

        ~UserManager()
        {
            GC.Collect();
        }

        public static void setup()
        {
            UserManager._pingThread = new Thread(new ThreadStart(UserManager.pingThread));
            UserManager._pingThread.Priority = ThreadPriority.AboveNormal;
            UserManager._pingThread.Start();
        }

        private static void pingThread()
        {
            while (true)
            {
                try
                {
                    foreach (virtualUser User in _Users)
                    {
                        if (User.pingOK)
                        {
                            try
                            {
                                byte[] buffer = new byte[32];
                                System.Net.NetworkInformation.PingOptions pingOptions = new System.Net.NetworkInformation.PingOptions(128, true);
                                System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();
                                System.Net.NetworkInformation.PingReply pingReply = ping.Send(((IPEndPoint)User.uSocket.RemoteEndPoint).Address, 75, buffer, pingOptions);

                                if (pingReply != null)
                                {
                                    switch (pingReply.Status)
                                    {
                                        case System.Net.NetworkInformation.IPStatus.Success:
                                            User.Ping = pingReply.RoundtripTime;
                                            break;
                                    }
                                }
                            }
                            catch
                            {
                            }
                            User.pingOK = false;
                            User.send(new PACKET_PING(User));
                        }
                    }
                }
                catch { }
                Thread.Sleep(2500);
            }
        }

        public static ArrayList getAllUsers()
        {
            return new ArrayList((ICollection)UserManager._Users);
        }

        public static ArrayList get50Players()
        {
            ArrayList allUsers = UserManager.getAllUsers();
            if (allUsers.Count >= 50)
                return allUsers.GetRange(0, 50);
            else
                return allUsers;
        }

        public static void SetOnlineToFriends(virtualUser usr, bool status)
        {
            foreach (virtualMessenger virtualMessenger in (IEnumerable<virtualMessenger>)usr.Friends.Values)
            {
                if (virtualMessenger != null)
                {
                    virtualUser user = UserManager.getUser(virtualMessenger.ID);
                    if (user != null)
                    {
                        virtualMessenger friend = user.getFriend(usr.UserID);
                        if (friend != null)
                            friend.isOnline = status;
                        virtualMessenger.isOnline = true;
                    }
                }
            }
        }

        /*public static virtualUser getUser(int ID)
        {
          foreach (virtualUser virtualUser in UserManager.getAllUsers())
          {
            if (virtualUser.UserID == ID)
              return virtualUser;
          }
          return (virtualUser) null;
        }*/
        public static virtualUser getUser(int ID)
        {
            foreach (virtualUser ServerUser in getAllUsers())
            {
                if (ServerUser.UserID == ID)
                    return ServerUser;
            }
            /*
            if (_Users.Contains(ID))
            {
                return (virtualUser)_Users[ID];
            }
            */
            return null;
        }
        public static virtualUser getUserBySessionID(int SessionID)
        {
            foreach (virtualUser virtualUser in UserManager._Users)
            {
                if (virtualUser.SessionID == SessionID)
                    return virtualUser;
            }
            return (virtualUser)null;
        }

        public static virtualUser getUserByRoomSlot(virtualRoom Room, int Roomslot)
        {
            foreach (virtualUser virtualUser in Room.Players)
            {
                if (virtualUser.RoomSlot == Roomslot)
                    return virtualUser;
            }
            return (virtualUser)null;
        }

        public static virtualUser getUser(string Username)
        {
            foreach (virtualUser virtualUser in UserManager._Users)
            {
                if (virtualUser.Nickname.ToLower() == Username.ToLower())
                    return virtualUser;
            }
            return (virtualUser)null;
        }

        public static virtualUser getTargetUser(int SessionID)
        {
            foreach (virtualUser virtualUser in UserManager._Users)
            {
                if (virtualUser.SessionID == SessionID)
                    return virtualUser;
            }
            return (virtualUser)null;
        }

        public static bool addUser(virtualUser User)
        {
            if (UserManager._Users.Contains(User))
                return false;
            UserManager._Users.Add(User);
            DB.runQuery("UPDATE users SET online = '1' WHERE id=" + User.UserID);
            User.LoginCountry.getName();
            Log.AppendText(User.Nickname + " logged in from " + User.Country.ToString() + " !");
            return true;
        }

        public static bool removeUser(virtualUser User)
        {
            if (!UserManager._Users.Contains(User))
                return false;
            UserManager._Users.Remove(User);
            Log.AppendText(User.Nickname + " logged out.");
            DB.runQuery("UPDATE users SET online='0', lastjoin='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', kills='" + User.Kills + "',deaths='" + User.Deaths + "', exp='" + User.Exp + "', dinar='" + User.Dinar + "' WHERE id=" + User.UserID);
            if (UserManager.UserCount == 0) FormCalling.frm1.button6InvokeVisibility("false");
            return true;
        }

        public static ArrayList getUsersInChannel(int ChannelID, bool inRoom)
        {
            ArrayList arrayList = new ArrayList();
            foreach (virtualUser virtualUser in UserManager._Users)
            {
                if (virtualUser.Channel == ChannelID && !(!inRoom & virtualUser.Room != null))
                    arrayList.Add(virtualUser);
            }
            return arrayList;
        }

        public static void sendToServer(Packet p)
        {
            foreach (virtualUser virtualUser in UserManager._Users)
                virtualUser.send(p);
        }

        public static void refreshRoom(virtualRoom Room)
        {
            foreach (virtualUser virtualUser in UserManager.getUsersInChannel(Room.Channel, false))
            {
                if ((Decimal)virtualUser.Page == Math.Floor(Convert.ToDecimal(Room.ID / 14)))
                    virtualUser.send((Packet)new PACKET_ROOMLIST_UPDATE(Room, 1));
            }
        }

        public static void sendToChannel(int Channel, bool inRoom, Packet p)
        {
            foreach (virtualUser User in UserManager._Users)
            {
                if (User.Channel == Channel && (User.Room == null || inRoom))
                {
                    User.send(p);
                    User.send((Packet)new PACKET_ROOM_LIST(User, 0));
                }
            }
        }
    }
}
