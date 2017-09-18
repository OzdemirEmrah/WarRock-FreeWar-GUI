using System.Collections;

using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_END_GAME_AI : Packet
    {
        public PACKET_END_GAME_AI(Virtual_Objects.Room.virtualRoom Room)
        {
            //30048 1 1 1555000 1 0 688 1 0 2328 1446 1049 43537 0 0 110 TimeAttack Win Hidden Stage
            //30048 1 0 112000 1 0 0 5 0 0 0 0 201684 0 0 110 Lose First Stage
            newPacket(30048);
            addBlock(1);
            if (Room.Mode == 11)
            {
                if (Room.ZombieDifficulty == 0)
                {
                    if (Room.Destructed) addBlock(1);
                    else addBlock(0);
                    Room.Zombies.Clear();
                }
                else
                {
                    if (Room.BossKilled) addBlock(1);
                    else addBlock(0);
                    Room.Zombies.Clear();
                }
            }
            else
            {
                addBlock(Room.Wave >= 22 ? 1 : 0);
                Room.Zombies.Clear();
            }
            addBlock(Room.RoundTimeSpent);
            ArrayList Players = Room.Players;
            addBlock(Players.Count); // Player count
            foreach (virtualUser Player in Players)
            {
                //688 1 0 2328 1446 1049 43537 0
                //0    5    0    0     0     0 201684 0
                addBlock(Player.RoomSlot); // Slot
                addBlock(Player.rKills); // Kills
                addBlock(Player.rDeaths); // Deaths
                addBlock(Player.rFlags); //Flags
                addBlock(Player.rPoints); // Points
                addBlock(Player.DinarEarned); // Dinar
                addBlock(Player.ExpEarned); // Exp
                addBlock(Player.Exp); // Player Exp
                addBlock(0); // 
                addBlock(0); //addBlock("1-0-0-0-" + Player.EndGameWord + "-0000000"); 

            }
            addBlock(Room.RoomMasterSlot);
            addBlock(110);
            
        }
    }
}