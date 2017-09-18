using System.Text;
/*
 S=> 56925817 30976
 * 1 
 * T,F,F,T 
 * DA02,I008,I005,DR01,^,^,^,DF02 //class ingegnere
 * DA02,I008,DF01,DQ01,^,^,^,DQ02 //class medic
 * DA02,DB01,DG05,DN01,^,^,^,DB03 //class sniper
 * DA02,I008,DC02,DN01,I004,^,^,DO02 //class assalt
 * DA02,DB01,I006,DL01,^,^,^,DB03 //class heavy
 * 
 * CA01-5-0-1504051605-0,
 * CJ01-5-0-1504051605-0,
 * CI01-3-0-1504051605-0,
 * CB08-2-0-1305152200-30,
 * DC06-5-0-1504051605-0,
 * DF04-3-0-1504051605-0,
 * DJ21-5-0-1504051605-0,
 * CA04-3-0-1504281605-0,
 * DB03-5-2-1504061740-0,
 * CB99-4-0-1503291729-0,
 * DF13-5-0-1504030252-0,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^ 0 1006

 */
namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_KILL_EVENT : Packet
    {
        enum ErrorCodes
        {
            InventoryFull = 97070
        }
        public PACKET_KILL_EVENT(ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser User/*, int Count, string ItemCode/*, long Duration*/)
        {
            newPacket(30976);
            addBlock(1);
            addBlock(User.getSlots());
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
            //armi evento: k1() m4a1() 
            // Build Inventory //
            addBlock(User.rebuildWeaponList());
            addBlock(0);
            addBlock(1006);
            //addBlock(ItemCode);
            //addBlock(Duration);
            //addBlock(Count);
        }
    }
}
