using ReBornWarRock_PServer.GameServer;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ReBornWarRock_PServer.GameServer
{
    internal class NetworkSocket
    {
        private static int _Limit = 0;
        private static int _AcceptedConnections = 0;
        private static UDPServer UDPServ = new UDPServer();
        public static ArrayList BannedIPs = new ArrayList();
        private static Socket _Socket;
        private static HashSet<int> _ActiveConnections;

        public static bool openSocket(int Port, int Limit)
        {
            try
            {
                NetworkSocket._Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                NetworkSocket._ActiveConnections = new HashSet<int>();
                NetworkSocket._Limit = Limit;
                new Thread(new ThreadStart(NetworkSocket.clearBannedIPs)).Start();
                new Thread(new ThreadStart(NetworkSocket.clearPermaBans)).Start();
                Log.WriteDoss("Banned IPs Clear Routine started!");
                UDPServer udpServer = new UDPServer();
                try
                {
                    if (!NetworkSocket.UDPServ.StartUDPServer())
                        return false;
                    NetworkSocket._Socket.Bind((EndPoint)new IPEndPoint(IPAddress.Any, Port));
                    NetworkSocket._Socket.Listen(999999);
                    NetworkSocket._Socket.BeginAccept(new AsyncCallback(NetworkSocket.acceptConnection), NetworkSocket._Socket);
                    Log.AppendText("Accepting connections from " + Port);
                    Log.AppendText("Listening on ports #5350-#5351 for UDP");
                    if (ConfigServer.HomePageEnabled)
                        Log.AppendText("Listening on ports #" + ConfigServer.HomePagePort + " for the Homepage");
                }
                catch
                {
                    Log.AppendError("Error while setting up asynchronous the socket server for game connections on port " + Port + ".");
                    Log.AppendError("Port " + Port + " could be invalid or in use already.");
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public static void CloseSocket()
        {
            try
            {
                NetworkSocket._Socket.Close();
            }
            catch
            {
            }
            NetworkSocket._Socket = (Socket)null;
        }

        private static void clearPermaBans()
        {
            while (true)
            {
                Thread.Sleep(900000);
                virtualProtection.clearConnections();
            }
        }

        private static void clearBannedIPs()
        {
            while (true)
            {
                virtualProtection.RunCheck();
                Thread.Sleep(1000);
            }
        }

        private static void acceptConnection(IAsyncResult iAr)
        {
            try
            {
                int SocketID = 0;
                for (int index = 1; index < NetworkSocket._Limit; ++index)
                {
                    if (!NetworkSocket._ActiveConnections.Contains(index))
                    {
                        SocketID = index;
                        break;
                    }
                }
                if (SocketID > 0)
                {
                    Socket uSocket = ((Socket)iAr.AsyncState).EndAccept(iAr);
                    string str = uSocket.RemoteEndPoint.ToString().Split(':')[0];
                    virtualProtection virtualProtection = virtualProtection.getProtectionByIP(str) ?? new virtualProtection(str);
                    ++virtualProtection.Connections;
                    if (virtualProtection.Connections >= 90 && !NetworkSocket.BannedIPs.Contains(virtualProtection))
                    {
                        ++virtualProtection.BannedTimes;
                        if (virtualProtection.BannedTimes >= 3)
                            Log.WriteDoss(virtualProtection.IP + " has been been permabanned!");
                        else
                            Log.WriteDoss(string.Concat(new object[4]
                            {
               virtualProtection.IP,
               " has been banned for connection limit (",
               virtualProtection.Connections,
               ")!"
                            }));
                        NetworkSocket.BannedIPs.Add(virtualProtection);
                    }
                    if (!NetworkSocket.BannedIPs.Contains(virtualProtection))
                    {
                       /* Log.AppendText(string.Concat(new object[4]
                        {
             "Accepted connection [",
             SocketID,
             "] from ",
             uSocket.RemoteEndPoint.ToString().Split(':')[0]
                        }));*/
                        NetworkSocket._ActiveConnections.Add(SocketID);
                        ++NetworkSocket._AcceptedConnections;
                        virtualUser virtualUser = new virtualUser(NetworkSocket._AcceptedConnections, SocketID, uSocket);
                    }
                    else
                        DB.runQuery("INSERT INTO log_connections (`timestamp`, `server`, `status`, `ip`, `host`) VALUES ('" + Structure.currTimeStamp + "', '" + ConfigServer.SERVER_ID.ToString() + "', '-1', '" + uSocket.RemoteEndPoint.ToString().Split(':')[0] + "', '" + Dns.GetHostEntry(uSocket.RemoteEndPoint.ToString().Split(':')[0]).HostName + "');");
                }
            }
            catch
            {
            }
            NetworkSocket._Socket.BeginAccept(new AsyncCallback(NetworkSocket.acceptConnection), NetworkSocket._Socket);
        }

        public static void freeSlot(int SocketID, string IPAddress, string Hostname)
        {
            if (!NetworkSocket._ActiveConnections.Contains(SocketID))
                return;
            NetworkSocket._ActiveConnections.Remove(SocketID);
        }
    }
}