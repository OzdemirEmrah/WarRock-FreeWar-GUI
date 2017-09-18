using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;
using ReBornWarRock_PServer.GameServer.Managers;

namespace ReBornWarRock_PServer.GameServer.Networking.Handlers
{
    class HANDLE_COSTUME_EQUIPMENT : PacketHandler
    {
        string getCostume(virtualUser User, int Class)
        {
            switch(Class)
            {
                case 0: return User.CostumeE.ToString();                 
                case 1: return User.CostumeM.ToString();
                case 2: return User.CostumeS.ToString();
                case 3: return User.CostumeA.ToString();
                case 4: return User.CostumeH.ToString(); 
            }
            return null;
        }

        void updateCostume(virtualUser User, int Class, string Code)
        {
            switch (Class)
            {
                case 0: User.CostumeE = Code; break;
                case 1: User.CostumeM = Code; break;
                case 2: User.CostumeS = Code; break;
                case 3: User.CostumeA = Code; break;
                case 4: User.CostumeH = Code; break;
            }
        }

        string getDefaultClass(int Class)
        {
            switch (Class)
            {
                case 0: return "BA01";
                case 1: return "BA02";
                case 2: return "BA03";
                case 3: return "BA04";
                case 4: return "BA05";
            }
            return null;
        }

        public override void Handle(virtualUser User)
        {
                //Nigga will invade the world.
                bool Equip = (getBlock(0) == "0") ? true : false;
                int Class = Convert.ToInt32(getBlock(1));
                string Code = getBlock(4);
                int WhereToPlace = Convert.ToInt32(getBlock(5));

                string Costume = getCostume(User, Class);
                
                Item Item = ItemManager.getItem(Code);
                {
                    if (Equip == true)
                    {
                        string[] Placment = Costume.Split(new char[] { ',' });
                        if (Code.Contains("BA"))
                        {
                            for (int I = 0; I < Placment.Length; I++)
                            {
                                Placment[I] = "^";
                            }
                            Placment[0] = Code;
                        }
                        else
                            Placment[WhereToPlace] = Code;
                        string newCostumeParts = string.Join(",", Placment);
                        Code = newCostumeParts;
                        updateCostume(User, Class, Code);
                        DB.runQuery("UPDATE users_costumes SET class_" + Class + "='" + Code + "' WHERE ownerid='" + User.UserID + "'");
                    }
                    else
                    {
                        string[] Placment = Costume.Split(new char[] { ',' });
                        if (Code.Contains("BA"))
                        {
                            for (int I = 0; I < Placment.Length; I++)
                            {
                                Placment[I] = "^";
                            }
                            Placment[0] = getDefaultClass(Class);
                        }
                        else
                            Placment[WhereToPlace] = "^";
                        string newCostumeParts = string.Join(",", Placment);
                        Code = newCostumeParts;
                        updateCostume(User, Class, Code);
                        DB.runQuery("UPDATE users_costumes SET class_" + Class + "='" + Code + "' WHERE ownerid='" + User.UserID + "'");
                    }
                }
                User.send(new Packets.PACKET_COSTUME_EQUIPMENT(User, Class, Code));
                User.reloadCash();
        }
    }
}
