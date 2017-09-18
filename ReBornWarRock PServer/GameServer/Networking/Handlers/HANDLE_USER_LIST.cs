using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

using ReBornWarRock_PServer.GameServer.Managers;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;

namespace ReBornWarRock_PServer.GameServer.Networking.Handlers
{
    class HANDLE_USER_LIST : PacketHandler
    {
        public override void Handle(virtualUser _User)
        {
            try
            {
                int Operation = Convert.ToInt32(getBlock(0));
                ArrayList PlayerList = new ArrayList();

                if (Operation == 0) // All
                {
                    foreach (virtualUser rPlayers in UserManager.getAllUsers())
                    {
                        if (rPlayers.GMMode == false && PlayerList.Count < 50)
                            PlayerList.Add(rPlayers);
                    }
                }
                else if (Operation == 1) // by Name
                {
                    string Nickname = getBlock(1);
                    foreach (virtualUser _Client in Managers.UserManager.getAllUsers())
                    {
                        if (_Client.Nickname.ToLower().Contains(Nickname.ToLower()) && PlayerList.Count < 50 && _Client.GMMode == false)
                        {
                            PlayerList.Add(_Client);
                        }
                    }
                }
                else if (Operation == 2) // by Clan
                {
                    string ClanName = getBlock(1).ToLower();
                    if (ClanName != "null")
                    {
                        foreach (virtualUser User in UserManager.getAllUsers())
                        {
                            if (User.ClanName.ToLower().Contains(ClanName) && User.GMMode == false && PlayerList.Count < 50)
                            {
                                PlayerList.Add(User);
                            }
                        }
                    }
                }
                else if (Operation == 3) // by Level
                {
                    string Range = getBlock(1);

                    if (Range.Contains("-"))
                    {
                        int Range1 = Convert.ToInt32(Range.Split(new char[] { '-' })[0]);
                        int Range2 = Convert.ToInt32(Range.Split(new char[] { '-' })[1]);

                        foreach (virtualUser _Client in Managers.UserManager.getAllUsers())
                        {
                            if (Managers.LevelCalculator.getLevelforExp(_Client.Exp) >= Range1 && Managers.LevelCalculator.getLevelforExp(_Client.Exp) <= Range2 && PlayerList.Count < 50 && _Client.GMMode == false)
                            {
                                PlayerList.Add(_Client);
                            }
                        }
                    }
                    else
                    {
                        foreach (virtualUser _Client in Managers.UserManager.getAllUsers())
                        {
                            if (Managers.LevelCalculator.getLevelforExp(_Client.Exp) == Convert.ToInt32(Range) && _Client.GMMode == false && PlayerList.Count < 50)
                            {
                                PlayerList.Add(_Client);
                            }
                        }
                    }
                }
                
                _User.send(new Packets.PACKET_USER_LIST(PlayerList));
            }
            catch (Exception ex)
            {
                Log.AppendError(ex.Message);
            }
        }
    }
}

