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
    public partial class Form7 : Form
    {
        public Form7()
        {
            InitializeComponent();
        }

        public void comboBox1Load()
        {
            int[] Numbers = DB.runReadColumn("SELECT id FROM users", 0, null);
            ArrayList myarray = new ArrayList();
            for (int key = 0; key < Numbers.Length; ++key)
            {
                string[] strArray = DB.runReadRow("SELECT username, bantime, banreason FROM users WHERE id=" + Numbers[key].ToString());
                if (strArray[1] != "-1" && strArray[2] != "Unknown")
                    this.comboBox1.Items.AddRange(new object[] { strArray[0] });
            }

        }

        private void Form7_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormCalling.frm7 = new Form7();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "")
            {
                MessageBox.Show("No Player Selected", "UnBan Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int[] Numbers = DB.runReadColumn("SELECT id FROM users", 0, null);
            for (int key = 0; key < Numbers.Length; ++key)
            {
                string[] strArray = DB.runReadRow("SELECT username, bantime, banreason FROM users WHERE id=" + Numbers[key].ToString());
                if (strArray[1] != "-1" && strArray[2] != "Unknown")
                {
                    DB.runQuery("UPDATE users SET bantime='" + -1 + "', rank='1', banned='0', banreason='" + "Unknown" + "' WHERE username='" + comboBox1.Text + "'");
                    MessageBox.Show("Requested UnBan Succeeded", "UnBan Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                comboBox1.Items.Clear();
                comboBox1Load();
                comboBox1.Text = "";
                FormCalling.frm4.comboBox1Load();
            }
        }
    }
}
