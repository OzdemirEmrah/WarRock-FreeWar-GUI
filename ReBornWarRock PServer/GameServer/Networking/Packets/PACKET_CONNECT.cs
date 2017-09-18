namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_CONNECT : Packet
    {
        public PACKET_CONNECT()
        {
            newPacket(4608);
            addBlock(new System.Random().Next(111111111, 999999999));
            addBlock(77);
        }
    }
}
