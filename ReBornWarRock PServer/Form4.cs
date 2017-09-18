using ReBornWarRock_PServer.GameServer.Managers;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;
using ReBornWarRock_PServer.LoginServer;
using System;
using System.Collections;
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
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (textBox2.Enabled == true)
                textBox2.Enabled = false;
            else textBox2.Enabled = true;
        }

        public void comboBox1Load()
        {
            int[] Numbers = DB.runReadColumn("SELECT id FROM users", 0, null);
            ArrayList myarray = new ArrayList();
            comboBox1.Items.Clear();
            for (int key = 0; key < Numbers.Length; ++key)
            {
                string[] strArray = DB.runReadRow("SELECT username, bantime, banreason FROM users WHERE id=" + Numbers[key].ToString());
                if ( strArray[1] == "-1" && strArray[2] == "Unknown")
               this.comboBox1.Items.AddRange(new object[] {strArray[0]});
                if (strArray[1] != "-1" && strArray[2] != "Unknown") button2.Enabled = true;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "")
            {
                MessageBox.Show("No Player Selected", "Ban Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (textBox1.Text == "")
            {
                MessageBox.Show("Reason Not Valid", "Ban Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (textBox2.Text == "" && !checkBox1.Checked )
            {
                MessageBox.Show("Time Of Ban Incorrect", "Ban Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int BannedTime = 0;
            if (!checkBox1.Checked) BannedTime = Convert.ToInt32(textBox2.Text);
            else BannedTime = 0;
            long Hours = Convert.ToInt64(BannedTime);

            if (!checkBox1.Checked)
            {          
            DateTime _BanCuurent = DateTime.Now;
            _BanCuurent = _BanCuurent.AddHours(Hours);
            BannedTime = Convert.ToInt32(String.Format("{0:yyMMddHH}", _BanCuurent));
            }
            DB.runQuery("UPDATE users SET bantime='" + BannedTime + "', rank='0', banned='1', banreason='" + textBox1.Text + "' WHERE username='" + comboBox1.Text + "'");
            foreach (virtualUser Player in UserManager.getAllUsers())
            {
                if (Player.Username == comboBox1.Text) Player.disconnect();
            }

            comboBox1.ResetText();
            int[] Numbers = DB.runReadColumn("SELECT id FROM users", 0, null);
            ArrayList myarray = new ArrayList();
            for (int key = 0; key < Numbers.Length; ++key)
            {
                string[] strArray = DB.runReadRow("SELECT username, bantime, banreason FROM users WHERE id=" + Numbers[key].ToString());
                if (strArray[1] == "-1" && strArray[2] == "Unknown")
                    this.comboBox1.Items.AddRange(new object[] { strArray[0] });
            }
            comboBox1.Items.Clear();
            comboBox1Load();
            MessageBox.Show("Requested Ban Succeeded", "Ban Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void Form4_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormCalling.frm4 = new Form4();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormCalling.frm7.comboBox1Load();
            FormCalling.frm7.Show();
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
    }
}
