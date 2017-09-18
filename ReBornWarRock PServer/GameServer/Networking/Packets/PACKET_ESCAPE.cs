using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_ESCAPE_MODE1 : Packet
    {
        public PACKET_ESCAPE_MODE1(int UID)
        {
            // S=> 831664759 31505 1 1 12 1 GetSide (0)
            // S=> 835753467 31505 1 1 0 0 GetSide (1)

            newPacket(31505);
            addBlock(1);
            addBlock(UID);
            addBlock(12);
            addBlock(1);
        }

    }

    class PACKET_ESCAPE_MODE2 : Packet
    {
        public PACKET_ESCAPE_MODE2()
        {
            // S=> 831664759 31505 1 1 12 1 GetSide (0)
            // S=> 835753467 31505 1 1 0 0 GetSide (1)

            newPacket(31505);
            addBlock(1);
            addBlock(1);
            addBlock(0);
            addBlock(0);
        }
    }


    class PACKET_ESCAPE_MODE3 : Packet
    {
        public PACKET_ESCAPE_MODE3()
        {
            //834515335 31505 1 0 2  0 0 12 1 1 0 0 1 12 32 3000 8000 8000 9000 12000 10000 10000 9000 12000 14000 3000 3000 14000 25000 10000 10000 10000 10000 10000 1000 10000 10000 10000 10000 8000 8000 8000 8000 8000 10000 5000 4000000

            newPacket(31505);
            addBlock(1);
            addBlock(0);
            addBlock(2);
            addBlock("");
            addBlock(0);
            addBlock(0);
            addBlock(12);
            addBlock(1);
            addBlock(1);
            addBlock(0);
            addBlock(0);
            addBlock(1);
            addBlock(12);
            addBlock(32);
            addBlock(3000);
            addBlock(8000);
            addBlock(8000);
            addBlock(9000);
            addBlock(12000);

        }
    }

    class PACKET_ESCAPE_MODE : Packet
    {
        public PACKET_ESCAPE_MODE()
        {
            // S=> 834487238 31505 1 0 2  0 0 12 1 0

            newPacket(31505);
            addBlock(1);
            addBlock(0);
            addBlock(2);
            addBlock("");
            addBlock(0);
            addBlock(0);
            addBlock(12);
            addBlock(1);
            addBlock(0);
        }

    }

    class PACKET_1 : Packet
    {
        public PACKET_1()
        {
            //1° = 834508912 31507 0 12 1 6 0 0 -1 0

            newPacket(31507);
            addBlock(0);
            addBlock(12);
            addBlock(1);
            addBlock(1);
            addBlock(6);
            addBlock(0);
            addBlock(0);
            addBlock(-1);
            addBlock(0);

        }

    }

    class PACKET_2 : Packet
    {
        public PACKET_2(int num)
        {
            //S=> 834541187 31507 0 12 1 5 0 0 -1 0
            //    833363252 31507 0 12 1 6 0 0 -1 0

            newPacket(31507);
            addBlock(0);
            addBlock(12);
            addBlock(1);
            addBlock(num);
            addBlock(0);
            addBlock(0);
            addBlock(-1);
            addBlock(0);
        }

    }

    class PACKET_HACKING_ESCAPE : Packet
    {
        public PACKET_HACKING_ESCAPE(int Id, int count, int Timer)
        {
            //S=> 834837857 31507 0 0 0 2 0 10 -1 0

            newPacket(31507);
            addBlock(0);
            addBlock(0);
            addBlock(0);
            addBlock(Id);
            addBlock(0);
            addBlock(count);
            addBlock(-1);
            addBlock(Timer);

        }
    }

    class PACKET_3 : Packet
    {
        public PACKET_3(int Num4, Virtual_Objects.Room.virtualRoom Room)
        {
            //S=> 834542789 31507 0 0 1 6 0 0 -1 0

            newPacket(31507);
            addBlock(0);
            addBlock(0);
            addBlock(Room.ID);
            addBlock(Num4);
            addBlock(0);
            addBlock(0);
            addBlock(-1);
            addBlock(0);

        }

    }
}
