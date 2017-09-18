using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.Room;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class SP_ROOM_INFO : Packet
    {
        public SP_ROOM_INFO(int tType, params object[] Params)
        {
            newPacket(30000);
            addBlock(1);
            for (int i = 0; i < Params.Length; i++)
            {
                addBlock(Params[i]);
                if (i == 2)
                    addBlock(tType);
            }
        }
    }

    class RoomDataNewRound : Packet
    {
        public RoomDataNewRound(Virtual_Objects.Room.virtualRoom Room, int WinningTeam, bool Prepare)
        {
            int Code = (Prepare == true ? 6 : 5);
            newPacket(30000);
            addBlock(1);//Success
            addBlock(-1);
            addBlock(Room.ID);
            addBlock(1);
            addBlock(Code);
            addBlock(0);
            addBlock(1);
            addBlock(WinningTeam);
            addBlock(Room.cDerbRounds);
            addBlock(Room.cNiuRounds);
            addBlock(5);
            addBlock(0);
            addBlock(92);
            addBlock(-1);
            addBlock(0);
            addBlock(0);
            addBlock(1200000);
            addBlock(-900000);
            addBlock(1200000);
            addBlock("778.0000");
            addBlock("32.0000");
            addBlock("1438.0000");
            addBlock("49.0000");
            addBlock("-275.0000");
            addBlock("-108.0000");
            addBlock(0);
            addBlock(0);
            addBlock("DS05");
        }
    }

    class InitializeNewRound : Packet
    {
        public InitializeNewRound(Virtual_Objects.Room.virtualRoom Room)
        {
            newPacket(30000);
            addBlock(1); // Success
            addBlock(-1); // ??
            addBlock(Room.ID); // Room ID
            addBlock(2);
            addBlock(403); // Sub id?
            addBlock(0);
            addBlock(1);
            addBlock(3);
            addBlock(363);
            addBlock(0);
            addBlock(1);
            addBlock(0);
            addBlock(0);
            addBlock(0);
            addBlock(0);
        }
    }
    class PACKET_ROOM_DATA : Packet
    {
        public PACKET_ROOM_DATA(ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser User, params object[] Params)
        {
            newPacket(30000);
            addBlock(1);
            for (int i = 0; i < Params.Length; i++)
            {
                addBlock(Params[i]);
            }
        }
    }
}