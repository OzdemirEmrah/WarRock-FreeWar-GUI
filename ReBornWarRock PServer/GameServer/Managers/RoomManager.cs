using ReBornWarRock_PServer.GameServer;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.Room;
using System;
using System.Collections;
using System.Threading;

namespace ReBornWarRock_PServer.GameServer.Managers
{
    internal class RoomManager
    {
        private static Hashtable[] _Rooms = new Hashtable[5];
        private static Thread _roomTick;
        private static Thread _roomRefresh;
         

        ~RoomManager()
        {
            GC.Collect();
        }

        public static void setup()
        {
            for (int index = 1; index < 5; ++index)
                RoomManager._Rooms[index] = new Hashtable();
            RoomManager._roomTick = new Thread(new ThreadStart(RoomManager.roomTick));
            RoomManager._roomTick.Priority = ThreadPriority.AboveNormal;
            RoomManager._roomTick.Start();
        }

        public static ArrayList getRoomsInChannel(int ID)
        {
            try
            {
                return new ArrayList((ICollection)RoomManager._Rooms[ID]);
            }
            catch
            {
                return (ArrayList)null;
            }
        }

        public static int getOpenID(int Channel)
        {
            try
            {
                for (int index = 0; index < 999; ++index)
                {
                    if (!RoomManager._Rooms[Channel].ContainsKey(index))
                        return index;
                }
                return -1;
            }
            catch
            {
                return -1;
            }
        }

        public static virtualRoom getRoom(int Channel, int ID)
        {
            try
            {
                if (RoomManager._Rooms[Channel].ContainsKey(ID))
                    return (virtualRoom)RoomManager._Rooms[Channel][ID];
                else
                    return (virtualRoom)null;
            }
            catch
            {
                return (virtualRoom)null;
            }
        }

        public static ArrayList getRoomsInChannel(int Channel, int Page)
        {
            try
            {
                ArrayList arrayList = new ArrayList();
                for (int index = 14 * Page; index < 14 * (Page + 1); ++index)
                {
                    if (RoomManager._Rooms[Channel].ContainsKey(index))
                        arrayList.Add(RoomManager._Rooms[Channel][index]);
                }
                return arrayList;
            }
            catch
            {
                return (ArrayList)null;
            }
        }

        public static bool addRoom(int Channel, int ID, virtualRoom Room)
        {
            try
            {
                if (RoomManager._Rooms[Channel].ContainsKey(ID))
                    return false;
                Log.AppendText("Added Room [" + ID + ":" + Room.Name + "] to the Room List.");
                RoomManager._Rooms[Channel].Add(ID, Room);
                return true;
            }
            catch (Exception ex)
            {
                Log.AppendText(ex.Message);
                return false;
            }
        }

        public static bool removeRoom(int Channel, int ID)
        {
            try
            {
                if (!RoomManager._Rooms[Channel].ContainsKey(ID))
                    return false;
                Log.AppendText("Removed Room [" + ID + "] from the Room List.");
                RoomManager._Rooms[Channel].Remove(ID);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static void shutDown()
        {
            for (int index = 1; index < 5; ++index)
            {
                foreach (virtualRoom virtualRoom in (IEnumerable)((Hashtable)RoomManager._Rooms[index].Clone()).Values)
                    virtualRoom.remove();
            }
        }

        public static int RoomToPageCount(int Channel)
        {
            if (_Rooms[Channel] != null)
            {
                return _Rooms[Channel].Count / 15;
            }
            else return 0;
        }

        private static void roomTick()
        {
            try
            {
                while (true)
                {
                    for (int index = 1; index < 5; ++index)
                    {
                        foreach (virtualRoom virtualRoom in (IEnumerable)((Hashtable)RoomManager._Rooms[index].Clone()).Values)
                            virtualRoom.update();
                    }
                    Thread.Sleep(50);
                }
            }
            catch (Exception ex)
            {
                Log.AppendText(ex.Message);
            }
        }
    }
}
