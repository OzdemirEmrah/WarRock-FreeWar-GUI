namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_MULTIPLAYER_ZOMBIE : Packet
    {
        public PACKET_MULTIPLAYER_ZOMBIE(params object[] Params)
        {
            newPacket(31490);
            foreach (object oBlock in Params)
                addBlock(oBlock);
        }
    }

    class PACKET_JOIN_VEHICLE : Packet
    {
        public PACKET_JOIN_VEHICLE(params object[] Params)
        {
            newPacket(29969);
            foreach (object oBlock in Params)
                addBlock(oBlock);
        }
    }
}
