using System;
using System.Collections;
using System.Text;

using ReBornWarRock_PServer.LoginServer.Packets;
using ReBornWarRock_PServer.LoginServer.Packets.List_Handle;
using LoginServer.Packets.List_Handle;

namespace ReBornWarRock_PServer.LoginServer.Docs
{
    class PacketManager
    {
        private static Hashtable _Packets = new Hashtable();

        public static void setup()
        {
            addPacket(8924, new HANDLE_LAUNCHER_INIT());
            addPacket(4352, new HANDLE_LOGIN());
            addPacket(4353, new HANDLE_NEW_NICKNAME());
            addPacket(4112, new HANDLE_UPDATE_CHECK());
            addPacket(99990, new SHANDLE_AUTH_SERVER());
            addPacket(99991, new SHANDLE_UPDATE_COUNT());
            addPacket(99992, new SHANDLE_BAN_PLAYER());
        }

        public static PacketHandler parsePacket(byte[] pBytes)
        {
            string packetStr = System.Text.Encoding.Default.GetString(pBytes);
            try
            {
                string[] tBlocks = packetStr.Split(Convert.ToChar(0x20));
                long Timestamp = long.Parse(tBlocks[0]);
                int PacketID = int.Parse(tBlocks[1]);

                if (_Packets.ContainsKey(PacketID))
                {
                    string[] _Blocks = new string[tBlocks.Length - 2];
                    Array.Copy(tBlocks, 2, _Blocks, 0, tBlocks.Length - 2);
                    PacketHandler pHandler = (PacketHandler)_Packets[PacketID];
                    pHandler.set(Timestamp, PacketID, _Blocks);
                    return pHandler;
                }
                else
                {
                        Log.AppendError("New packet ID found: " + PacketID);
                        Log.AppendError("Packet -> " + packetStr);

                }
            }
            catch { };

            return null;
        }



        private static void addPacket(int ID, PacketHandler Handler)
        {
            if (_Packets.ContainsKey(ID) == false)
            {
                _Packets.Add(ID, Handler);
            }
            else
            {
                Log.AppendError("Packet Manager already contains packetID: " + ID.ToString());
            }
        }
    }
}
