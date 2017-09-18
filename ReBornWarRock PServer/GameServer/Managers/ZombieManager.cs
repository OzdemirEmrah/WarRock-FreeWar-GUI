using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ReBornWarRock_PServer.GameServer
{
    internal enum ZombieType
    {
        Madman = 0,
        Maniac = 1,
        Grinders = 2,
        Grounders = 3,
        Heavys = 4,
        Growlers = 5,
        Lovers = 6,
        Handgeman = 7,
        Chariot = 8,
        Crushers = 9,
        Buster = 10,
        Crasher = 11,
        Envy = 12,
        Claw = 13,
        Bomber = 14,
        Defeder = 15,
        MadSoldier = 16,
        MadPrisoner = 17
    }

    class ZombieData
    {
        public int Type = 0;
        public string Name = null;
        public int Health = 0;
        public int Points = 1;
        public int Damage = 150;
        public bool SkillPoint = false;

        public ZombieData(int Type, string Name, int Health, int Points, int Damage, bool SkillPoint)
        {
            this.Health = Health;
            this.Name = Name;
            this.Points = Points;
            this.Damage = Damage;
            this.SkillPoint = SkillPoint;
            this.Type = Type;
        }
    }

    class ZombieManager
    {
        public static Dictionary<int, ZombieData> Datas = new Dictionary<int, ZombieData>();
         

        public static void Load()
        {
            try
            {
                Datas.Clear();

            int[] tableIDs = DB.runReadColumn("SELECT id FROM zombies;", 0, null);
           
            for (int i = 0; i < tableIDs.Length; i++)
            {
                string[] Datasa = DB.runReadRow("SELECT * FROM `zombies` WHERE id=" + tableIDs[i]);
                int type = Convert.ToInt32(Datasa[1]);
                string name = Datasa[2];
                int health = Convert.ToInt32(Datasa[3]);
                int points = Convert.ToInt32(Datasa[4]);
                int damage = Convert.ToInt32(Datasa[5]);
                int skillpoints = Convert.ToInt32(Datasa[6]);
                ZombieData Data = new ZombieData(type, name, health, points, damage, skillpoints > 0 ? true : false);
                if (!Datas.ContainsKey(type))
                {
                    Datas.Add(type, Data);
                }
                else
                {
                    Log.AppendText("Duplicate Zombie Type [" + type + "]");
                }
            }
                Log.AppendText("Succesful loaded [ " + ZombieManager.Datas.Count + " ] Zombies");
            }
            catch
            {
            }
        }
        

        public static ZombieData GetZombieDataByType(int Type)
        {
            if (Datas.ContainsKey(Type))
            {
                return (ZombieData)Datas[Type];
            }
            return null;
        }

        public static void GetZombieData(virtualZombie Zombie)
        {
            ZombieData Data = GetZombieDataByType(Zombie.Type);
            if (Data != null)
            {
                Zombie.name = Data.Name;
                Zombie.Health = Data.Health;
                Zombie.Points = Data.Points;
                Zombie.doDamage = Data.Damage;
                Zombie.givesSkillPoints = Data.SkillPoint;
            }
        }
    }
}