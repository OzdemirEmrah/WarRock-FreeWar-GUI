using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.Room;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_ZOMBIE_SPAWN : Packet
    {
        public PACKET_ZOMBIE_SPAWN(int Slot, int FollowUser, int Place, int ZombieType, int health)
        {
            newPacket(13432);
            addBlock(Slot);
            addBlock(FollowUser);
            addBlock(ZombieType);
            addBlock(Place);
            addBlock(health);
        }
    }

    class PACKET_ZOMBIR_CHANGE_ENEMY : Packet
    {
        public PACKET_ZOMBIR_CHANGE_ENEMY(virtualRoom Room, int SlotID)
        {
            int A = 0;
            List<virtualZombie> list = Room.ZombieFollowers(SlotID);
            Room.ZombieFollowers(SlotID);//ci devo mettere roomslot di chi è morto
            Room.CheckAliveRoomSlot();
            newPacket(13433);
            addBlock(Room.Players.Count);//conto dei player in room
            addBlock(list.Count);//zombi che seguivano il Roomslot uscito

              foreach (virtualZombie z in list)
              {
                addBlock(z.ID);
                
                if(Room.ListCheckAliveRoomSlot.Count > 1)
                {A++;}
                if (Room.ListCheckAliveRoomSlot.Count == A)
                {A = 0;}

                addBlock(Room.ListCheckAliveRoomSlot[A]);

            }
        }
    }


    /* class PACKET_ZOMBIR_CHANGE_ENEMY : Packet
     {
         public PACKET_ZOMBIR_CHANGE_ENEMY(virtualRoom Room, int RoomSlot)
         {
             List<virtualZombie> list = Room.ZombieFollowers(RoomSlot);
             newPacket(13433);
             addBlock(RoomSlot);
             addBlock(list.Count);
             foreach (virtualZombie z in list)
             {
                 addBlock(z.ID);
                 addBlock(Room.RoomMasterSlot);
             }
         }
     }*/

    class PACKET_SPAWN_ZOMBIE_MADMAN : Packet
    {
        public PACKET_SPAWN_ZOMBIE_MADMAN(virtualRoom Room, int Slot)
        {
            newPacket(13432);
            addBlock(Slot);
            addBlock(0);
            addBlock(0);
            addBlock(new System.Random().Next(1, 23));
            addBlock(150);
        }
    }

    class PACKET_SPAWN_ZOMBIE_MANIAC : Packet
    {
        public PACKET_SPAWN_ZOMBIE_MANIAC(virtualRoom Room, int Slot)
        {
            newPacket(13432);
            addBlock(Slot);
            addBlock(0);
            addBlock(1);
            addBlock(new System.Random().Next(1, 23));
            addBlock(150);
        }
    }

    class PACKET_SPAWN_ZOMBIE_GRINDER : Packet
    {
        public PACKET_SPAWN_ZOMBIE_GRINDER(virtualRoom Room, int Slot)
        {
            newPacket(13432);
            addBlock(Slot);
            addBlock(0);
            addBlock(2);
            addBlock(new System.Random().Next(1, 23));
            addBlock(200);
        }
    }

    class PACKET_SPAWN_ZOMBIE_GROUNDER : Packet
    {
        public PACKET_SPAWN_ZOMBIE_GROUNDER(virtualRoom Room, int Slot)
        {
            newPacket(13432);
            addBlock(Slot);
            addBlock(0);
            addBlock(3);
            addBlock(new System.Random().Next(1, 23));
            addBlock(200);
        }
    }

    class PACKET_SPAWN_ZOMBIE_GROWLER : Packet
    {
        public PACKET_SPAWN_ZOMBIE_GROWLER(virtualRoom Room, int Slot)
        {
            newPacket(13432);
            addBlock(Slot);
            addBlock(0);
            addBlock(4);
            addBlock(new System.Random().Next(1, 23));
            addBlock(250);
        }
    }

    class PACKET_SPAWN_ZOMBIE_HEAVY : Packet
    {
        public PACKET_SPAWN_ZOMBIE_HEAVY(virtualRoom Room, int Slot)
        {
            newPacket(13432);
            addBlock(Slot);
            addBlock(0);
            addBlock(5);
            addBlock(new System.Random().Next(1, 23));
            addBlock(450);
        }
    }

    class PACKET_SPAWN_ZOMBIE_LOVER : Packet
    {
        public PACKET_SPAWN_ZOMBIE_LOVER(virtualRoom Room, int Slot)
        {
            newPacket(13432);
            addBlock(Slot);
            addBlock(0);
            addBlock(6);
            addBlock(new System.Random().Next(1, 23));
            addBlock(800);
        }
    }

    class PACKET_SPAWN_ZOMBIE_HANDGEMAN : Packet
    {
        public PACKET_SPAWN_ZOMBIE_HANDGEMAN(virtualRoom Room, int Slot)
        {
            newPacket(13432);
            addBlock(Slot);
            addBlock(0);
            addBlock(7);
            addBlock(new System.Random().Next(1, 23));
            addBlock(200);
        }
    }

    class PACKET_SPAWN_ZOMBIE_CHARIOT : Packet
    {
        public PACKET_SPAWN_ZOMBIE_CHARIOT(virtualRoom Room, int Slot)
        {
            newPacket(13432);
            addBlock(Slot);
            addBlock(0);
            addBlock(8);
            addBlock(new System.Random().Next(1, 23));
            addBlock(9000);
        }
    }

    class PACKET_SPAWN_ZOMBIE_CRUSHER : Packet
    {
        public PACKET_SPAWN_ZOMBIE_CRUSHER(virtualRoom Room, int Slot)
        {
            newPacket(13432);
            addBlock(Slot);
            addBlock(0);
            addBlock(9);
            addBlock(new System.Random().Next(1, 23));
            addBlock(10000);
        }
    }


    class PACKET_SPAWN_ZOMBIE_BUSTER : Packet
    {
        public PACKET_SPAWN_ZOMBIE_BUSTER(virtualRoom Room, int Slot)
        {
            newPacket(13432);
            addBlock(Slot);
            addBlock(0);
            addBlock(10);
            addBlock(new System.Random().Next(1, 23));
            addBlock(20);
        }
    }

    class PACKET_SPAWN_ZOMBIE_CRASHER : Packet
    {
        public PACKET_SPAWN_ZOMBIE_CRASHER(virtualRoom Room, int Slot)
        {
            newPacket(13432);
            addBlock(Slot);
            addBlock(0);
            addBlock(11);
            addBlock(new System.Random().Next(1, 23));
            addBlock(10000);
        }
    }

    class PACKET_SPAWN_ZOMBIE_ENVY : Packet // NOT ADDED
    {
        public PACKET_SPAWN_ZOMBIE_ENVY(virtualRoom Room, int Slot)
        {
            newPacket(13432);
            addBlock(Slot);
            addBlock(0);
            addBlock(12);
            addBlock(new System.Random().Next(1, 23));
            addBlock(10);
        }
    }

    class PACKET_SPAWN_ZOMBIE_CLAW : Packet
    {
        public PACKET_SPAWN_ZOMBIE_CLAW(virtualRoom Room, int Slot)
        {
            newPacket(13432);
            addBlock(Slot);
            addBlock(0);
            addBlock(13);
            addBlock(new System.Random().Next(1, 23));
            addBlock(10);
        }
    }

    class PACKET_SPAWN_ZOMBIE_BOMBER : Packet
    {
        //S=> 215583432 13432 25 0 14 4 30000
        public PACKET_SPAWN_ZOMBIE_BOMBER(virtualRoom Room, int Slot)
        {
            newPacket(13432);
            addBlock(Slot);
            addBlock(0);
            addBlock(14);
            addBlock(4);
            addBlock(30000);
        }
    }

    class PACKET_SPAWN_ZOMBIE_DEFENDER : Packet
    {
        //S=> 250078626 13432 4 0 15 6 300000
        public PACKET_SPAWN_ZOMBIE_DEFENDER(virtualRoom Room, int Slot)
        {
            newPacket(13432);
            addBlock(Slot);
            addBlock(0);
            addBlock(15);
            addBlock(6);
            addBlock(300000);
        }
    }

    class PACKET_SPAWN_ZOMBIE_BREAKER : Packet
    {
        //S=> 275918502 13432 4 0 16 0 350000
        public PACKET_SPAWN_ZOMBIE_BREAKER(virtualRoom Room, int Slot)
        {
            newPacket(13432);
            addBlock(Slot);
            addBlock(0);
            addBlock(16);
            addBlock(0);
            addBlock(100000);
            //addBlock(350000);
        }
    }

    class PACKET_SPAWN_ZOMBIE_MADSOLDIER : Packet
    {
        public PACKET_SPAWN_ZOMBIE_MADSOLDIER(virtualRoom Room, int Slot)
        {
            newPacket(13432);
            addBlock(Slot);
            addBlock(0);
            addBlock(17);
            addBlock(new System.Random().Next(1, 23));
            addBlock(10);
        }
    }

    class PACKET_SPAWN_ZOMBIE_MADPRISONER : Packet
    {
        public PACKET_SPAWN_ZOMBIE_MADPRISONER(virtualRoom Room, int Slot)
        {
            newPacket(13432);
            addBlock(Slot);
            addBlock(0);
            addBlock(18);
            addBlock(new System.Random().Next(1, 23));
            addBlock(10);
        }
    }
    class PACKET_SPAWN_ZOMBIE_UNKNOW : Packet
    {
        public PACKET_SPAWN_ZOMBIE_UNKNOW(virtualRoom Room, int Slot)
        {
            newPacket(13432);
            addBlock(Slot);
            addBlock(0);
            addBlock(19);
            addBlock(new System.Random().Next(1, 23));
            addBlock(10);
        }
    }
    class PACKET_SPAWN_ZOMBIE_UNKNOW1 : Packet
    {
        public PACKET_SPAWN_ZOMBIE_UNKNOW1(virtualRoom Room, int Slot)
        {
            newPacket(13432);
            addBlock(Slot);
            addBlock(0);
            addBlock(20);
            addBlock(new System.Random().Next(1, 23));
            addBlock(10);
        }
    }
    class PACKET_SPAWN_ZOMBIE_LADY : Packet
    {
        public PACKET_SPAWN_ZOMBIE_LADY(virtualRoom Room, int Slot)
        {
            newPacket(13432);
            addBlock(Slot);
            addBlock(0);
            addBlock(21);
            addBlock(new System.Random().Next(1, 23));
            addBlock(10);
        }
    }
    class PACKET_SPAWN_ZOMBIE_UNKNOW3 : Packet
    {
        public PACKET_SPAWN_ZOMBIE_UNKNOW3(virtualRoom Room, int Slot)
        {
            newPacket(13432);
            addBlock(Slot);
            addBlock(0);
            addBlock(22);
            addBlock(new System.Random().Next(1, 23));
            addBlock(10);
        }
    }
}