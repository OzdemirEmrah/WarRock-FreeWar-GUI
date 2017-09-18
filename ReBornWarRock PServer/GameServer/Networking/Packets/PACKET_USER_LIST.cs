using System.Collections;

using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_USER_LIST : Packet
    {
        public PACKET_USER_LIST(ArrayList PlayerList)
        {
            newPacket(28960);
            addBlock(PlayerList.Count);

            foreach (virtualUser _Client in PlayerList)
            {
                //23512231 FanOfHannah 0 -1 -1 35 0 -1 -1 -1
                addBlock(_Client.UserID);
                addBlock(_Client.Nickname);
                addBlock(_Client.Premium);
                addBlock(_Client.ClanID);
                addBlock(_Client.ClanIconID);
                addBlock(Managers.LevelCalculator.getLevelforExp(_Client.Exp));
                addBlock(_Client.Channel);
                if (_Client.Room == null || _Client.isSpectating == true)
                {
                    addBlock(-1);
                }
                else
                {
                    addBlock(_Client.Room.ID);
                }

                if (_Client.hasItem("CK01"))
                {
                    addBlock(0);
                }
                else
                {
                    addBlock(-1);
                }
                //addBlock(-1);
                addBlock(_Client.ClanName); //Prova
            }
        }
    }
}
