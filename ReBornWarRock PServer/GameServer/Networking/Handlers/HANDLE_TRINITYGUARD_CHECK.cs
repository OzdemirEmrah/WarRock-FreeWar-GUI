using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReBornWarRock_PServer.GameServer.Networking.Packets;

namespace ReBornWarRock_PServer.GameServer.Networking.Handlers
{
    class HANDLE_TRINITYGUARD_CHECK : PacketHandler
    {
        public override void Handle(ReBornWarRock_PServer.GameServer.Virtual_Objects.User.virtualUser User)
        {
            try
            {
                int OPCode = Convert.ToInt32(getNextBlock());
                string Value = getNextBlock();
                switch (OPCode)
                {
                    case 200:
                        {
                            //Login packet
                            if (Convert.ToInt32(Value) != 13)
                            {
                                User.disconnect();
                                return;
                            }
                            else
                            {
                                Log.AppendText("Passed TrinityGuard Check");
                            }
                            break;
                        }
                    case 207:
                        {
                            Log.AppendError("Tried to modify asm!");
                            User.disconnect();
                            break;
                        }
                    case 209:
                        {
                            Log.AppendError("Running some illegal program!");
                            User.disconnect();
                            break;
                        }
                    case 210:
                        {
                            Log.AppendError("IntegritY check has failed!");
                            User.disconnect();
                            break;
                        }
                    case 211:
                        {
                            Log.AppendError("DirectX hook has failed!");
                            User.disconnect();
                            break;
                        }
                    default:
                        {
                            Log.AppendError("Received unknown TrinityGuard function: " + getAllBlocks());
                            User.disconnect();
                            break;
                        }
                }
            }
            catch { }
        }
    }
}