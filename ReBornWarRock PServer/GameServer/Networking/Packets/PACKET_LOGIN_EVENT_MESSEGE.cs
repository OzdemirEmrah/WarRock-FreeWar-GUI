using ReBornWarRock_PServer.GameServer;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    internal class PACKET_LOGIN_EVENT_MESSEGE : Packet
    {
        public PACKET_LOGIN_EVENT_MESSEGE(virtualUser User)
        {
            newPacket(21281);
            addBlock("0");
            addBlock("CZ99");
            addBlock((User.rebuildWeaponList()));
            addBlock(User.rebuildCostumeList());
        }
    }
}
