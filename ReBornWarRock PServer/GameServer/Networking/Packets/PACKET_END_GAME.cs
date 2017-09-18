using ReBornWarRock_PServer.GameServer;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.Room;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    internal class PACKET_END_GAME : Packet
    {
        public PACKET_END_GAME(virtualRoom Room)
        {
            newPacket(30048);
            addBlock(1);
            if (Room.Mode != 5)
            {
            addBlock((Room.Channel != 1 || Room.Mode != 0 ? Room.KillsDeberanLeft : Room.cDerbRounds));
            addBlock((Room.Channel != 1 || Room.Mode != 0 ? Room.KillsNIULeft : Room.cNiuRounds));
            }
            else
            {
                if (Room.MapID == 42)
                {
                    addBlock(Room.Mission3 != null ? 1 : 0);
                    addBlock(Room.Mission3 != null ? 0 : 1);
                }
                else if (Room.MapID == 56)
                {
                    addBlock(Room.Mission2 != null ? 1 : 0);
                    addBlock(Room.Mission2 != null ? 0 : 1);
                }
                else
                {
                    Fill(0, 2);
                }
            }
            addBlock(Room.KillsDerb);
            addBlock(Room.DeathDerb);
            addBlock(Room.KillsNiu);
            addBlock(Room.DeathNiu);
            addBlock(0);
            addBlock(0);
            addBlock(Room.Players.Count);
            foreach (virtualUser virtualUser in Room.Players)
            {
                addBlock(virtualUser.RoomSlot);
                addBlock(virtualUser.rKills);
                addBlock(virtualUser.rDeaths);
                addBlock(virtualUser.rFlags);
                addBlock(virtualUser.rPoints);
                addBlock(virtualUser.DinarEarned);
                addBlock(virtualUser.ExpEarned);
                addBlock(virtualUser.Exp);
                addBlock(0);
                addBlock(0);
            }
            addBlock(Room.RoomMasterSlot);
        }
    }
}
