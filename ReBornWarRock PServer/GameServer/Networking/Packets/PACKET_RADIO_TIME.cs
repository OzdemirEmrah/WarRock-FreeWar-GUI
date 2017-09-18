namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_RADIO_TIME : Packet
    {
        public PACKET_RADIO_TIME(int rs, int Percentage, int Type, int Base)
        {


            // 70499140 29985 0 0 0 0 0 0 -1 0

            //29985 0 0 0 2 0 14 -1 0 
            newPacket(29985);
            addBlock(0);
            addBlock(rs);
            addBlock(0);
            addBlock(Type);
            addBlock(Base);
            addBlock(Percentage);
            addBlock(-1);
            addBlock(0);
        }
    }
    class PACKET_RADIO_TIME1 : Packet
    {
        public PACKET_RADIO_TIME1(int Type)
        {

            newPacket(29985);
            addBlock(0);
            addBlock(-1);
            addBlock(1);
            addBlock(Type);
            addBlock(-1);
            addBlock(0);
            addBlock(-1);
            addBlock(0);
        }
    }
    //class PACKET_RADIO_TIME : Packet
    //{
    //    public PACKET_RADIO_TIME(ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser Client, Virtual_Objects.currentRoom.virtualcurrentRoom currentRoom, int a, int b, int c, int RType, int g, int Completed)
    //    {


    //        // 70499140 29985 0 0 0 0 0 0 -1 0

    //        newPacket(29985);
    //        addBlock(0);
    //        addBlock(a);
    //        addBlock(0);
    //        addBlock(RType);
    //        addBlock(1);
    //        addBlock(Completed); //0
    //        addBlock(-1);
    //        addBlock(0);

    //    }
    //}
}
