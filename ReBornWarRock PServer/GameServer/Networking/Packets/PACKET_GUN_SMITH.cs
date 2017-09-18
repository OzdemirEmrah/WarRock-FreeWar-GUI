using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.Room;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    internal class PACKET_GUN_SMITH : Packet
    {
        public PACKET_GUN_SMITH(virtualUser User, string item, PACKET_GUN_SMITH.WonType type)
        {
            this.newPacket(30995);
            this.addBlock(1);
            this.addBlock(item);
            this.addBlock((byte)type);
            this.addBlock(User.Dinar);
            this.addBlock(User.Cash);
            this.addBlock(User.rebuildWeaponList());
            this.addBlock(User.Equipment);
        }

        public enum WonType
        {
            Lose = 0,
            Normal = 1,
            Rare = 2,
        }
    }
}
