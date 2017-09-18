using System;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.Room;
namespace ReBornWarRock_PServer.GameServer
{
    class PacketHandler : IDisposable
    {
        ~PacketHandler()
        {
            GC.Collect();
        }
        private long Timestamp = 0;
        private int ID = 0;
        public string[] Blocks;
        private int currBlock = -1;
        public bool sendPacket = false;

        public void set(long Timestamp, int ID, string[] Blocks)
        {
            this.Timestamp = Timestamp;
            this.ID = ID;
            this.Blocks = Blocks;
            this.currBlock = -1;
        }
        public void FillData(int subtype, string[] Blocks)
        {
            this.ID = subtype;
            this.Blocks = Blocks;
            this.sendPacket = false;
        }
        public string getNextBlock()
        {
            this.currBlock++;
            return this.Blocks[currBlock];
        }

        public string[] getAllBlocks()
        {
            return this.Blocks;
        }

        public string getBlock(int I)
        {
            this.currBlock = I;
            return this.Blocks[I];
        }
        public virtual void Handle(virtualUser User)
        {
            /* Override */
        }

        public virtual void Handle1(virtualUser User, virtualRoom Room)
        {
            /* Override */
        }

        public virtual void HandlePacket(virtualUser User, PacketHandler Packet)
        {
            /* Override */
        }
        public virtual void HandleSC(ServerClient SClient)
        {
            /* Override */
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
