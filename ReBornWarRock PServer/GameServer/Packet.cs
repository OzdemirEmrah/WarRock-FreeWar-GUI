using ReBornWarRock_PServer.GameServer.Virtual_Objects.Room;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ReBornWarRock_PServer.GameServer
{
    class Packet
    {
        ~Packet()
        {
            GC.Collect();
        }
        [DllImport("winmm.dll")]
        public static extern long timeGetTime();

        private int PacketID;
        private string[] blocks = new string[0];
        private bool Client = false;

        public byte[] getBytes()
        {
            try
            {
                string str = string.Concat(new object[4]
                {
         Environment.TickCount,
         " ",
         this.PacketID,
         " "
                });
                for (int index = 0; index < this.blocks.Length; ++index)
                    str = str + this.blocks[index].Replace(' ', '\x001D') + " ";
                if (!str.Contains("25600") && Structure.PacketView)
                {
                    Log.WritePackets("S=>" + str);
                }
                return this.Crypt(Encoding.GetEncoding("Windows-1250").GetBytes((str).ToString() + ' ' + '\n'));
            }
            catch
            {
                return (byte[])null;
            }
        }
        protected void newPacket(int PacketID)
        {
            this.PacketID = PacketID;
        }

        protected void setClient(bool C)
        {
            this.Client = C;
        }

        protected void addBlock(object Block)
        {
            Array.Resize(ref blocks, blocks.Length + 1);
            blocks[blocks.Length - 1] = Convert.ToString(Block);
        }

        public byte[] Crypt(byte[] tTemp)
        {
            for (int index = 0; index < tTemp.Length; ++index)
                tTemp[index] = Convert.ToByte((int)tTemp[index] ^ 91);
            return tTemp;
        }

        protected void Fill(object block, int length)
        {
            for (int i = 0; i < length; i++)
            {
                addBlock(block);
            }
        }


        public void addRoomInfo(virtualRoom Room)
        {
            //|4 1 1 0 Let'spl 0 16 1 12 3 2 0 0 4 1 0 1 0 0 0 1 0 66 0 -1 
            //18 1 1 0 ClanWar 0 16 1 12 3 2 0 0 4 1 0 1 1 0 0 1 0 54 0 99999999 20514 1001001 Montana -1 -1 ? 
            addBlock(Room.ID);
            addBlock(Room.RoomStatus);
            addBlock(Room.RoomStatus);
            addBlock(Room.RoomMasterSlot);
            addBlock(Room.Name);
            addBlock(Room.EnablePassword);
            addBlock(Room.MaxPlayers);
            addBlock(Room.PlayerCount);
            addBlock(Room.MapID);
            if (Room.Channel == 3)
                addBlock(Room.ZombieDifficulty);
            else
                addBlock(Room.Rounds);
            addBlock(Room.Rounds);
            addBlock(Room.TimeLimit);
            addBlock(Room.Mode);
            addBlock(4);
            addBlock(Room.isJoinAble() ? 1 : 0); // 0 = unjoinable(grey room) // Once We are almost over
            addBlock(0);
            addBlock(Room.NewMode);
            addBlock(Room.SubNewMode);
            addBlock(Room.SuperMaster ? 1 : 0); // 1 = Room has Supermaster
            addBlock(Room.RoomType);
            addBlock(Room.LevelLimit);
            addBlock(Room.PremiumOnly);
            addBlock(Room.VoteKick);
            addBlock(Room.AutoStart ? 1 : 0); // AutoStart
            addBlock(0);
            addBlock(Room.Ping);
            if (Room.RoomType == 1)
            {
                virtualClan Clan1 = ClanManager.getClan(Room.getClanID(0));
                virtualClan Clan2 = ClanManager.getClan(Room.getClanID(1));
                addBlock(1);
                if (Clan1 == null)
                {
                    addBlock(-1);
                    addBlock(-1);
                    addBlock("?");
                }
                else
                {
                    addBlock(Clan1.clanID);
                    addBlock(Clan1.clanIconID);
                    addBlock(Clan1.clanName);
                }
                if (Clan2 == null)
                {
                    addBlock(-1);
                    addBlock(-1);
                    addBlock("?");
                }
                else
                {
                    addBlock(Clan2.clanID);
                    addBlock(Clan2.clanIconID);
                    addBlock(Clan2.clanName);
                }
            }
            else
            {
                addBlock(-1);
            }
        }
    }
}