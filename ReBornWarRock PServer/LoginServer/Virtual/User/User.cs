using System;
using System.Net;
using System.Net.Sockets;
using ReBornWarRock_PServer.LoginServer.Docs;
using ReBornWarRock_PServer.LoginServer.Packets;
using ReBornWarRock_PServer.LoginServer.Packets.List_Packets;

namespace ReBornWarRock_PServer.LoginServer.Virtual.User
{
    class User
    {
        public int UserID;
        public string Nickname = "";
        public string Username = "";
        public string Password = "";
        public int SessionID = 0;
        public int AccessLevel = 0;
        public int ClanID = -1;
        public int ClanRank = 0;
        public long ClanIconID = 0;
        public string ClanName = "NULL";
        public bool FirstLogin = false;
        public int Rank = 1;
        public int Banned = 0;

        private Socket uSocket;
        private byte[] dataBuffer = new byte[1024];
        private bool isDisconnected = false;

        public string Hostname
        {
            get
            {
                string hostName = Dns.GetHostEntry(this.IPAddress).HostName;
                return hostName;
            }
        }

        public string IPAddress
        {
            get
            {
                string str = this.uSocket.RemoteEndPoint.ToString();
                char[] chrArray = new char[] { ':' };
                string str1 = str.Split(chrArray)[0];
                return str1;
            }
        }


        public User(Socket uSocket)
        {
            this.uSocket = uSocket;
            string ip = uSocket.LocalEndPoint.ToString();
            this.send(new PACKET_CONNECT());
            uSocket.BeginReceive(this.dataBuffer, 0, (int)this.dataBuffer.Length, SocketFlags.None, new AsyncCallback(this.arrivedData), null);
        }

        private void arrivedData(IAsyncResult iAr)
        {
            try
            {
                int num = this.uSocket.EndReceive(iAr);
                if (num <= 1)
                {
                    //this.disconnect();
                }
                else
                {
                    byte[] numArray = new byte[num];
                    Array.Copy(this.dataBuffer, 0, numArray, 0, num);
                    for (int i = 0; i < (int)numArray.Length; i++)
                    {
                        numArray[i] = (byte)(numArray[i] ^ 195);
                    }
                    PacketHandler packetHandler = PacketManager.parsePacket(numArray);
                    if (packetHandler != null)
                    {
                        packetHandler.Handle(this);
                    }
                    this.uSocket.BeginReceive(this.dataBuffer, 0, (int)this.dataBuffer.Length, SocketFlags.None, new AsyncCallback(this.arrivedData), null);
                }
            }
            catch
            {
                this.disconnect();
            }
        }

        public void disconnect()
        {
            if (!this.isDisconnected)
            {
                this.isDisconnected = true;
                Log.AppendText(string.Concat("IP : ", this.IPAddress, " is disconnected"));
                try
                {
                    this.uSocket.Close();
                }
                catch
                {
                }
            }
        }


        public void send(Packet p)
        {
            byte[] bytes = p.getBytes();
            try
            {
                this.uSocket.BeginSend(bytes, 0, (int)bytes.Length, SocketFlags.None, new AsyncCallback(this.sendCallBack), null);
            }
            catch
            {
                this.disconnect();
            }
        }

        private void sendCallBack(IAsyncResult iAr)
        {
            try
            {
                this.uSocket.EndSend(iAr);
            }
            catch
            {
                this.disconnect();
            }
        }
    }
}
