using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;
using ReBornWarRock_PServer.GameServer.Networking.Handlers;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class SP_RoomHackMission : Packet
    {
        public SP_RoomHackMission(int rs, int Percentage, int Type, int Base, int value)
        {
            //29985 0 0 0 2 0 14 -1 0 
            newPacket(29985);
            addBlock(0);
            addBlock(rs);
            addBlock(value);
            addBlock(Type);
            addBlock(Base);
            addBlock(Percentage);
            addBlock(-1);
            addBlock(0);
            //250048643 29985 0 -1 1 5 -1 0 -1 0 Fine Radio!

        }
    }
    class SP_RoomHackMission1 : Packet
    {
        public SP_RoomHackMission1(int rs, int Percentage, int Type, int Base)
        {
            //29985 0 0 0 2 0 14 -1 0 
            newPacket(29985);
            addBlock(0);
            addBlock(rs);
            addBlock(0);
            addBlock(Type);
            addBlock(Base);
            addBlock(Percentage);
            addBlock(-1);
            addBlock(0);
        }
    }

}