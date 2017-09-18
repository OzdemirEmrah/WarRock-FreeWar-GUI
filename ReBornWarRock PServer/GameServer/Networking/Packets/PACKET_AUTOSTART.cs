using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_AUTOSTART : Packet
    {
        public PACKET_AUTOSTART(Virtual_Objects.Room.virtualRoom Room)
        {
            //C=>991597 30000 0 1 1 1 1 0 0 0 0 0 0 0 0 0
            //S=>991474 30000 1 0 1 1 4 1 0 5 0 0 0 0 0 0 0 
            //S=>991475 30000 1 0 1 1 4 1 0 5 0 0 0 0 0 0 0 

            //C=>957670 30000 0 0 1 1 1 0 0 0 0 0 0 0 0 0 
            //S=>957566 30000 1 0 0 1 4 1 0 71 0 0 0 0 0 0 0 
            foreach (virtualUser User in Room.Players)
            {
                Room.Channel = User.Channel;
                Room.RoomStatus = 2;
                //Room.LobbyChange = true;
                //User.currentVehicle = null;
            }

            base.newPacket(30000);
            base.addBlock(1);
            base.addBlock(Room.ID);
            base.addBlock(1);
            base.addBlock(1);
            base.addBlock(4);
            base.addBlock(1);
            base.addBlock(0);
            base.addBlock(Room.MapID);
            base.addBlock(0);
            base.addBlock(0);
            base.addBlock(0);
            base.addBlock(0);
            base.addBlock(0);
            base.addBlock(0);
            base.addBlock(0);
        }


    }
}
