using System;

namespace ReBornWarRock_PServer.GameServer.Networking.Handlers
{
    class HANDLE_PING : PacketHandler
    {
        public override void Handle(ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser User)
        {
            User.Ping = (long)Environment.TickCount - User.LastTimeStamp;
            User.pingOK = true;
            if (User.sendPing)
            {
                User.send(new Packets.PACKET_PING(User));
                User.sendPing = false;
            }
        }
    }
}
