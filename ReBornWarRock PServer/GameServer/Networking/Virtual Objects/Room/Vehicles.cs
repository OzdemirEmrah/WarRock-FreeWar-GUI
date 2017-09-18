using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;
using ReBornWarRock_PServer.GameServer.Networking;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.Room;

namespace ReBornWarRock_PServer.GameServer
{
    class Vehicle
    {
        public int ID;
        public string Code;
        public string Name;
        public int Health;
        public int MaxHealth;
        public int RespawnTime;
        public int SpawnProtection = 5;
        public string ChangedCode = string.Empty;
        public string X = null;
        public string Y = null;
        public string Z = null;
        public string PosX = null;
        public string PosY = null;
        public string PosZ = null;
        public ConcurrentDictionary<int, VehicleSeat> Seats = new ConcurrentDictionary<int, VehicleSeat>();
        public string SeatString = null;
        public int RespawnTick = 0;
        public bool isJoinable = true;
        public int TimeWithoutOwner = 0;

        public void LoadSeats(string Seats)
        {
            this.Seats.Clear();

            int SeatID = 0;

            string[] seatSplit = Seats.Split(new char[] { ';' });
            foreach (string sSeat in seatSplit)
            {
                try
                {
                    //999,0:FB04 - 10,2:FJ03; 0,0:FA01-0,0:FA01
                    string[] theSeat = sSeat.Split(new char[] { '-' });

                    string[] MainSeatSplit = theSeat[0].Split(new char[] { ':' });
                    string[] SubSeatSplit = theSeat[1].Split(new char[] { ':' });

                    string splittedMain = MainSeatSplit[0];
                    string splittedSub = SubSeatSplit[0];

                    string[] MainCTSplit = splittedMain.Split(new char[] { ',' });
                    string[] SubCTSplit = splittedSub.Split(new char[] { ',' });

                    VehicleSeat VehSeat = new VehicleSeat(SeatID, Convert.ToInt32(MainCTSplit[0]), Convert.ToInt32(MainCTSplit[1]), Convert.ToInt32(SubCTSplit[0]), Convert.ToInt32(SubCTSplit[1]), MainSeatSplit[1], SubSeatSplit[1]);
                    this.Seats.TryAdd(SeatID, VehSeat);
                    SeatID++;
                }
                catch
                {
                    Log.AppendError("Error while loading seat: " + sSeat);
                }
            }
        }

        public Vehicle(int ID, string Code, string Name, int Health, int MaxHealth, int RespawnTime, string Seats, bool isJoinable)
        {
            this.ID = ID;
            this.Code = Code;
            this.Name = Name;
            this.Health = Health;
            this.MaxHealth = MaxHealth;
            this.RespawnTime = RespawnTime;
            this.isJoinable = isJoinable;
            SeatString = Seats;
            LoadSeats(Seats);
        }

        public VehicleSeat GetSeatByID(int ID)
        {
            if (Seats.ContainsKey(ID))
            {
                return (VehicleSeat)Seats[ID];
            }
            return null;
        }

        public List<virtualUser> Users
        {
            get
            {
                List<virtualUser> list = new List<virtualUser>();
                foreach (VehicleSeat s in Seats.Values)
                {
                    if (s.seatOwner != null)
                    {
                        list.Add(s.seatOwner);
                    }
                }
                return list;
            }
        }

        public int GetUserSeatID(virtualUser usr)
        {
            var v = Seats.Values.Where(r => r.seatOwner.UserID == usr.UserID).FirstOrDefault();
            if (v != null)
            {
                return v.ID;
            }
            return -1;
        }

        public bool IsRightVehicle(string code)
        {
            return this.Code == code;
        }

        public bool FreeSeat(int SeatID)
        {
            if (Seats.ContainsKey(SeatID))
            {
                VehicleSeat seat = (VehicleSeat)Seats[SeatID];
                if (seat.seatOwner == null)
                {
                    return true;
                }
            }
            return false;
        }

        public int Side
        {
            get
            {
                foreach (VehicleSeat VehicleSeat in Seats.Values)
                {
                    if (VehicleSeat.seatOwner != null)
                    {
                        virtualUser Owner = VehicleSeat.seatOwner;
                        virtualRoom Room = VehicleSeat.seatOwner.Room;
                        return Room.getSide(Owner);
                    }
                }
                return -1;
            }
        }

        public int getHealthPercentage(int Percentage)
        {
            return Convert.ToInt32(((Math.Truncate((double)(MaxHealth * Percentage))) / 100).ToString());
        }

        public List<virtualUser> Players
        {
            get
            {
                List<virtualUser> list = new List<virtualUser>();
                foreach (VehicleSeat seat in Seats.Values)
                {
                    list.Add(seat.seatOwner);
                }
                return list;
            }
        }

        public VehicleSeat GetSeatByUser(virtualUser usr)
        {
            try
            {
                return Seats.Values.Where(r => r.seatOwner.UserID == usr.UserID).First();
            }
            catch { }

            return null;
        }

        public int GetSeat(virtualUser usr)
        {
            try
            {
                return Seats.Values.Where(r => r.seatOwner.UserID == usr.UserID).First().ID;
            }
            catch { }

            return -1;
        }

        public void SwitchSeat(int ID, virtualUser usr)
        {
            VehicleSeat Seat = GetSeatByID(ID);
            if (Seat == null) return;
            if (Seat.ID == ID && Seat.seatOwner == null)
            {
                usr.currentSeat.LeaveSeat(usr);
                usr.currentSeat = Seat;
                Seat.seatOwner = usr;
            }
        }

        public bool TakeSeat(int ID, virtualUser usr)
        {
            Seats.Values.Where(r => r.seatOwner.UserID == usr.UserID).First().LeaveSeat(usr);

            foreach (VehicleSeat Seat in Seats.Values)
            {
                return Seat.TakeSeat(usr);
            }
            return false;
        }

        public bool Join(virtualUser usr)
        {
            foreach (VehicleSeat Seat in Seats.Values)
            {
                if (Seat.TakeSeat(usr))
                {
                    usr.currentVehicle = this;
                    usr.currentSeat = Seat;
                    return true;
                }
            }
            return false;
        }
        public void Leave(virtualUser usr)
        {
            if (usr.currentSeat != null)
            {
                usr.currentSeat.LeaveSeat(usr);
                usr.currentSeat = null;
            }
            usr.currentVehicle = null;
        }
    }
}
