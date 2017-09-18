using System;
using System.Net;
using System.Net.Sockets;

using ReBornWarRock_PServer.LoginServer.Docs;
using ReBornWarRock_PServer.LoginServer.Packets;
using ReBornWarRock_PServer.LoginServer.Connection;
using ReBornWarRock_PServer.LoginServer.Packets.List_Packets;

namespace ReBornWarRock_PServer.LoginServer.Virtual.Server
{
    class Server
    {
        public bool PingOK = true;
        private int _ID;
        private string _Name;
        private string _IP;
        private int _Count;

        public Server(int SocketID, Socket uSocket)
        {
            this.SocketID = SocketID;
            this.uSocket = uSocket;

            send(new SPACKET_CONNECT());
             
            uSocket.BeginReceive(dataBuffer, 0, dataBuffer.Length, SocketFlags.None, new AsyncCallback(arrivedData), null);
        }

        public void newPacket(int ID)
        {
            _ID = ID;
        }

        public void setCount(int Count)
        {
            _Count = Count;
        }

        public void setup(string Name, string IP)
        {
            _Name = Name;
            _IP = IP;
        }

        public string getName()
        {
            return _Name;
        }

        public int getID()
        {
            return _ID;
        }

        public int getCount()
        {
            return _Count;
        }

        public string getIP()
        {
            return _IP;
        }

        #region Networking

        private int SocketID;
        private Socket uSocket;
        private byte[] dataBuffer = new byte[1024];
        private bool isDisconnected = false;

        #region Arrival

        private void arrivedData(IAsyncResult iAr)
        {
            try
            {
                int DataLength = uSocket.EndReceive(iAr);

                if (DataLength > 1)
                {
                    byte[] packetBuffer = new byte[DataLength];
                    Array.Copy(dataBuffer, 0, packetBuffer, 0, DataLength);

                    /* Decode Packet */
                    for (int I = 0; I < packetBuffer.Length; I++)
                    {
                        packetBuffer[I] = (byte)(packetBuffer[I] ^ 0x96);
                    }

                    PacketHandler pHandler = PacketManager.parsePacket(packetBuffer);

                    if (pHandler != null)
                    {
                        pHandler.Handle(this);
                    }
                }

                uSocket.BeginReceive(dataBuffer, 0, dataBuffer.Length, SocketFlags.None, new AsyncCallback(arrivedData), null); 
            }
            catch { disconnect(); }
        }

        #endregion

        #region Send

        public void send(Packet p)
        {
            byte[] sendBuffer = p.getBytes();
            try { uSocket.BeginSend(sendBuffer, 0, sendBuffer.Length, SocketFlags.None, new AsyncCallback(sendCallBack), null); }
            catch { disconnect(); }
        }

        private void sendCallBack(IAsyncResult iAr)
        {
            try { uSocket.EndSend(iAr); }
            catch { disconnect(); }
        }

        #endregion

        #region Disconnect
        public void disconnect()
        {
            if (isDisconnected) return;

            isDisconnected = true;

            ServerManager.removeServer(_ID);

            try { uSocket.Close(); }
            catch { }

            ServerSocket.freeSlot(SocketID);

        }
        #endregion

        #endregion
    }
}