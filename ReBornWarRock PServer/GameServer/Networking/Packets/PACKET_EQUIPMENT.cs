namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_EQUIPMENT : Packet
    {
        public enum ErrorCode
        {
            AlreadyEquipped = 97090
        }

        public PACKET_EQUIPMENT(ErrorCode Code)
        {
            newPacket(29970);
            addBlock((int)Code);
        }

        public PACKET_EQUIPMENT(int Class, string Equipment)
        {
            newPacket(29970);
            addBlock(1);
            addBlock(Class);
            addBlock(Equipment);
        }
    }
}