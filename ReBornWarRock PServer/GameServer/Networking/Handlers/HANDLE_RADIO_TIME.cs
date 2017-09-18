using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading;
using ReBornWarRock_PServer.GameServer.Managers;
using ReBornWarRock_PServer.GameServer.Networking.Packets;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.Room;

using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;


namespace ReBornWarRock_PServer.GameServer.Networking.Handlers
{
    class HANDLE_RADIO_TIME : PacketHandler
    {
        public override void Handle(ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser User)
        {


            virtualRoom currentRoom = User.Room;
            int value = 0;
            int totPerc = (currentRoom.HackPercentageA + currentRoom.HackPercentageB);
            if (!currentRoom.GameActive || totPerc >= 100) return;

            //0 1 6 0 99 0
            User.hackingBase = Convert.ToInt32(getBlock(3));
            int sType = 2;

            if (getBlock(1) == "0" && getBlock(2) == "0" && (getBlock(3) == "0" || getBlock(3) == "1")) // Starting to hack send animation
            {
                User.send(new SP_RoomHackMission(User.RoomSlot, (User.hackingBase == 0 ? currentRoom.HackPercentageA : currentRoom.HackPercentageB), 0, User.hackingBase, value));
                return;
            }
            else if (getBlock(1) == "1" && getBlock(2) == "6" && getBlock(3) == "0" && currentRoom.PickuppedC4 == false) // Pickup the C4
            {
                if (currentRoom.MapID == 56)
                {
                    if (currentRoom.getSide(User) != (int)currentRoom.getSide(User)) return;
                    currentRoom.PickuppedC4 = true;
                    User.hasC4 = true;
                    currentRoom.send(new SP_Unknown(29985, 0, 0, 1, 6, 0, 0, -1, 0)); // Remove the C4 from table
                    currentRoom.send(new SP_Unknown(30000, 1, User.RoomSlot, User.UserID, 2, 155, 0, 0, 92, 0, 0, 0, 0, 0, 0, 0)); // Switch to C4
                }
                else
                {
                    Log.AppendError("Tried to pickup C4 in room " + currentRoom.MapID);
                }
                return;
            }
            else if (getBlock(1) == "1" && getBlock(2) == "0" && getBlock(3) == "0") // Use the C4
            {
                if (currentRoom.MapID == 58)
                {
                    //30000 1 -1 4 2 104 0 1 1 0 0 92 0 92 -1 0 0 1333333 -1166666 1333333 0 2845.7510 205.0797 3374.0964 -70.9974 45.4165 -287.9179 0 0 DP05
                    currentRoom.PickuppedC4 = false;
                    User.hasC4 = false;
                    //currentRoom.SiegeWarC4User = User.UserID.ToString();
                    currentRoom.send(new SP_Unknown(29985, 0, 0, 1, 0, 0, 0, 0, 0)); // Remove C4 from the user 
                    currentRoom.SiegeWarTime = 5;
                }
                else
                {
                    Log.AppendError("Tried to use C4 in room " + currentRoom.MapID);
                }
                return;
            }
            else if (Convert.ToInt32(getBlock(2)) == 3) // Stop to hack
            {
                User.isHacking = false;
                sType = 3;
                currentRoom.send(new SP_RoomHackMission(User.RoomSlot, (User.hackingBase == 0 ? currentRoom.HackPercentageA : currentRoom.HackPercentageB), sType, User.hackingBase, value));
                return;
            }

            //if (currentRoom == null || User.LastHackTick > currentRoom.InitialTime && User.LastHackTick > 0) return;

            if (User.hackingBase != User.LastHackBase)
            {
                User.hackTick = 0;
            }

            if ((User.LastHackTick < currentRoom.InitialTime) || (User.LastHackTick == 0))
            {
                User.LastHackTick = 0;
                User.hackTick++;
                User.rPoints++;
                User.hackTick = 0;

                if (User.hackingBase == 0)
                {
                    currentRoom.HackPercentageA++;
                }
                else
                {
                    currentRoom.HackPercentageB++;
                }
            }

            int totalPercentage = (currentRoom.HackPercentageA + currentRoom.HackPercentageB);
            if (User.GodMode) totalPercentage = 100;

            User.LastHackBase = User.hackingBase;

            User.hackPercentage++;
            User.isHacking = true;

            if (totalPercentage >= 100)
            {
                if (currentRoom.MapID == 58)
                {
                    if (currentRoom.Mission1 == null)
                    {
                        currentRoom.Mission1 = User.Nickname;
                        User.rPoints += 1;
                        currentRoom.send(new SP_Unknown(29985, 0, 0, 1, 1, 99, 0)); // Go to mission 2
                        currentRoom.send(new SP_Unknown(29985, 0, 0, 0, 4, 1, 100, -1, 0)); // Go to mission 2 
                        currentRoom.send(new SP_Unknown(29985, 0, -1, 1, 5, -1, 0, -1, 0)); // Spawn the C4
                        currentRoom.Flags[2] = 1;
                        currentRoom.Flags[3] = 0;
                        currentRoom.Flags[0] = currentRoom.Flags[1] = -1;
                        currentRoom.send(new SP_Unknown(30000, 1, -1, currentRoom.ID, 2, 156, 0, 1, 2, 1, -1, 2, 0, 92, -1, 0, 0, 1333333, -1166666, 1333333, 0, 3689.6670, 969.9617, 4332.0752, 64.4469, 37.4174, -290.5969, 0, 0, "DU02"));
                        currentRoom.send(new SP_Unknown(30000, 1, -1, currentRoom.ID, 2, 156, 0, 1, 3, 0, -1, 2, 0, 92, -1, 0, 0, 1333333, -1166666, 1333333, 0, 3689.6670, 969.9617, 4332.0752, 64.4469, 37.4174, -290.5969, 0, 0, "DU02"));
                        currentRoom.send(new SP_Unknown(30000, 1, -1, currentRoom.ID, 2, 156, 0, 1, 0, -1, 0, 0, 0, 92, -1, 0, 0, 1333333, -1166666, 1333333, 0, 3689.6670, 969.9617, 4332.0752, 64.4469, 37.4174, -290.5969, 0, 0, "DU02"));
                        currentRoom.send(new SP_Unknown(30000, 1, -1, currentRoom.ID, 2, 156, 0, 1, 1, -1, 1, 1, 0, 92, -1, 0, 0, 1333333, -1166666, 1333333, 0, 3689.6670, 969.9617, 4332.0752, 64.4469, 37.4174, -290.5969, 0, 0, "DU02"));
                        currentRoom.PickuppedC4 = false;
                    }
                }
                else if (totalPercentage >= 100 && currentRoom.Mode == 11)
                {
                    currentRoom.InitialTime += 480000;
                    //S=> 250048643 29985 0 0 0 4 1 100 -1 0
                    sType = 4;
                    currentRoom.send(new SP_RoomHackMission(User.RoomSlot, totalPercentage, sType, User.hackingBase, value));

                    //250048643 30053 3 131487 0 100 -1 0
                    currentRoom.Stage2.Stop();
                    currentRoom.milliSec = currentRoom.Stage2.ElapsedMilliseconds;

                    //currentRoom.send(new PACKET_TIMEATTACK_ALL(0, 3, totalPercentage, -1, currentRoom.milliSec, 0));
                    currentRoom.send(new PACKET_SCORE_BOARD_AI_TIMEATTACK(currentRoom, User, currentRoom.milliSec));
                    currentRoom.milliSec = 0;
                    currentRoom.freezeZombies = false;

                    //250048643 29985 0 -1 1 5 -1 0 -1 0
                    sType = 5;
                    value = 0;
                    totalPercentage = 0;
                    User.hackingBase = -1;
                    currentRoom.send(new SP_RoomHackMission(User.RoomSlot, totalPercentage, sType, User.hackingBase, value));
                    //currentRoom.timeattack.PrepareNewStage(3);
                }
            }
            currentRoom.send(new SP_RoomHackMission(User.RoomSlot, (User.hackingBase == 0 ? currentRoom.HackPercentageA : currentRoom.HackPercentageB), sType, User.hackingBase, value));

            //currentRoom.send(new SP_RoomHackMission(User.RoomSlot, (User.hackingBase == 0 ? currentRoom.HackPercentageA : currentRoom.HackPercentageB), sType, User.hackingBase));
        }
    }
}