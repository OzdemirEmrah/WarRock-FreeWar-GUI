using ReBornWarRock_PServer.LoginServer.Virtual.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReBornWarRock_PServer.LoginServer.Packets
{
    class PacketHandler
    {
        private long Timestamp = 0;
        private int ID = 0;
        private string[] Blocks;
        private int currBlock = -1;

        public void set(long Timestamp, int ID, string[] Blocks)
        {
            this.Timestamp = Timestamp;
            this.ID = ID;
            this.Blocks = Blocks;
            currBlock = -1;
        }

        public string getNextBlock()
        {
            currBlock++;
            return Blocks[currBlock];
        }

        public string getBlock(int I)
        {
            currBlock = I;
            return Blocks[I];
        }

        public virtual void Handle(Virtual.User.User User)
        {
            /* Override */
        }

        public virtual void Handle(Server Server)
        {
            /* Override */
        }
    }
}
