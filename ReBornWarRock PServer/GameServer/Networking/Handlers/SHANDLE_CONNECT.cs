using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ReBornWarRock_PServer.GameServer.Networking.Packets;

namespace ReBornWarRock_PServer.GameServer.Networking.Handlers
{
    class SHANDLE_CONNECT : PacketHandler
    {
        public override void HandleSC(ServerClient SClient)
        {
            SClient.send(new SPACKET_LOGIN());
        }
    }
}
