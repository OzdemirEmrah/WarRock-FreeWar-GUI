namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_ROOM_KICK : Packet
    {
        public PACKET_ROOM_KICK(int Slot)
        {
            newPacket(29505); // Packet ID
            addBlock(1); // Error Code = 1 = Success
            addBlock(Slot); // Slot
        }
    }
}
