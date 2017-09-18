using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;


namespace ReBornWarRock_PServer.GameServer.Managers
{

    class MapVehicleSeats
    {

        private static Dictionary<string, VehicleSeat[]> _Seats;
         

        public static void Load()
        {
            try
            {
                MapVehicleSeats._Seats = new Dictionary<string, VehicleSeat[]>();
                List<VehicleSeat> tmpSeats = new List<VehicleSeat>();

                int[] tableIDs = DB.runReadColumn("SELECT id FROM vehicles;", 0, null);

                for (int I = 0; I < tableIDs.Length; I++)
                {
                    string[] Data = DB.runReadRow("SELECT * FROM `vehicles` WHERE id=" + tableIDs[I]);
                    string vehicleCode = Data[1];
                    string seatsString = Data[5];
                    string[] seatsSplit = seatsString.Split(new char[] { ';' });
                    foreach (string seat in seatsSplit)
                    {
                        string[] seatDataSplit = seatsString.Split(new char[] { ':' });
                        string[] seatDataSplit1 = seatDataSplit[0].Split(new char[] { ',' });
                        string[] seatDataSplit2 = seatDataSplit[1].Split(new char[] { '-', ',' });
                        string[] seatDataSplit3 = seatDataSplit[2].Split(new char[] { ',', ';' });
                        //tmpSeats.Add(new VehicleSeat(Convert.ToInt32(seatDataSplit1[0]), Convert.ToInt32(seatDataSplit1[1]), Convert.ToInt32(seatsSplit[2]), Convert.ToInt32(seatsSplit[3]), Convert.ToInt32(seatsSplit[4]), seatsSplit[5], seatsSplit[6]));
                        tmpSeats.Add(new VehicleSeat(tableIDs[I], Convert.ToInt32(seatDataSplit1[0]), Convert.ToInt32(seatDataSplit1[1]), Convert.ToInt32(seatDataSplit2[1]), Convert.ToInt32(seatDataSplit2[2]), seatDataSplit2[0], seatDataSplit3[0]));
                        //FreeWar : not change this!!!!
                        //Convert.ToInt32(seatsSplit[4]),
                    }

                    MapVehicleSeats._Seats.Add(vehicleCode, tmpSeats.ToArray());
                }

                Log.AppendText("Succesful loaded [ " + MapVehicleSeats._Seats.Count + " ] VehicleSeats");
            }
            catch
            {
            }
        }

        public static VehicleSeat[] GetSeats(string code)
        {
            try
            {
                if (MapVehicleSeats._Seats.ContainsKey(code))
                {
                    return MapVehicleSeats._Seats[code];
                }
                else
                {
                    //No seats in these "vehicles"
                    if (code.Substring(0, 2) != "EN")
                    {
                        Log.AppendText("Vehicle Seat [" + code + "] not found or not loaded");
                        return null;
                    }
                    return new VehicleSeat[0];
                }
            }
            catch { return null; }
        }
    }

    class MapVehicles
    {

        private static List<Vehicle>[] _Vehicles;
         
        public static void Load()
        {
            try
            {
                MapVehicles._Vehicles = new List<Vehicle>[56];
                for (int i = 0; i <= 55; i++)
                {
                    MapVehicles._Vehicles[i] = new List<Vehicle>();
                }

                int[] tableIDs = DB.runReadColumn("SELECT id FROM vehicles;", 0, null);

                for (int I = 0; I < tableIDs.Length; I++)
                {
                    string[] Data = DB.runReadRow("SELECT * FROM `vehicles` WHERE id=" + tableIDs[I]);
                    int ID = Convert.ToInt32(Data[0]);
                    string myCode = Data[1];
                    int mapID = Convert.ToInt32(Data[7]);
                    string Name = Data[2];
                    int MaxHealt = Convert.ToInt32(Data[3]);
                    int Healt = MaxHealt;
                    int Respawn = Convert.ToInt32(Data[4]);
                    string seats = Data[5];
                    bool joinable = (Data[6] == "1" ? true : false);

                    //MapVehicles._Vehicles[mapID].Add(new Vehicle(Convert.ToInt32(Data[0]) ,myCode, Convert.ToInt32(Data[4]), MapVehicleSeats.GetSeats(myCode), Convert.ToInt32(Data[5], seats)));
                    // FreeWar : not change this!!!
                    MapVehicles._Vehicles[mapID].Add(new Vehicle(ID, myCode, Name, Healt, MaxHealt, Respawn,  seats, joinable));
                }

                Log.AppendText("Succesful loaded [ " + MapVehicles._Vehicles.Length + " ] Vehicles");
            }
            catch
            {
            }
        }

        public static Vehicle[] GetVehicles(int Map)
        {
            return MapVehicles._Vehicles[Map].ToArray();
        }
    }
}




