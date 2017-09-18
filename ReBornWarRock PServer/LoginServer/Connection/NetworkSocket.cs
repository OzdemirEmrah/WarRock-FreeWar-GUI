using ReBornWarRock_PServer.LoginServer;
using ReBornWarRock_PServer.LoginServer.Virtual.User;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ReBornWarRock_PServer.LoginServer.Connection
{
    class NetworkSocket
    {
        private static Socket _Socket;

        public static ArrayList BannedIPs;

        static NetworkSocket()
        {
            NetworkSocket.BannedIPs = new ArrayList();
        }

        public NetworkSocket()
        {
        }

        private static void acceptConnection(IAsyncResult iAr)
        {
            bool flag;
            try
            {
                Socket socket = ((Socket)iAr.AsyncState).EndAccept(iAr);
                string str = socket.RemoteEndPoint.ToString();
                char[] chrArray = new char[] { ':' };
                string str1 = str.Split(chrArray)[0];
                Protection protectionByIP = Protection.getProtectionByIP(str1);
                if (protectionByIP == null)
                {
                    protectionByIP = new Protection(str1);
                }
                Protection connections = protectionByIP;
                connections.Connections = connections.Connections + 1;
                flag = (protectionByIP.Connections < 10 ? true : NetworkSocket.BannedIPs.Contains(protectionByIP));
                if (!flag)
                {
                    Protection bannedTimes = protectionByIP;
                    bannedTimes.BannedTimes = bannedTimes.BannedTimes + 1;
                    NetworkSocket.BannedIPs.Add(protectionByIP);
                }
                if (!NetworkSocket.BannedIPs.Contains(protectionByIP))
                {
                    //Message.WriteLine(string.Concat("Accepted a connection from: ", socket.RemoteEndPoint));
                    User _User = new User(socket);
                }
            }
            catch
            {
            }
            NetworkSocket._Socket.BeginAccept(new AsyncCallback(NetworkSocket.acceptConnection), NetworkSocket._Socket);
        }

        private static void clearBannedIPs()
        {
            while (true)
            {
                Protection.RunCheck();
                Thread.Sleep(1000);
            }
        }

        private static void clearPermaBans()
        {
            while (true)
            {
                Thread.Sleep(900000);
                Protection.clearConnections();
            }
        }

        public static bool openSocket(int Port)
        {
            bool flag;
            NetworkSocket._Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            (new Thread(new ThreadStart(NetworkSocket.clearBannedIPs))).Start();
            (new Thread(new ThreadStart(NetworkSocket.clearPermaBans))).Start();
            try
            {
                NetworkSocket._Socket.Bind(new IPEndPoint(IPAddress.Any, Port));
                NetworkSocket._Socket.Listen(999999);
                NetworkSocket._Socket.BeginAccept(new AsyncCallback(NetworkSocket.acceptConnection), NetworkSocket._Socket);
                flag = true;
                return flag;
            }
            catch
            {
                Log.AppendError(string.Concat("Error while setting up asynchronous the socket server for connections on port ", Port, "."));
                Log.AppendError(string.Concat("Port ", Port, " could be invalid or in use already."));
            }
            flag = false;
            return flag;
        }
    }
}
