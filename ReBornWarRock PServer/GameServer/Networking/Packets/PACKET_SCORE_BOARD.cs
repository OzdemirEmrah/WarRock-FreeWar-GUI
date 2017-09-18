namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_SCORE_BOARD : Packet
    {
        public PACKET_SCORE_BOARD(ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser User)
        {
            Virtual_Objects.Room.virtualRoom Room = User.Room;

            newPacket(30032);
            addBlock(1);
            addBlock(Room.cDerbRounds);
            addBlock(Room.cNiuRounds);
            if (Room.Mode == 1)
            {
                addBlock(Room.FFAKillPoints);
                addBlock(Room.highestKills);
            }
            else
            {
                addBlock(Room.KillsDeberanLeft);
                addBlock(Room.KillsNIULeft);
            }
            addBlock(Room.PlayerCount);
            foreach (ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser RoomUser in Room.Players)
            {
                addBlock(RoomUser.RoomSlot);
                addBlock(RoomUser.rKills);
                addBlock(RoomUser.rDeaths);
                addBlock(RoomUser.rFlags);
                addBlock(RoomUser.rPoints);
                addBlock(0); // Assist in chapter 1
            }
        }
    }
}