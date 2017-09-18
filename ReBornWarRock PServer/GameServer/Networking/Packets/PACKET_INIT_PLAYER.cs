using ReBornWarRock_PServer.GameServer;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.Room;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;
using System;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    internal class PACKET_INIT_PLAYER : Packet
    {
        public PACKET_INIT_PLAYER(virtualUser User)
        {
            try
            {
                virtualRoom Room = User.Room;
                if (Room == null)
                    return;
                this.newPacket(30017);
                this.addBlock(Room.Players.Count);
                foreach (virtualUser RoomUser in Room.Players)
                {
                    this.addBlock(RoomUser.RoomSlot);
                    this.addBlock(RoomUser.Health);
                    this.addBlock(-1);
                    this.addBlock(-1);
                    this.addBlock(-1);
                }
                this.addBlock(Room.Vehicles.Count);
                foreach (Vehicle virtualVehicle in Room.Vehicles.Values)
                {
                    this.addBlock(virtualVehicle.ID);
                    this.addBlock(virtualVehicle.Health);
                    this.addBlock(virtualVehicle.MaxHealth);
                    this.addBlock("NULL");
                }
            }
            catch (Exception ex)
            {
                Log.AppendError("Error at init player: " + ex.Message);
            }
        }
    }
}