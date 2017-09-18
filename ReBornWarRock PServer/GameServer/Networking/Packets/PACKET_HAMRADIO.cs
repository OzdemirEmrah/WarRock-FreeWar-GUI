using ReBornWarRock_PServer.GameServer;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    internal class PACKET_HAMRADIO : Packet
    {
        public PACKET_HAMRADIO(virtualUser User)
        {
            newPacket(30720);
            addBlock(1111);
            addBlock(1);
            addBlock("CZ73");
            addBlock((User.rebuildWeaponList()).ToString().Remove((User.rebuildWeaponList()).ToString().Length - 1));
            addBlock(User.getSlots());
            addBlock(User.Dinar);
        }
    }
}
