using ReBornWarRock_PServer.GameServer;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.Room;
using System;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    internal class PACKET_ROOM_TICK : Packet
    {
        public PACKET_ROOM_TICK(virtualRoom Room)
        {
            try
            {
                newPacket(30016);
                if (Room.Channel == 3)
                {
                    if (Room.Mode == 11)
                    {
                        addBlock(Room.InitialTime);
                        addBlock(Room.RoundTimeSpent);
                        addBlock(0);
                        addBlock(2);
                        addBlock(0);
                        addBlock(30);
                    }
                    else
                    {
                        addBlock(-1);
                        addBlock(Room.RoundTimeSpent);
                        addBlock(Room.zombiePoints);
                        addBlock(Room.zombiePoints);
                        addBlock(30);
                    }
                }
                else
                {
                    switch (Room.Mode)
                {
                    case 8: // Total War
                        {
                            //30016 322000 0 0 0 0 0 0 1478000 0 0 22 36 2 1 2
                            addBlock(Room.RoundTimeSpent);
                            Fill(0, 4);
                            addBlock(Room.Kills);
                            addBlock(0);
                            addBlock(Room.RoundTimeLeft);
                            Fill(0, 2);
                            addBlock(Room.TotalWarDerb);
                            addBlock(Room.TotalWarNIU);
                            break;
                        }
                    }
                    addBlock(Room.RoundTimeSpent);
                    addBlock(Room.RoundTimeLeft);
                    if (Room.Mode == 2 || Room.Mode == 3)
                    {
                        addBlock(0);
                        addBlock(0);
                        addBlock(Room.KillsDeberanLeft);
                        addBlock(Room.KillsNIULeft);
                    }
                    else
                    {
                        addBlock(Room.cDerbRounds);
                        addBlock(Room.cNiuRounds);
                        addBlock(Room.FFAKillPoints);
                        addBlock(Room.highestKills);
                    }
                    addBlock(2);
                    addBlock(0);
                }
                addBlock(30);
            }
            catch (Exception ex)
            {
                Log.AppendError("Error @ room tick: " + ex.Message);
            }
        }
    }
}
