using System;
using ReBornWarRock_PServer.GameServer.Networking.Packets;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.Room;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;
using System.Diagnostics;

namespace ReBornWarRock_PServer.GameServer.Networking.Handlers
{
    class HANDLE_POWCAMP : PacketHandler
    {
        public override void Handle(virtualUser User)
        {

            virtualRoom currentRoom = User.Room;
            Stopwatch Timer = new Stopwatch();
            
            int Open = Convert.ToInt32(getBlock(0));
            int stageNum = Convert.ToInt32(getBlock(1));

            if (Open == 4 && stageNum == 0) //2° Stage
            {
                currentRoom.IsTimeOpenDoor = true;
                currentRoom.PowPlayer++;
            }

            if (Open == 4 && stageNum == 1) //3° Stage
            {
                currentRoom.IsTimeOpenDoor = true;
                currentRoom.PowPlayer++;
            }

            if (Open == 4 && stageNum == 2) //4° Stage Sembra che il client non manda questo pacchetto!
            {
                currentRoom.IsTimeOpenDoor = true;
                currentRoom.PowPlayer++;
            }

            if (Open == 6) //4° Stage Sembra che il client non manda questo pacchetto!
            {
                int SupplyFind = stageNum;
                bool found = currentRoom.SupplyBox.TryGetValue("SupplyNum" + stageNum, out SupplyFind);
                if (found == false)
                {
                User.SupplyTemp = stageNum;
                currentRoom.SupplyBox.Add("SupplyNum" + stageNum, stageNum);
                currentRoom.PowSupply++;
                }
            }
            if (currentRoom.Players.Count == currentRoom.PowSupply)
            {
                foreach (virtualUser Player in currentRoom.Players)
                {
                    string[] SnowWeapons = new string[] { "DF36", "DC34", "DF65", "DC93" };

                    string WeaponName = "";

                    Random random = new Random(); //1877

                    int event_weapon = random.Next(4);
                    string ItemCode = SnowWeapons[event_weapon];

                    switch (ItemCode)
                    {
                        case "DF36": WeaponName = "MP7A1_Snow_Camo"; break;
                        case "DC34": WeaponName = "FAMAS_Snow_Camo"; break;
                        case "DF65": WeaponName = "K1_Snow_Camo"; break;
                        case "DC93": WeaponName = "M4A1_Snow_Camo"; break;
                    }

                    int InventorySlot = -1;
                    for (int I = 0; I < 30; I++)
                    {
                        if (Player.Inventory[I] == null) { InventorySlot = I; break; }
                    }

                    Player.AddOutBoxItem(ItemCode, 7, 1);
                    Player.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> Congratulations! You achieved 50 kills by the Snow Kill Event!", 999, "NULL"));

                    if (InventorySlot <= 0)
                    {
                        Player.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> Your inventory is full!", 999, "NULL"));
                        Player.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> Please delete a weapon and play again.", 999, "NULL"));
                    }
                    else
                    {
                        Player.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> You got '" + WeaponName + "' for 7 days.", 999, "NULL"));
                        Player.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM >> Please relog.", 999, "NULL"));
                    }

                    User.send(new PACKET_SUPPLY_EVENT(User, currentRoom, ItemCode));
                    currentRoom.PowSupply = 0;
                    currentRoom.SupplyBox.Clear();
                    User.SupplyTemp = -1;
                }
            }
        }

    }
}


