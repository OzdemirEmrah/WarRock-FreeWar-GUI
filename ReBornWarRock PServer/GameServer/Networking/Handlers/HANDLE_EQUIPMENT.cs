using System.Text;
using System;

using ReBornWarRock_PServer.GameServer.Networking.Packets;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;

namespace ReBornWarRock_PServer.GameServer.Networking.Handlers
{
    class HANDLE_EQUIPMENT : PacketHandler
    {
        public bool isDefaultWeapon(string ItemID, int Class, int TargetSlot)
        {
            if (ItemID == "DA02" && TargetSlot == 0 || (ItemID == "DB01" && TargetSlot == 1) || (ItemID == "DF01" && TargetSlot == 2 && (Class == 0 || Class == 1)) || (ItemID == "DG05" && TargetSlot == 2 && Class == 2) || (ItemID == "DC02" && TargetSlot == 2 && Class == 3) || (ItemID == "DJ01" && TargetSlot == 2 && Class == 4) || (ItemID == "DR01" && Class == 0 && TargetSlot == 3) || (ItemID == "DQ01" && Class == 1 && TargetSlot == 3) || (ItemID == "DN01" && (Class == 2 || Class == 3) && TargetSlot == 3) || (ItemID == "DL01" && Class == 4 && TargetSlot == 3))
                return true;
            return false;
        }

        public override void Handle(virtualUser User)
        {
            try
            {
                bool Equip = (getNextBlock() == "0" ? true : false);
                bool is6thSlot = (getBlock(0) == "3" ? true : false);
                int Class = Convert.ToInt32(getNextBlock());
                string ItemID = getBlock(4);
                int TargetSlot = Convert.ToInt32(getBlock(5));
                int Equip6th = Convert.ToInt32(getBlock(0));

                bool DefaultWeapon = isDefaultWeapon(ItemID, Class, TargetSlot);

                string InventoryCode = User.getInventoryID(ItemID);

                if (is6thSlot)
                {
                    if (TargetSlot == 5 && ItemID.Contains("-"))
                    {
                        string[] CodeInfo = ItemID.Split(Convert.ToChar("-"));

                        if (User.hasItem(CodeInfo[0]) == false || User.hasItem(CodeInfo[1]) == false) return;

                        string[] CodeInfoIDs = new string[2];
                        CodeInfoIDs[0] = User.getInventoryID(CodeInfo[0]);
                        CodeInfoIDs[1] = User.getInventoryID(CodeInfo[1]);

                        InventoryCode = string.Join("-", CodeInfoIDs);
                    }
                    else
                    {
                        if (ItemID.Contains("-"))
                        {
                            string[] CodeInfo = User.Equipment[Class, 5].Split(Convert.ToChar("-"));

                            if (CodeInfo[0] == InventoryCode)
                                InventoryCode = CodeInfo[1];
                            else if (CodeInfo[1] == InventoryCode)
                                InventoryCode = CodeInfo[0];
                        }
                    }

                    User.Equipment[Class, 5] = InventoryCode;

                    User.LoadRetails();

                    StringBuilder Items = new StringBuilder();
                    for (int I = 0; I < 8; I++)
                    {
                        Items.Append(User.Equipment[Class, I] + ",");
                    }
                    string EquipStr = Items.ToString().Remove(Items.ToString().Length - 1);

                    User.send(new PACKET_EQUIPMENT(Class, EquipStr));
                    DB.runQuery("UPDATE equipment SET class" + Class + "='" + EquipStr + "' WHERE ownerID=" + User.UserID.ToString());
                }
                else if (Equip)
                {
                    if (User.hasItem(ItemID) == false && DefaultWeapon == false) return;

                    if (DefaultWeapon) InventoryCode = ItemID;

                    bool hasItemEquipped = false;

                    for (int J = 0; J < 8; J++)
                    {
                        if (User.Equipment[Class, J] == InventoryCode) hasItemEquipped = true;
                    }

                    //Check for the 5th slot

                    if (TargetSlot == 4)
                    {
                        string[] SplitSlots = User.getSlots().Split(Convert.ToChar(","));
                        if (SplitSlots[0] != "T")
                            User.disconnect();
                    }

                    if (hasItemEquipped == false)
                    {
                        User.Equipment[Class, TargetSlot] = InventoryCode;

                        User.LoadRetails();

                        StringBuilder Items = new StringBuilder();
                        for (int I = 0; I < 8; I++)
                        {
                            Items.Append(User.Equipment[Class, I] + ",");
                        }
                        string EquipStr = Items.ToString().Remove(Items.ToString().Length - 1);

                        User.send(new PACKET_EQUIPMENT(Class, EquipStr));
                        DB.runQuery("UPDATE equipment SET class" + Class + "='" + EquipStr + "' WHERE ownerID=" + User.UserID.ToString());
                    }
                    else
                        User.send(new PACKET_EQUIPMENT(PACKET_EQUIPMENT.ErrorCode.AlreadyEquipped));
                }
                else
                {
                    int _TargetSlot = Convert.ToInt32(getBlock(3));
                    if (User.Equipment[Class, _TargetSlot] == InventoryCode || _TargetSlot == 5)
                    {
                        if (User.Equipment[Class, _TargetSlot] != "^")
                        {
                            User.Equipment[Class, _TargetSlot] = "^";

                            User.LoadRetails();

                            StringBuilder Items = new StringBuilder();
                            for (int I = 0; I < 8; I++)
                            {
                                Items.Append(User.Equipment[Class, I] + ",");
                            }
                            string EquipStr = Items.ToString().Remove(Items.ToString().Length - 1);

                            User.send(new PACKET_EQUIPMENT(Class, EquipStr));
                            DB.runQuery("UPDATE equipment SET class" + Class + "='" + EquipStr + "' WHERE ownerID=" + User.UserID.ToString());
                        }
                    }
                    else
                    {
                        //User.disconnect();
                    }
                }
            }
            catch (Exception ex) { /*Log.AppendError(ex.Message);*/ }
        }
    }
}
