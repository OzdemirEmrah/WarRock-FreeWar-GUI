using ReBornWarRock_PServer.GameServer.Managers;
using ReBornWarRock_PServer.GameServer.Networking.Packets;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReBornWarRock_PServer
{
    public partial class Form1 : Form
    { 
        public Form1()
        {
            InitializeComponent();
            _Form1 = this;
        }

        public static Form1 _Form1;
        public static string ipdns = null;

        public void AppendTextBox1(string value)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(AppendTextBox1), new object[] { value });
                return;
            }
            textBox2.Text = value;
        }

        public void AppendLabelBox1(string value)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(AppendLabelBox1), new object[] { value });
                return;
            }
            label1.Text = value;
        }

        public void AppendColorLabelBox1(string value)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(AppendColorLabelBox1), new object[] { value });
                return;
            }
            label1.ForeColor = Color.Green;
        }

        public void AppendColorLabelBox2(string value)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(AppendColorLabelBox2), new object[] { value });
                return;
            }
            label2.ForeColor = Color.Green;
        }
        public void AppendColorLabelBox6(string value)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(AppendColorLabelBox6), new object[] { value });
                return;
            }
            label6.ForeColor = Color.Green;
        }

        public void AppendLabelBox6(string value)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(AppendLabelBox6), new object[] { value });
                return;
            }
            label6.Text = value;
        }

        public void AppendLabelBox2(string value)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(AppendLabelBox2), new object[] { value });
                return;
            }
            label2.Text = value;
        }

        public void button6InvokeVisibility(string value)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(button6InvokeVisibility), new object[] { value });
                return;
            }
        }

        public void AppendTextBox(string value)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(AppendTextBox), new object[] { value });
                return;
            }
            if (value.Contains("[ANTI - DDOS]")) richTextBox1.SelectionColor = System.Drawing.Color.DarkMagenta;
            richTextBox1.AppendText(value + Environment.NewLine);
            richTextBox1.ScrollToCaret();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked && textBox1.Text == null)
            {
                MessageBox.Show("Please enter a ddns server", "Empty Server Address", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            label1.Text = "Starting...";
            Structure.StartUpLogin();
            button2.Enabled = true;
            button1.Enabled = false;
            button7.Enabled = true;
            textBox1.Enabled = false;
            checkBox1.Enabled = false;
            label8.Visible = true;
            ipdns = LoginServer.Packets.List_Packets.PACKET_SERVER_LIST.GetIpFromDNS(textBox1.Text);
            label8.Text = ipdns;
                
            //textBox1.Text = "ma che cazzo";
            //BanManager.load();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Structure.PacketManager.setup();
            label2.Text = "Starting...";
            Structure.StartUpServer();
            button2.Enabled = button7.Enabled = false;
            button3.Enabled = button4.Enabled = button5.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FormCalling.frm4.comboBox1Load();
            FormCalling.frm4.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FormCalling.frm3.CheckBoxesUpdate();
            FormCalling.frm3.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {

            FormCalling.frm5.Show();

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Structure.shutDown();
        }


        private void button7_Click(object sender, EventArgs e)
        {
            FormCalling.frm11.Show();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Visible = true;
        }
    }
}
