using ReBornWarRock_PServer.LoginServer;
using ReBornWarRock_PServer.LoginServer.Packets;
using ReBornWarRock_PServer.LoginServer.Packets.List_Packets;
using ReBornWarRock_PServer.LoginServer.Virtual.User;
using System;
using System.Data;

namespace ReBornWarRock_PServer.LoginServer.Packets.List_Handle
{
    class HANDLE_NEW_NICKNAME : PacketHandler
    {
        public override void Handle(LoginServer.Virtual.User.User User)
        {
            int UserID = User.UserID;
            if (UserID > 0)
            {
                string nickName = DB.Stripslash(getNextBlock());
                DataTable checkUsedNick = MYSQL.runRead("SELECT * FROM users WHERE nickname='" + nickName + "'");
                if (checkUsedNick.Rows.Count > 0)
                {
                    User.send(new List_Packets.PACKET_SERVER_LIST(List_Packets.PACKET_SERVER_LIST.errorCodes.AlreadyUsedNick));
                }
                else
                {
                    if (nickName.Contains(" ")) return;
                    DB.runQuery("UPDATE users SET nickname='" + nickName + "', firstlogin='2' WHERE id='" + User.UserID + "'");
                }
            }
            User.send(new List_Packets.PACKET_SERVER_LIST(User));
        }
    }
}
