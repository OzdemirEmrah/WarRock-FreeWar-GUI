namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_BOMB : Packet
    {
        public PACKET_BOMB(params object[] Params)
        {
            newPacket(29984);
            addBlock(1);
            for (int i = 0; i < Params.Length; i++) {
                addBlock(Params[i]);
            }
            //addBlock(0);
        }
    }
}
