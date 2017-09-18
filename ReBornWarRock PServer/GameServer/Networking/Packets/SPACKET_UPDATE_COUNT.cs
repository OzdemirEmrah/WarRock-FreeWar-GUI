namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class SPACKET_UPDATE_COUNT : Packet
    {
        public SPACKET_UPDATE_COUNT(int Value)
        {
            setClient(false);
            newPacket(99991);
            addBlock(Value);
            addBlock(0);
        }
    }
}
