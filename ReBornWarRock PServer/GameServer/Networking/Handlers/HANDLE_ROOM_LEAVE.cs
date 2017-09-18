using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;
using ReBornWarRock_PServer.GameServer.Networking.Packets;

namespace ReBornWarRock_PServer.GameServer.Networking.Handlers
{
    class HANDLE_ROOM_LEAVE : PacketHandler
    {
        public override void Handle(virtualUser User)
        {
            try
            {
                //if (User.Room.RoomType == 1 && User.Room.RoomStatus == 2) return;
                Virtual_Objects.Room.virtualRoom Room = User.Room;
                if (Room.Channel == 3 && Room.Mode == 12)
                {
                    if (User.IsEscapeZombie) Room.EscapeZombie--;
                    else Room.EscapeHuman--;
                }
                if (User.isSpectating)
                {
                    User.Room.removeSpectator(User);
                    return;
                }
                if (User.Room != null)
                    User.Room.RemoveUser(User.RoomSlot);
                User.Alive = false; //FreeWar : Test Per Bug CQC
                User.BackedToRoom = false;
                User.send(new Packets.PACKET_ROOM_LIST(User, User.Page));
            }
            catch (Exception ex)
            {
                Log.AppendError(ex.Message);
            }
        }
    }
}
