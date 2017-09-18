using System;
namespace ReBornWarRock_PServer.GameServer.Managers
{
    internal class Item
    {
        public int Level = 1;
        public int BuyType = 1;
        private int[] Price = new int[6];
        private int[] Cash = new int[6];
        private int[] EA = new int[4]
    {
      1,
      10,
      30,
      60
    };
        public int ID;
        public string Code;
        public string Name;
        public string Personal;
        public string Surface;
        public int Damage;
        public bool Premium;
        public bool Buyable;

        public Item(int ID, string Code, string Name, string Price, string Cash, int Damage, int BuyType, string Surface, string Personal, int Level, bool Premium, bool Buyable)
        {
            try
            {
                this.ID = ID;
                for (int index = 0; index < 6; ++index)
                {
                    this.Price[index] = -1;
                    this.Cash[index] = -1;
                }
                this.Code = Code;
                this.Name = Name;
                this.BuyType = BuyType;
                string[] strArray1 = Price.Split(',');
                for (int index = 0; index < strArray1.Length; ++index)
                    this.Price[index] = Convert.ToInt32(strArray1[index]);
                string[] strArray2 = Cash.Split(',');
                for (int index = 0; index < strArray2.Length; ++index)
                    this.Cash[index] = Convert.ToInt32(strArray2[index]);
                this.Damage = Damage;
                if (Surface != null)
                    this.Surface = Surface;
                if (Personal != null)
                    this.Personal = Personal;
                this.Level = Level;
                this.Premium = Premium;
                this.Buyable = Buyable;
            }
            catch
            {
            }
        }

        public int getPrice(int Type)
        {
            return this.Price[Type];
        }

        public int getCashPrice(int Type)
        {
            return this.Cash[Type];
        }

        public int GetEACount(int Type)
        {
            return this.EA[Type];
        }
    }
}
