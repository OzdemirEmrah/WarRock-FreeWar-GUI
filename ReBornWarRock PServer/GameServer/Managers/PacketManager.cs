using System;
using System.Collections;
using System.Net;
using System.Text;
using ReBornWarRock_PServer.GameServer.Networking;
using ReBornWarRock_PServer.GameServer.Networking.Handlers;

namespace ReBornWarRock_PServer.GameServer.Managers
{
    class PacketManager
    {
        private static Hashtable _Packets = new Hashtable();

        ~PacketManager()
        {
            GC.Collect();
        }

        public static void setup()
        {
            _Packets = new Hashtable();

            //addPacket(99989, new SHANDLE_CONNECT()); //checked for loginserver
            addPacket(24576, new HANDLE_LOGOUT());
            addPacket(24832, new HANDLE_WELCOME_PACKET()); // 24832
            addPacket(25088, new HANDLE_CHARACTER_INFO());
            addPacket(29472, new HANDLE_QUICK_JOIN());
            addPacket(25600, new HANDLE_PING());
            addPacket(28673, new HANDLE_CHANNEL_SWITCH());
            addPacket(29184, new HANDLE_ROOM_LIST());
            addPacket(25588, new HANDLE_TRINITYGUARD_CHECK());
            addPacket(29488, new HANDLE_SPECTATE_ROOM());
            addPacket(29440, new HANDLE_ROOM_CREATION());
            addPacket(29456, new HANDLE_ROOM_JOIN());
            addPacket(29504, new HANDLE_ROOM_LEAVE());
            addPacket(29505, new HANDLE_ROOM_KICK());
            addPacket(29520, new HANDLE_ROOM_INVITE());
            addPacket(29696, new HANDLE_CHAT());
            addPacket(29970, new HANDLE_EQUIPMENT());
            addPacket(29984, new HANDLE_ROOM_BOMB());
            addPacket(30000, new HANDLE_ROOM_DATA());//rifatto
            addPacket(30032, new HANDLE_SCORE_BOARD());
            addPacket(30208, new HANDLE_ITEMSHOP());
            addPacket(29201, new HANDLE_ROOM_UPDATE()); // change settings waitingroom
            addPacket(28960, new HANDLE_USER_LIST());
            addPacket(25605, new HANDLE_COUPON_EVENT());
            addPacket(25606, new HANDLE_COUPON_BUY());
            addPacket(30257, new HANDLE_LUCKY_SHOT());
            addPacket(30258, new HANDLE_LUCKY_SHOT_WIN());
            addPacket(30259, new HANDLE_LUCKY_SHOT());
            addPacket(30224, new HANDLE_DELETE_WEAPON());
            addPacket(30720, new HANDLE_CREDITS());
            addPacket(30225, new HANDLE_DELETE_COSTUME());
            addPacket(29971, new HANDLE_COSTUME_EQUIPMENT());
            addPacket(30209, new HANDLE_COSTUME_BUY());
            addPacket(30752, new HANDLE_OUTBOX());
            addPacket(31490, new HANDLE_MULTIPLAYER_ZOMBIE());
            addPacket(29969, new HANDLE_LEAVE_VEHICLE());
            addPacket(31492, new HANDLE_SKILL_POINT());
            addPacket(30993, new HANDLE_LOGIN_EVENT());
            addPacket(26384, new HANDLE_CLAN());
            addPacket(30272, new HANDLE_CARE_PACKAGE());
            addPacket(30273, new HANDLE_CARE_PACKAGE_WIN());
            addPacket(30992, new HANDLE_SHOP_COUPON());
            addPacket(26464, new HANDLE_CLAN_RANKING());
            addPacket(30053, new HANDLE_POWCAMP());
            addPacket(29985, new HANDLE_RADIO_TIME());
            addPacket(31507, new HANDLE_ESCAPE_MODE());
            addPacket(32256, new HANDLE_MESSENGER());
            addPacket(30995, new HANDLE_GUN_SMITH());
            //addPacket(46723, new HANDLE_CHECKS());
            //addPacket(30209, new HANDLE_CostumeItem_BUY_2());
            //addPacket(24593, new HANDLE_FIND_USER());
        }

        public static PacketHandler parsePacket(string thePacket)
        {
            try
            {
                string[] strArray = thePacket.Split(Convert.ToChar(32));
                long Timestamp = long.Parse(strArray[0]);
                int ID = int.Parse(strArray[1]);
                if (PacketManager._Packets.ContainsKey(ID))
                {
                    string[] Blocks = new string[strArray.Length - 2];
                    Array.Copy((Array)strArray, 2, (Array)Blocks, 0, strArray.Length - 2);
                    PacketHandler packetHandler = (PacketHandler)PacketManager._Packets[ID];
                    packetHandler.set(Timestamp, ID, Blocks);
                    return packetHandler;
                }
            }
            catch (Exception ex)
            {
            }
            return (PacketHandler)null;
        }

        public static PacketHandler getHandler(int packetId, long timeStamp, string[] data)
        {
            try
            {
                if (_Packets.ContainsKey(packetId))
                {
                    PacketHandler pHandler = (PacketHandler)_Packets[packetId];
                    pHandler.set(timeStamp, packetId, data);
                    return pHandler;
                }
                else
                {
                    if (Structure.Debug == 1)
                    {
                        Log.AppendError("Unknown Packet ID: " + packetId);

                    }
                    return null;
                }
            }
            catch { return null; }
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

        public static string IPToAddr(long address)
        {
            return IPAddress.Parse(address.ToString()).ToString();
        }

        public static long IPToInt(string addr)
        {
            return (long)((ulong)IPAddress.NetworkToHostOrder((int)IPAddress.Parse(ReverseIP(addr)).Address));
        }

        private static string ReverseIP(string tString)
        {
            try
            {
                string[] strArray = tString.Split(new char[] { '.' });
                string str = "";
                for (int i = strArray.Length - 1; i > -1; i--)
                {
                    str = str + strArray[i] + ".";
                }
                return str.Substring(0, str.Length - 1);
            }
            catch (Exception ex) { Log.AppendError(ex.Message); return "-1"; }
        }

        public static int ReversePort(int iPort)
        {
            try
            {
                char[] array = iPort.ToString().ToCharArray();
                Array.Reverse(array);
                return Convert.ToInt32(new string(array));
            }
            catch (Exception ex) { Log.AppendError(ex.Message); return -1; }
        }
    }
}
