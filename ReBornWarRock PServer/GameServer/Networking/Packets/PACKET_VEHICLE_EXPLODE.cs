using ReBornWarRock_PServer.GameServer;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    internal class PACKET_VEHICLE_EXPLODE : Packet
    {
        public PACKET_VEHICLE_EXPLODE(int RoomID, int TargetID, int UserSlot)
        {
            this.newPacket(30000);
            this.addBlock(1);
            this.addBlock(0);
            this.addBlock(RoomID);
            this.addBlock(2);
            this.addBlock(153);
            this.addBlock(0);
            this.addBlock(1);
            this.addBlock(0);
            this.addBlock(TargetID);
            this.addBlock(0);
            this.addBlock(0);
            this.addBlock(0);
            this.addBlock(0);
            this.addBlock(0);
            this.addBlock(0);
            this.addBlock(0);
            this.addBlock(100);
            this.addBlock(0);
            this.addBlock(0);
            this.addBlock(0);
            this.addBlock(0);
            this.addBlock(0);
            this.addBlock(0);
            this.addBlock(0);
            this.addBlock(0);
            this.addBlock(0);
            this.addBlock(0);
            this.addBlock(0);
            this.addBlock("FFFF");
        }
    }
}
