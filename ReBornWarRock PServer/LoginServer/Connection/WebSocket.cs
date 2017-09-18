using System;
using System.Net;
using System.Net.Sockets;

namespace ReBornWarRock_PServer.LoginServer.Connection
{
    class WebSocket
    { //TODO
        private static Socket ServerWebSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static string IP = "";

        public static bool load(int Port, string _IP)
        {
            try
            {
                IP = _IP;
                return true;
            }
            catch { }

            return false;
        }
    }
}
