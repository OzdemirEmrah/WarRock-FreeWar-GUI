namespace ReBornWarRock_PServer.LoginServer.Packets.List_Packets
{
    class SPACKET_CONNECT : Packet
    {
        public SPACKET_CONNECT()
        {
            base.newPacket(99989);
            base.addBlock(new System.Random().Next(111111111, 999999999));
        }
    }
}

