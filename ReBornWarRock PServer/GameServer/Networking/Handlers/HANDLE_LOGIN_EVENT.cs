using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReBornWarRock_PServer.GameServer.Networking.Handlers
{
    class HANDLE_LOGIN_EVENT : PacketHandler
    {
        string getWeapon(int Count)
        {
            switch (Count)
            {
                case 0: return "DA06";
                case 1: return "CE02";
                case 2: return "DF35";
                case 3: return "CT02";
                case 4: return "DV01";
                case 5: return "DS01";
                case 6: return "CA01";
            }
            return "NULL";
        }
        public override void Handle(ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser User)
        {
            string Weapon = getWeapon(User.LoginEvent);
            if (User.LoginEventCheck == 0)
            {
                User.LoginEventCheck = 1;
                User.AddItem(Weapon, 3, 1);
                User.send(new Packets.PACKET_LOGIN_EVENT(User, Weapon));
                User.LoginEvent++;
                DB.runQuery("UPDATE users SET loginevent='" + User.LoginEvent + "', logineventcheck='1' WHERE id='" + User.UserID + "'");
            }
            else
                User.send(new Packets.PACKET_LOGIN_EVENT());
        }
    }
}
