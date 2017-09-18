using System;
using System.Net.Sockets;

using ReBornWarRock_PServer.GameServer.Managers;

namespace ReBornWarRock_PServer.GameServer
{
    class ServerClient
    {
        private Socket _Socket;
        private byte[] dataBuffer = new byte[1024];
        private bool isDisconnected = false;

        public ServerClient(int Port)
        {
            _Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public bool connect(string Host, int Port)
        {
            try
            {
                _Socket.Connect(Host, Port);
                _Socket.BeginReceive(dataBuffer, 0, dataBuffer.Length, SocketFlags.None, new AsyncCallback(dataArrived), _Socket);
                Log.AppendText("Succesfully connected to the loginserver!");
                return true;
            }
            catch { disconnect(); }

            return false;
        }

        public void CloseSocket()
        {
            try { _Socket.Close(); }
            catch { }

            _Socket = null;
        }

        public void send(Packet p)
        {
            try
            {
                byte[] packetData = p.getBytes();
                _Socket.BeginSend(packetData, 0, packetData.Length, SocketFlags.None, new AsyncCallback(sendCallBack), null);
            }
            catch { disconnect(); }
        }

        private void sendCallBack(IAsyncResult iAr)
        {
            try { _Socket.EndSend(iAr); }
            catch { disconnect(); }
        }

        public void dataArrived(IAsyncResult iAr)
        {
            try
            {
                int DataLength = _Socket.EndReceive(iAr);
                if (DataLength > 1)
                {
                    byte[] packetBuffer = new byte[DataLength];
                    Array.Copy(dataBuffer, 0, packetBuffer, 0, DataLength);
                    for (int I = 0; I < packetBuffer.Length; I++)
                    {
                        packetBuffer[I] = (byte)(packetBuffer[I] ^ 0x96);
                    }



                    PacketHandler pHandler = PacketManager.parsePacket(packetBuffer.ToString());
                    if (pHandler != null)
                    {
                        pHandler.HandleSC(this);
                    }
                }
                _Socket.BeginReceive(dataBuffer, 0, dataBuffer.Length, SocketFlags.None, new AsyncCallback(dataArrived), _Socket);
            }
            catch { disconnect(); }
        }

        public void disconnect()
        {
            if (isDisconnected) return;
            isDisconnected = true;

            try { _Socket.Close(); }
            catch { }
        }
    }
}
