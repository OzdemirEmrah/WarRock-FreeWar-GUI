using System;
namespace ReBornWarRock_PServer.GameServer.Networking.Handlers
{
    class HANDLE_ROOM_KICK : PacketHandler
    {
        public override void Handle(ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser User)
        {
            try
            {
                if (User.Room != null)
                {
                    if (User.Room.RoomMasterSlot == User.RoomSlot)
                    {
                        int Slot = Convert.ToInt32(getNextBlock());
                        ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser _Target = User.Room.getPlayer(Slot);
                        if (_Target != null)
                        {
                            _Target.send(new Packets.PACKET_ROOM_KICK(Slot));
                        }
                    }
                }
            }
            catch { }
        }
    }
}