namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_ZOMBIE_WAVE : Packet
    {
        public PACKET_ZOMBIE_WAVE(int Wave)
        {
            newPacket(13431);
            addBlock(1);
            addBlock(13);
            addBlock(Wave);
            addBlock(Wave == 22 ? 1 : 0);
        }
    }
}
