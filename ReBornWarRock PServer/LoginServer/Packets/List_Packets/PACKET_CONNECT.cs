namespace ReBornWarRock_PServer.LoginServer.Packets.List_Packets
{
    class PACKET_CONNECT : Packet
    {
        public PACKET_CONNECT()
        {
            base.newPacket(4608);
            base.addBlock(new System.Random().Next(111111111, 999999999));
        }
    }
}
