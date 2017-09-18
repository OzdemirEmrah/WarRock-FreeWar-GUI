using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReBornWarRock_PServer.LoginServer.Packets.List_Packets
{
    class PACKET_UPDATE_CHECK : Packet
    { 
        public PACKET_UPDATE_CHECK()
        {
            newPacket(4112);
            addBlock(Structure.Format); // Format
            addBlock(Structure.Launcher); // Launcher Version
            addBlock(Structure.Updater); // Updater Version
            addBlock(Structure.Client); // Client Version
            addBlock(Structure.Sub); // Sub Version
            addBlock(Structure.Option); // Option
            addBlock(Structure.UpdateUrl);
        }
    }
}