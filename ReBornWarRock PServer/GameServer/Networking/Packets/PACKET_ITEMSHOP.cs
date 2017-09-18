namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_ITEMSHOP : Packet
    {
        public enum ErrorCodes
        {
            Success = 1,
            CannotBeBougth = 97020,
            PremiumOnly = 98010,
            NotEnoughDinar = 97040,
            LevelLow = 97050,
            NotEnoughCash = 97092,
            InventoryFull = 97070,
            MaximumTimeLimit = 97100,
        }

        public PACKET_ITEMSHOP(ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser User)
        {
            newPacket(30208);
            addBlock(1);
            addBlock(1110);
            addBlock(-1);
            addBlock(3);
            addBlock(4);
            addBlock(User.rebuildWeaponList());
            addBlock(User.Dinar);
            addBlock(User.getSlots()); // Slot Enabled
        }

        public PACKET_ITEMSHOP(ErrorCodes Err, string unknownobj)
        {
            newPacket(30208);
            addBlock((int)Err);
            addBlock(unknownobj);
        }
    }
}
