using ReBornWarRock_PServer.GameServer;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.Room;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;
using System;
using System.Collections;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    internal class PACKET_MAP_DATA : Packet
    {
        public PACKET_MAP_DATA(virtualRoom Room, virtualUser User)
        {
            /*Escape packet
            // Zombie --> 379669456 29968 1 6 0 1 -1 -1 -1 -1 659 2 0 -1 0 0 0 0 0 1000 -1 -1 0 12 -1 0 0 0 0 0 1000 -1 -1 0 0
            // Human  --> 379970291 29968 1 6 0 1 -1 -1 -1 -1 615 2 0 -1 0 0 0 0 0 1000 -1 -1 0 13 -1 0 0 0 0 0 1000 -1 -1 0 0
            */


            try
            {
                 newPacket(29968);
                 addBlock(1);
                 addBlock(Room.MapData.Flags);
                 for (int i = 0; i < Room.MapData.Flags; i++)
                 { 
                     addBlock(Room.Flags[i]);
                 }
                 addBlock(0);
                 addBlock(Room.Players.Count);
                foreach (virtualUser Client in Room.Players)
                {
                    /* addBlock(Client.RoomSlot);
                     addBlock(-1);
                     addBlock(Client.Room.getSide(Client));
                     addBlock(0);
                     addBlock(Client.Class);
                     addBlock(Client.Weapon);
                     addBlock(Client.Health);
                     addBlock((Client.currentVehicle == null ? -1 : Client.currentVehicle.ID));
                     addBlock((Client.currentSeat == null ? -1 : Client.currentSeat.ID));
                     addBlock(0);*/
                    addBlock(Client.RoomSlot); // Slot
                    addBlock(-1);
                    addBlock(Client.rKills);
                    addBlock(Client.rDeaths);
                    addBlock(Client.Class); // Class
                    addBlock(Client.Weapon);
                    addBlock(Client.Health); // Health
                    addBlock((Client.currentVehicle == null ? -1 : Client.currentVehicle.ID));
                    addBlock((Client.currentSeat == null ? -1 : Client.currentSeat.ID));
                    addBlock(0);
                }
                try
                {
                    ArrayList arrayList = new ArrayList();
                    foreach (Vehicle virtualVehicle in Room.Vehicles.Values)
                    {
                        if (virtualVehicle.ChangedCode != string.Empty)
                            arrayList.Add(virtualVehicle);
                    }
                     addBlock(arrayList.Count);
                    if (arrayList.Count <= 0)
                        return;
                     addBlock(" ");
                    foreach (Vehicle virtualVehicle in Room.Vehicles.Values)
                    {
                         addBlock(virtualVehicle.ID);
                         addBlock(virtualVehicle.Health);
                         addBlock(virtualVehicle.X);
                         addBlock(virtualVehicle.Y);
                         addBlock(virtualVehicle.Z);
                         addBlock(virtualVehicle.PosX);
                         addBlock(virtualVehicle.PosY);
                         addBlock(virtualVehicle.PosZ);
                         addBlock(virtualVehicle.PosZ);
                         addBlock(virtualVehicle.ChangedCode);
                         addBlock(0);
                         addBlock(0);
                         addBlock(-75);
                         addBlock(0);
                         addBlock(0);
                         addBlock(0);
                         addBlock(53);
                         addBlock(0);
                    }
                }
                catch
                {
                     addBlock(0);
                }
                
            }
            catch (Exception)
            {
            }
        }
    }
}