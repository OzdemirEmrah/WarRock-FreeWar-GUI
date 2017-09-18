using ReBornWarRock_PServer.LoginServer.Virtual.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReBornWarRock_PServer.LoginServer.Packets.List_Handle
{
    class SHANDLE_UPDATE_COUNT : PacketHandler
    {
        public override void Handle(Server Server)
        {
            Server.setCount(int.Parse(getNextBlock()));
        }
    }
}