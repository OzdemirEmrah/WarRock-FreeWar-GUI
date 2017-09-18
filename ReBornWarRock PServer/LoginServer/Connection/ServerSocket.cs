using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

using ReBornWarRock_PServer.LoginServer.Virtual.Server;

namespace ReBornWarRock_PServer.LoginServer.Connection
{
    class ServerSocket
    {
        private static Socket _Socket;
        private static HashSet<int> _ActiveConnections;
        private static int _Limit = 0;
        private static int _AcceptedConnections = 0;

        public static bool openSocket(int Port, int Limit)
        {
            _Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _ActiveConnections = new HashSet<int>();
            _Limit = Limit;

            try
            {
                _Socket.Bind(new IPEndPoint(IPAddress.Any, Port));
                _Socket.Listen(5);
                _Socket.BeginAccept(new AsyncCallback(acceptConnection), _Socket);
                Log.AppendText("Accept Connection Gameserver By Port " + Port);
                return true;
            }
            catch
            {
                Log.AppendError("Error while setting up asynchronous the socket server for game server connections on port " + Port + ".");
                Log.AppendError("Port " + Port + " could be invalid or in use already.");
            }

            return false;
        }

        private static void acceptConnection(IAsyncResult iAr)
        {
            try
            {
                int SocketID = 0;

                for (int I = 1; I < _Limit; I++)
                {
                    if (_ActiveConnections.Contains(I) == false)
                    {
                        SocketID = I;
                        break;
                    }
                }

                if (SocketID > 0)
                {
                    Socket uSocket = ((Socket)iAr.AsyncState).EndAccept(iAr);
                    //Message.WriteLine("Accepted connection [" + SocketID + "] from " + uSocket.RemoteEndPoint.ToString().Split(':')[0]);
                    _ActiveConnections.Add(SocketID);
                    _AcceptedConnections++;

                    Server ServerObject = new Server(SocketID, uSocket);
                }
            }
            catch { }
            _Socket.BeginAccept(new AsyncCallback(acceptConnection), _Socket);
        }

        public static void freeSlot(int SocketID)
        {
            if (_ActiveConnections.Contains(SocketID))
            {
                Log.AppendText("Flagged connection [" + SocketID + "] as free.");
                _ActiveConnections.Remove(SocketID);
            }
        }
    }
}