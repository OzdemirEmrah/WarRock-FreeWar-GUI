using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ReBornWarRock_PServer.LoginServer.Packets
{
    class Packet
    {
        [DllImport("winmm.dll")]
        public static extern long timeGetTime();

        int PacketID;
        string[] blocks = new string[0];

        public byte[] getBytes()
        {
            StringBuilder sPacket = new StringBuilder();
            sPacket.Append(timeGetTime());
            sPacket.Append(Convert.ToChar(0x20));
            sPacket.Append(PacketID);
            sPacket.Append(Convert.ToChar(0x20));

            for (int I = 0; I < blocks.Length; I++)
            {
                sPacket.Append(blocks[I].Replace(Convert.ToChar(0x20), Convert.ToChar(0x1D)) + Convert.ToChar(0x20));
            }

            //Message.WriteDebug("Sending Packet -> " + sPacket.ToString());

            return encrypt(System.Text.Encoding.Default.GetBytes(sPacket.ToString() + Convert.ToChar(0x20) + Convert.ToChar(0x0A)));
        }

        protected void newPacket(int PacketID)
        {
            this.PacketID = PacketID;
        }

        protected void addBlock(object Block)
        {
            Array.Resize(ref blocks, blocks.Length + 1);
            blocks[blocks.Length - 1] = Convert.ToString(Block);
        }

        protected void Fill(object block, int length)
        {
            for (int i = 0; i < length; i++)
            {
                addBlock(block);
            }
        }

        private byte[] encrypt(byte[] input)
        {
            for (int I = 0; I < input.Length; I++)
            {
                input[I] = (byte)(input[I] ^ 0x96);
            }

            return input;
        }
    }
}
