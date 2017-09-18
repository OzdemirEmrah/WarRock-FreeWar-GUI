using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;

namespace ReBornWarRock_PServer.GameServer.Networking.Handlers
{
    class HANDLE_ROOM_INVITE : PacketHandler
    {
        public override void Handle(virtualUser User)
        {
            try
            {
                if (User.Room != null)
                {
                    if (User.Room.PlayerCount < User.Room.MaxPlayers)
                    {
                        string Username = getNextBlock();
                        string Message = getNextBlock();

                        if (Username == "NULL") // Send Random User
                        {
                            System.Collections.ArrayList _ValidPlayers = new System.Collections.ArrayList();
                            foreach (virtualUser _User in Managers.UserManager.getAllUsers())
                            {
                                if (_User != null && _User.Room == null && _User.Channel == User.Channel)
                                    _ValidPlayers.Add(_User);
                            }

                            if (_ValidPlayers.Count > 0)
                            {
                                int rPlayer = new System.Random().Next(0, _ValidPlayers.Count - 1);

                                virtualUser _Target = (virtualUser)_ValidPlayers[rPlayer];
                                if (_Target != null)
                                {
                                    _Target.send(new Packets.PACKET_ROOM_INVITE(User, Message));
                                    User.send(new Packets.PACKET_ROOM_INVITE(Packets.PACKET_ROOM_INVITE.ErrorCodes.Invited));
                                }
                            }
                        }
                        else
                        { // Match User
                            foreach (virtualUser _User in Managers.UserManager.getAllUsers())
                            {
                                if (_User.Nickname.ToLower() == Username.ToLower())
                                {
                                    if (_User.Room == null)
                                    {
                                        _User.send(new Packets.PACKET_ROOM_INVITE(User, Message));
                                        User.send(new Packets.PACKET_ROOM_INVITE(User, Message));
                                        return;
                                    }
                                    else
                                    {
                                        User.send(new Packets.PACKET_ROOM_INVITE(Packets.PACKET_ROOM_INVITE.ErrorCodes.IsPlaying));
                                        return;
                                    }
                                }
                            }
                            User.send(new Packets.PACKET_ROOM_INVITE(Packets.PACKET_ROOM_INVITE.ErrorCodes.GenericError));
                        }
                    }
                }

            }
            catch { }
        }
    }
}
