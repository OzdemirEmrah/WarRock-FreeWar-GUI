using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameServer.Virtual_Objects.User;
using GameServer.Networking.Handlers;

namespace GameServer.Networking.Packets
{
    class PACKET_ZOMBIE_COUNT : Packet
    {
        public PACKET_ZOMBIE_COUNT(GameServer.Virtual_Objects.User.virtualUser Client, GameServer.Virtual_Objects.Room.virtualRoom Room, string ZombieCount, string cazzoneso, string cazzoneso2, string cazzoneso3, string Time, string cazzoneso4)
        {
            //FreeWar:
            //Questo pacchetto è molto grezzo, infatti se muori rimanda il pacchetto
            //Vanno ben editati perché se no rimanda lo stesso pacchetto senza
            //Modifiche e quindi passa allo stage 2.     

            // 225691402 30000 1 0 2 2 150 0 1 1 4 1 0 0 0 0 0 0 0 0 0 0 0.0000 0.0000 0.0000 0.0000 0.0000 0.0000 0.0000 0.0000 ÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿ225691402 30053 0 0 3 5225691402 30053 2 299

            newPacket(30053);
            addBlock(cazzoneso);// FreeWar : Questo Dovrebbe la identificazione dello Stage!
            addBlock(Time);
            addBlock(ZombieCount);
            addBlock(cazzoneso2);
            addBlock(cazzoneso3);
            addBlock(cazzoneso4);
            // FreeWar : Questo è il conto dei Zombie del primo Stage
            //addBlock(3); // FreeWar : Questo Non So Se Esiste!
            //addBlock(52); // FreeWar : E Neanche Questo!

        }
    }
    /*class PACKET_TIMEATTACK_END : Packet
    {
        public PACKET_TIMEATTACK_END(GameServer.Virtual_Objects.User.virtualUser Client, GameServer.Virtual_Objects.Room.virtualRoom Room)
        {
            //S=> 275910489 30053 3 219734 0 500220 -1 0
            newPacket(30053);
            addBlock(1);
            addBlock(1);



        }
    }*/
}