using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReBornWarRock_PServer
{
    public partial class Form6 : Form
    {
        public Form6()
        {
            InitializeComponent();
        }

        public void AppendTextBox(string value)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(AppendTextBox), new object[] { value });
                return;
            }
            if (value.Contains("S=>"))
                richTextBox1.SelectionColor = System.Drawing.Color.Green;
            if (value.Contains("C=>"))
                richTextBox1.SelectionColor = System.Drawing.Color.HotPink;
            richTextBox1.AppendText(value + Environment.NewLine);
            richTextBox1.ScrollToCaret();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form6_Load(object sender, EventArgs e)
        {

        }

        private void Form6_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormCalling.frm3.checkBox4.Checked = false;
            FormCalling.frm6 = new Form6();
            //this.Close();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Pause")
            {
                button1.Text = "Resume";
            Structure.PacketView = false;
            }
            else
            {
                button1.Text = "Pause";
                Structure.PacketView = true;
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
