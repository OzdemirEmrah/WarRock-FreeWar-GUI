namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_ZOMBIE_STAGE : Packet
    {
        public PACKET_ZOMBIE_STAGE(int Stage)
        {
            newPacket(13431);
            addBlock(1);
            addBlock(13);
            addBlock(Stage);
            addBlock(Stage == 4 ? 1 : 0);
        }
    }
}
