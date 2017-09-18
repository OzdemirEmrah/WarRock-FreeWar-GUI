using ReBornWarRock_PServer.GameServer.Managers;
using ReBornWarRock_PServer.GameServer.Networking.Packets;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.Room;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;


// works by SdfSdf
namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_VEHICLE_RESPAWN : Packet
    {
        public PACKET_VEHICLE_RESPAWN(int ID)
        {
            newPacket(30000);
            addBlock(1);
            addBlock(-1);
            addBlock(13);
            addBlock(2);
            addBlock(151);
            addBlock(0);
            addBlock(1);
            addBlock(ID);
            addBlock(0);
            addBlock(0);
            addBlock(0);
            addBlock(20);
            addBlock(1);
            addBlock(0);
            addBlock(0);
            addBlock(1200000);
            addBlock(-1036745);
            addBlock(1200000);
            addBlock("0.0000");
            addBlock("0.0000");
            addBlock("0.0000");
            addBlock("0.0000");
            addBlock("0.0000");
            addBlock("0.0000");
            addBlock(0);
            addBlock(0);
            addBlock("DV01");
        }
    }
}
