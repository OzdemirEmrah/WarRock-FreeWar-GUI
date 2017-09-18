using ReBornWarRock_PServer.GameServer.Managers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReBornWarRock_PServer
{
    public partial class Form11 : Form
    {

        public StringBuilder SBSave = new StringBuilder();

        public Form11()
        {
            InitializeComponent();
        }

        private void searchableRichTextBox1_TextChanged(object sender, EventArgs e)
        {
            ItemManager.Items = searchableRichTextBox1.Text;
        }
        public void EncryptBinRaw(string Raw)
        {
            try
            {
                byte[] databuffer = Encoding.Default.GetBytes(Raw);

                for (int i = 0; i < databuffer.Length; i++)
                {
                    databuffer[i] = Convert.ToByte(databuffer[i] ^ ItemManager.XOR);
                    SBSave.Append(databuffer[i].ToString("x2").ToUpper());
                }
            }
            catch (Exception ex) { }
        }

        public void EncryptBinData()
        {
            try
            {
                FileStream outbin = new FileStream(@"out.bin", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter mWrtiter = new StreamWriter(outbin, System.Text.Encoding.GetEncoding(28605));
                mWrtiter.Write(SBSave.ToString());
                mWrtiter.Close();
            }
            catch
            {

            }
        }

        private void Form11_FormClosing(object sender, FormClosingEventArgs e)
        {
            EncryptBinRaw(ItemManager.Items);
            EncryptBinData();
        }
    }
}
