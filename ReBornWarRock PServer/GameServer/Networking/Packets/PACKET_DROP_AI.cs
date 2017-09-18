namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_DROP_AI : Packet
    {
        public PACKET_DROP_AI(Virtual_Objects.User.virtualUser User, int ID, int UsementID, int Type)
        {
            newPacket(30000);
            addBlock(1);
            addBlock(ID);
            addBlock(17);
            addBlock(2);
            addBlock(901);
            addBlock(UsementID);
            addBlock(0);
            addBlock(-1); // Useless
            addBlock(Type); // (0 = Respawn, 1 = Medic, 2 = Ammo, 3 = Repair, 4 = Skill Point)
            addBlock(User.SessionID + UsementID);
            addBlock(ID);
            addBlock(UsementID);
            addBlock(13);
            addBlock(UsementID);
            addBlock(13);
            addBlock(UsementID);
        }
    }
}