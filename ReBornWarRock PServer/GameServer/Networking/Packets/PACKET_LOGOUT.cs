namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_LOGOUT : Packet
    {
        public PACKET_LOGOUT()
        {
            newPacket(24576);
            addBlock(1);
        }
    }
}
