using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_DELETE_COSTUME : Packet
    {
        public PACKET_DELETE_COSTUME(ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser User, string itemCode)
        {
            newPacket(30225);
            addBlock(1);
            addBlock(itemCode);

            addBlock(User.rebuildCostumeList());

            addBlock(User.CostumeE);
            addBlock(User.CostumeM);
            addBlock(User.CostumeS);
            addBlock(User.CostumeA);
            addBlock(User.CostumeH);

        }
    }
}
