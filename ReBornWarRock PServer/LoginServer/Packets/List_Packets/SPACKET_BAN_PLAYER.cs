namespace ReBornWarRock_PServer.LoginServer.Packets.List_Packets
{
    class SPACKET_BAN_PLAYER : Packet
    {
        public SPACKET_BAN_PLAYER(int ID, int UserID, string IP, string Host)
        {
            newPacket(99992);
            addBlock(ID);
            addBlock(UserID);
            addBlock(IP);
            addBlock(Host);
        }
    }
}
