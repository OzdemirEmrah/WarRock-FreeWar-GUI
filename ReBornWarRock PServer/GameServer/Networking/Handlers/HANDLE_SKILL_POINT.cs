namespace ReBornWarRock_PServer.GameServer.Networking.Handlers
{
    class PACKET : Packet
    {
        public PACKET(int PacketID, params object[] Params)
        {
            newPacket(PacketID);
            for (int i = 0; i < Params.Length; i++)
                addBlock(Params[i]);
        }
    }

    class HANDLE_SKILL_POINT : PacketHandler
    {
        public override void Handle(Virtual_Objects.User.virtualUser User)
        {
            if (User.rSkillPoints >= 5)
            {
                User.rSkillPoints = 0;
                User.Room.send(new Packets.PACKET_SKILL_POINT_KILL(getAllBlocks()));
            }
        }
    }
}
