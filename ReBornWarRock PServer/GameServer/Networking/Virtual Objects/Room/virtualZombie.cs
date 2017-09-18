using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ReBornWarRock_PServer.GameServer
{
    class virtualZombie
    {
        public int ID = 0;
        public string name = null;
        public int Health = 0;
        public int Points = 1;
        public int doDamage = 150;
        public bool givesSkillPoints = false;
        public int Type = 0;
        public int FollowUser = -1;
        public int timestamp = 0;
        public int RespawnTick;
        public bool Death = false;

        public virtualZombie(int ID, int FollowUser, int timestamp, int Type)
        {
            this.ID = ID;
            this.FollowUser = FollowUser;
            this.timestamp = timestamp;
            this.Type = Type;
            this.RespawnTick = 0;
            this.Health = 0;
        }

        public void Reset()
        {
            ZombieManager.GetZombieData(this);
        }
    }
}