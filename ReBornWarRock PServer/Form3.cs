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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReBornWarRock_PServer
{
    public partial class Form3 : Form
    {
        
        public Form3()
        {
            InitializeComponent();
        }
        new bool Load = false;
        public void CheckBoxesUpdate()
        {
            button3.Enabled = true;
            button1.Enabled = true;
            Load = true;
            if (Structure.isEvent)
            {
                button1.Text = "Deactive";
                FormCalling.frm3.listBox1.Text = "True";
                listBox1.Enabled = false;
                textBox1.Enabled = false;
                textBox2.Enabled = false;
                button3.Enabled = false;
            }
            else
            {
                button1.Text = "Active";
                FormCalling.frm3.listBox1.Text = "False";
            }
            checkBox1.Checked = ConfigServer.CQC;
            checkBox2.Checked = ConfigServer.BG;
            checkBox3.Checked = ConfigServer.AI;
            if (ConfigServer.Debug == 0) radioButton2.Checked = true;
            else radioButton1.Checked = true;
            Load = false;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (!Load) ConfigServer.CQC = !ConfigServer.CQC;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (!Load) ConfigServer.BG = !ConfigServer.BG;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (!Load) ConfigServer.AI = !ConfigServer.AI;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton2.Checked == false)
            {
                ConfigServer.Debug = 1;
            Log.AppendText("Debug Mode --> ON");
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == false)
            {
                ConfigServer.Debug = 0;
            Log.AppendText("Debug Mode --> OFF");
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (!Structure.PacketView)
            {
            FormCalling.frm6.Show(); // FreeWar --> The Form isn't necessary to open in FormClosing Class, we can open the form when we want by new Form6();
            Structure.PacketView = true;
                Log.setupPackets("Log_" + "Packets" + ".txt");
            }
            else Structure.PacketView = false;
            Log.AppendText("SYSTEM >> Packet Visible: " + Convert.ToString(Structure.PacketView));
        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormCalling.frm3 = new Form3();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(!Structure.isEvent)
            {
                int Minutes = Convert.ToInt32(textBox1.Text);
            int Percentage = Convert.ToInt32(textBox2.Text);
            Structure.isEvent = true;
            Structure.EventTime = Minutes * 60;
            //FreeWar --> This Section Is Percentage
            Structure.EXPEvent = (Percentage / 100);
            Structure.DinarEvent = (Percentage / 100);
            Structure.EXPBanner = 1;
            foreach (virtualUser Player in UserManager.getAllUsers())
            {
                Player.send(new PACKET_PING(Player));
                Player.send(new PACKET_EVENT_MESSAGE(PACKET_EVENT_MESSAGE.EventCodes.EXP_Activate));
            }
            MessageBox.Show("Event Actived Succesfully" , "Event Information" , MessageBoxButtons.OK , MessageBoxIcon.Information);
            listBox1.Text = Structure.isEvent.ToString();
            listBox1.Enabled = false;
            textBox1.Enabled = false;
            textBox2.Enabled = false;
                button1.Text = "Deactive";
            }
            else
            {
                Structure.isEvent = false;
                Structure.EventTime = -1;
                Structure.EXPBanner = 0;
                listBox1.Enabled = true;
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                button1.Text = "Active";
                MessageBox.Show("Event Deactived Succesfully", "Event Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                foreach (virtualUser Player in UserManager.getAllUsers())
                {
                    Player.send(new PACKET_PING(Player));
                    Player.send(new PACKET_EVENT_MESSAGE(PACKET_EVENT_MESSAGE.EventCodes.EXP_Deactivate));
                }
            }
        }

        public void UnlockAll(string value)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(UnlockAll), new object[] { value });
                return;
            }
            textBox2.Enabled = bool.Parse(value);
            textBox1.Enabled = bool.Parse(value);
            listBox1.Enabled = bool.Parse(value);
            //button1.Enabled = bool.Parse(value);
            button3.Enabled = bool.Parse(value);
            button1.Text = "Active";
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormCalling.frm9.CreateList();
            FormCalling.frm9.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //button3.Enabled = false;
            FormCalling.frm3.UnlockAll("false");
            Structure.isEvent = true;
            Structure.EventTime = 60 * 60;
            Structure.EXPBanner = 5;
            foreach (virtualUser Player in UserManager.getAllUsers())
            {
                if(!Player.ReceivedRandomBox)
                {
                    Player.AddItem("CZ99", -1, 1);
                Player.ReceivedRandomBox = true;
                Player.send(new PACKET_LOGIN_EVENT_MESSEGE(Player));
                    DB.runQuery("INSERT INTO users_events (eventid, userid) VALUES ('" + 5 + "','" + Player.UserID + "')");
                    //Player.send(new PACKET_LOGIN_EVENT(Player , "CZ99"));
                }

            }
            button1.Text = "Deactive";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FormCalling.frm8.CreateList();
            FormCalling.frm8.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            FormCalling.frm12.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string path = IO.workingDirectory + "/ZombiesTool.exe";
            if (!File.Exists(path))
            {
                MessageBox.Show("ZombieList.exe not found at " + path, "Tool Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
