using System;
using System.Globalization;

namespace ReBornWarRock_PServer.GameServer.Networking.Packets
{
    class PACKET_LOGIN : Packet
    {
        public enum ErrorCodes
        {
            Success = 1,
            ClientVersionMissmatch = 90020
        }

        public PACKET_LOGIN(ErrorCodes ErrorCode)
        {
            newPacket(24832);
            addBlock(ErrorCode);
        }
        public PACKET_LOGIN()
        {
            newPacket(24832);
            addBlock(1);
            DateTime now = DateTime.Now;
            int years = now.Year - 1970 + 70;
            int days = now.Day - 1;
            int months = now.Month - 1;

            string date = (now.Second) + "/" + (now.Minute) + "/" + (now.Hour + 20) + "/" + days + "/" + months + "/" + years + "/0/" + now.DayOfYear + "/0";
            addBlock(date);
        }
    }
}
