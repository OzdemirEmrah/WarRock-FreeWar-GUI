using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ReBornWarRock_PServer.GameServer.Managers;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_CHARACTER_INFO : Packet
    {
        public PACKET_CHARACTER_INFO(ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser User)
        {
            newPacket(25088);
            addBlock(1);
            addBlock("ReBornWarRock");
            addBlock(User.SessionID);   // SessionID UDP
            addBlock(User.UserID);      // UserID
            addBlock(User.SessionID);   // SessionIDz UDP
            addBlock(User.Nickname);    // Nickname
            //Clan
            addBlock(User.ClanID);
            addBlock(User.ClanIconID);
            addBlock(User.ClanName);
            addBlock(-1);
            addBlock(User.ClanRank);
            //END Clan

            addBlock(0); // Premium Type

            // Level & Exp
            addBlock(LevelCalculator.getLevelforExp(User.Exp)); // Level for EXP
            addBlock(User.Exp); // Exp 
            addBlock(-1); // Unknown
            addBlock(0);
            addBlock(User.Dinar);
            addBlock(User.Kills); // Kills
            addBlock(User.Deaths); // Deaths
            addBlock(0);
            addBlock(0);
            addBlock(0);
            addBlock(0);
            addBlock(0);
            addBlock(User.getSlots()); //Slots Enabled
            
            // Player Equipment //
            for (int Class = 0; Class < 5; Class++)
            {
                StringBuilder ClassBuilder = new StringBuilder();

                for (int Slot = 0; Slot < 8; Slot++)
                {
                    ClassBuilder.Append(User.Equipment[Class, Slot]);
                    if (Slot != 7) ClassBuilder.Append(",");

                }
                addBlock(ClassBuilder.ToString());
            }
            
            addBlock(User.rebuildWeaponList());

            addBlock(User.MaxSlots);

            addBlock(User.CostumeE);
            addBlock(User.CostumeM);
            addBlock(User.CostumeS);
            addBlock(User.CostumeA);
            addBlock(User.CostumeH);

            // Costume Inventory //

            addBlock(User.rebuildCostumeList());
            addBlock(User.Premium);
            addBlock(1);
            addBlock(-1);
            addBlock(0);
            addBlock(0);
            addBlock(0);
            addBlock(1);
        }

        public PACKET_CHARACTER_INFO(int ErrorID)
        {
            newPacket(25088);
            addBlock(ErrorID);
        }
    }
}
