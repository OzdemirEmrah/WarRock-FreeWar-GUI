using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_ALPHABET_EVENT : Packet
    {
        public enum ErrorCodes
        {
            InventoryFull = 97070,
            ItemNotAvailable = -1
        }

        public PACKET_ALPHABET_EVENT(ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser User, ErrorCodes ErrCode)
        {
            newPacket(30997);
            addBlock((int)ErrCode);
        }

        public PACKET_ALPHABET_EVENT(ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser User, string ItemCode, long DurationTime)
        {
            newPacket(30997);
            addBlock(1);
            addBlock(ItemCode);
            addBlock(User.getSlots());
            addBlock((User.rebuildWeaponList()).ToString().Remove((User.rebuildWeaponList()).ToString().Length - 1));
            addBlock(User.rebuildCostumeList());
            /*
            base.AddBlock(1);
            base.AddBlock(ItemCode);
            base.AddBlock(Client.GetSlotString());
            base.AddBlock(Client.GetInventory());
            base.AddBlock(Client.GetEquippedCostumes()[0]);
            base.AddBlock(Client.GetEquippedCostumes()[1]);
            base.AddBlock(Client.GetEquippedCostumes()[2]);
            base.AddBlock(Client.GetEquippedCostumes()[3]);
            base.AddBlock(Client.GetEquippedCostumes()[4]);
            base.AddBlock(Client.GetCostumes());*/
        }
    }
}
