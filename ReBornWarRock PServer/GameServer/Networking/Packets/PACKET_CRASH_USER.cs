namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    internal class PACKET_CRASH_USER : Packet
    {
        public PACKET_CRASH_USER()
        {
            this.newPacket(55555);
            this.addBlock("0");
            this.addBlock("0");
        }
    }
}
