using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReBornWarRock_PServer.LoginServer.Packets.List_Handle
{
    class HANDLE_UPDATE_CHECK : PacketHandler
    {
        public override void Handle(LoginServer.Virtual.User.User User)
        {
           // Log.AppendText("Sending " + User.IPAddress + " Launcher Information [" + Structure.UpdateUrl + "]");
            User.send(new List_Packets.PACKET_UPDATE_CHECK());
        }
    }
}
