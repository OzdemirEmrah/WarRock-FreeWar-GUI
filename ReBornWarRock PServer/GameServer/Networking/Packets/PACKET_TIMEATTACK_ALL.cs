using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.Room;
using ReBornWarRock_PServer.GameServer.Networking.Handlers;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_TIMEATTACK_STAGE : Packet
    {
        public PACKET_TIMEATTACK_STAGE(Virtual_Objects.Room.virtualRoom Room, int Stage, int ZombieCount)
        {
            
            newPacket(30053);
            addBlock(Stage);
            addBlock(ZombieCount);
           }
    }
    class PACKET_TIMEATTACK_NEWSTAGE : Packet
    {
        public PACKET_TIMEATTACK_NEWSTAGE(Virtual_Objects.Room.virtualRoom Room, int Stage)
        {

            newPacket(30053);
            addBlock(Stage);
        }
    }
    class PACKET_TIMEATTACK_ALL : Packet
    {
        public PACKET_TIMEATTACK_ALL(int packetId, params object[] par)
        {
            newPacket(packetId);
            foreach (var p in par)
            {
                addBlock(p);
            }
        }
    }
    class PACKET_TIMEATTACK_END : Packet
    {
        public PACKET_TIMEATTACK_END(virtualRoom Room)
        {
            newPacket(30053);
            addBlock(1);
            addBlock(0);
        }
    }
    class PACKET_TIMEATTACK_ENDLOSE : Packet
    {
        public PACKET_TIMEATTACK_ENDLOSE(virtualRoom Room)
        {
            newPacket(30053);
            addBlock(1);
            addBlock(-1);
        }
    }

    class PACKET_SUPPLY_EVENT : Packet
    {
        public PACKET_SUPPLY_EVENT(virtualUser Client, virtualRoom Room, string ItemCode)
        {
            // 
            // 6 1 0 0 DF13 1
            newPacket(30053);
            addBlock(6); // FreeWar : Questo Dovrebbe la identificazione dello Stage!
            addBlock(1);
            addBlock(0);
            addBlock(Client.SupplyTemp);
            addBlock(ItemCode);// code arma
            addBlock("3"); // tempo

        }
    }
}