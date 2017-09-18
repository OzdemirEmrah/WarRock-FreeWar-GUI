using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

using ReBornWarRock_PServer.GameServer.Managers;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;

namespace ReBornWarRock_PServer.GameServer
{
    internal class UDPServer
    {
        private UdpClient UDPSocket_1;
        private UdpClient UDPSocket_2;

        public void BeginSocketA()
        {
            try
            {
                UDPSocket_1 = new UdpClient(5350);
                UDPSocket_1.BeginReceive(new AsyncCallback(onReceiveA), UDPSocket_1);

                Log.AppendText("Initialized UDP Socket #1 [5350]");
            }
            catch (Exception ex) { Log.AppendError("BeginSocketA Error: " + ex.ToString()); }
        }

        public void BeginSocketB()
        {
            try
            {
                UDPSocket_2 = new UdpClient(5351);
                UDPSocket_2.BeginReceive(new AsyncCallback(onReceiveB), UDPSocket_2);

                Log.AppendText("Initialized UDP Socket #2 [5351]");
            }
            catch (Exception ex) { Log.AppendError("BeginSocketB Error: " + ex.ToString()); }
        }

        private void onReceiveA(IAsyncResult iAr)
        {
            try
            {
                UdpClient targetClient = (UdpClient)iAr.AsyncState;
                IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);

                byte[] remoteBytes = targetClient.EndReceive(iAr, ref remoteEndPoint);

                try
                {
                    byte[] analyzedPacket = AnalyzePacket(remoteBytes, remoteEndPoint);
                    targetClient.Send(analyzedPacket, analyzedPacket.Length, remoteEndPoint);
                    targetClient.BeginReceive(new AsyncCallback(onReceiveA), targetClient);
                }
                catch
                {
                    targetClient.Close();
                    targetClient = null;
                    targetClient = new UdpClient(5350);
                    targetClient.BeginReceive(new AsyncCallback(onReceiveA), targetClient);
                }
            }
            catch (Exception ex) { Log.AppendError("OnReceiveA Error: " + ex.ToString()); }
        }

        private void onReceiveB(IAsyncResult iAr)
        {
            try
            {
                UdpClient targetClient = (UdpClient)iAr.AsyncState;
                IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);

                byte[] remoteBytes = targetClient.EndReceive(iAr, ref remoteEndPoint);

                try
                {
                    byte[] analyzedPacket = AnalyzePacket(remoteBytes, remoteEndPoint);
                    targetClient.Send(analyzedPacket, analyzedPacket.Length, remoteEndPoint);
                    targetClient.BeginReceive(new AsyncCallback(onReceiveB), targetClient);
                }
                catch
                {
                    targetClient.Close();
                    targetClient = null;
                    targetClient = new UdpClient(5351);
                    targetClient.BeginReceive(new AsyncCallback(onReceiveB), targetClient);
                }
            }
            catch (Exception ex) { Log.AppendError("OnReceiveB Error: " + ex.ToString()); }
        }

        public bool StartUDPServer()
        {
            try
            {
                BeginSocketA();
                BeginSocketB();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private byte[] AnalyzePacket(byte[] RecvPacket, IPEndPoint IPeo)
        {
            try
            {
                byte[] Response = new Byte[0];

                if (RecvPacket[0] == 0x10 && RecvPacket[1] == 0x10 && RecvPacket[2] == 0x00)
                {
                    String[] exc = IPeo.Address.ToString().Split('.');
                    int b1 = ((byte)Int32.Parse(exc[0])) ^ 0x11;
                    int b2 = ((byte)Int32.Parse(exc[1])) ^ 0x11;
                    int b3 = ((byte)Int32.Parse(exc[2])) ^ 0x11;
                    int b4 = ((byte)Int32.Parse(exc[3])) ^ 0x11;

                    Response = new Byte[65]
                    {
                         0x10, 0x10, 0, 0, RecvPacket[4], RecvPacket[5],
                         0xff, 0xff, 0xff, 0xff, 0x00, 0x00, 0x00, 0x00, 0x21, 0x00, 0x00, 0x41,
                         0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x11, 0x13, 0x11,
                         (byte)(RecvPacket[32]^0x54), (byte)(RecvPacket[33]^0x54), (byte)b1, (byte)b2, (byte)b3, (byte)b4,
                         0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x01, 0x11, 0x13, 0x11,
                         (byte)(RecvPacket[32]^0x54), (byte)(RecvPacket[33]^0x54), (byte)(RecvPacket[34]^0x54), (byte)(RecvPacket[35]^0x54), (byte)(RecvPacket[36]^0x54), (byte)(RecvPacket[37]^0x54),
                         0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x11
                    };
                }
                else if (RecvPacket[0] == 0x10 && RecvPacket[1] == 0x01 && RecvPacket[2] == 0x01)
                {
                    Response = new Byte[14] { 0x10, 0x01, 0x01, 0x00, 0x14, 0xe7, 0x00, 0x00, 0x00, 0x00,
                    RecvPacket[RecvPacket.Length - 4],
                    RecvPacket[RecvPacket.Length - 3],
                    RecvPacket[RecvPacket.Length - 2],
                    RecvPacket[RecvPacket.Length - 1] };

                    int tID = (RecvPacket[RecvPacket.Length - 4] << 24) | (RecvPacket[RecvPacket.Length - 3] << 16) | (RecvPacket[RecvPacket.Length - 2] << 8) | RecvPacket[RecvPacket.Length - 1];

                    foreach (virtualUser _Client in UserManager.getAllUsers())
                    {
                        if (_Client.UserID == tID)
                        {
                            _Client.nIP = IPeo.Address.Address;
                            byte[] ArrSortedPort = BitConverter.GetBytes(IPeo.Port);
                            _Client.nPort = BitConverter.ToUInt16(new byte[] { ArrSortedPort[1], ArrSortedPort[0] }, 0);
                            _Client.lIP = _Client.nIP;
                            _Client.lPort = _Client.nPort;
                            break;
                        }
                    }
                }
                else
                {
                    Response = new Byte[1] { 0x00 };
                }
                return Response;
            }
            catch (Exception)
            {
                //Log.AppendError("AnalyzePacket Error: " + ex.ToString());
                return new Byte[1] { 0x00 };
            }
        }
    }
}