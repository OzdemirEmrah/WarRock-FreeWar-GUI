namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_CREATE_ROOM : Packet
    {
        public PACKET_CREATE_ROOM(Virtual_Objects.Room.virtualRoom currentRoom)
        {
            newPacket(29440);
            addBlock(1);
            addBlock(0);
            addRoomInfo(currentRoom);
        }
    }
}
