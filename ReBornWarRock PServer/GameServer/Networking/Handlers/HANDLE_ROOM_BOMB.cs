using System;
using System.Collections.Generic;
using System.Text;

using ReBornWarRock_PServer.GameServer.Virtual_Objects.Room;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;
using ReBornWarRock_PServer.GameServer.Networking.Packets;

namespace ReBornWarRock_PServer.GameServer.Networking.Handlers
{
    class HANDLE_ROOM_BOMB : PacketHandler
    {
        public override void Handle(virtualUser User)
        {
            try
            {
                int Type = Convert.ToInt32(getNextBlock());
                virtualRoom currentRoom = User.Room;
                if (currentRoom.getSide(User) == 1 && User.Weapon == 118) // NIU
                {
                    if (currentRoom.bombPlanted == false) User.disconnect();
                    currentRoom.bombPlanted = false;
                    currentRoom.bombDefused = true;
                    currentRoom.prepareRound(1);
                    currentRoom.cNIUExplosivePoints += 12;
                }
                else if (currentRoom.getSide(User) == 0 && User.Weapon == 91) // Derb
                {
                    currentRoom.bombPlanted = true;
                    currentRoom.cDerbExplosivePoints += 6;
                    currentRoom.RoundTimeLeft = 44000;
                }
                currentRoom.send(new PACKET_BOMB(getAllBlocks()));
            }
            catch (Exception ex)
            {
                Log.AppendError("currentRoom Bomb error: " + ex.Message);
            }
        }
    }
}
