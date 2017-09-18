using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;
using ReBornWarRock_PServer.GameServer.Networking.Packets;
using System;
using System.Threading;
using ReBornWarRock_PServer.GameServer;

namespace ReBornWarRock_PServer.GameServer.Networking.Handlers
{
    class HANDLE_WELCOME_PACKET : PacketHandler
    {
        public override void Handle(virtualUser User)
        {
            try
            {
                User.HWID = getBlock(2);
                string[] HWIDCheck = DB.runReadRow("SELECT * FROM users_hwid WHERE hwid='" + User.HWID.ToString() + "'");
                if (HWIDCheck.Length > 0)
                {
                    Log.AppendError(User.HWID + " tried to login but this HWID is banned!");
                    User.disconnect();
                }
                else
                {
                    User.send(new PACKET_LOGIN());
                }
            }
            catch (Exception ex) { Log.AppendError(ex.Message); }
        }
    }
}
