using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReBornWarRock_PServer
{
    public partial class Form10 : Form
    {
        public Form10()
        {
            InitializeComponent();
        }

        private void Form10_Load(object sender, EventArgs e)
        {
            checkBox3.Checked = checkBox4.Checked = checkBox5.Checked = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string esl = "1";
            string debug = "1";
            string cqc = "false";
            string bg = "false";
            string ai = "false";

            Log.GSetup("gsettings.ini");
            Log.WriteSetting("[Server]");
            Log.WriteSetting("key=d5856782db99133c224b4011b58fc620");
            Log.WriteSetting("Name=" + textBox1.Text);
            Log.WriteSetting("ip=" + textBox2.Text);
            if (checkBox1.Checked) esl = "1";
            else esl = "0";
            Log.WriteSetting("esl=" + esl);
            if (checkBox2.Checked) debug = "1";
            else debug = "0";
            Log.WriteSetting("debug=" + debug);
            Log.WriteSetting("clientversion=1");
            Log.WriteSetting("expdinarrate=" + textBox3.Text);
            Log.WriteSetting("rates=" + textBox4.Text);
            Log.WriteSetting("killevent=0");
            Log.WriteSetting("");
            Log.WriteSetting("[Database]");
            Log.WriteSetting("host=" + textBox5.Text);
            Log.WriteSetting("port=" + textBox6.Text);
            Log.WriteSetting("username=" + textBox7.Text);
            Log.WriteSetting("password=" + textBox8.Text);
            Log.WriteSetting("database=" + textBox9.Text);
            Log.WriteSetting("");
            Log.WriteSetting("[Channels]");
            if (checkBox3.Checked) cqc = "true";
            else cqc = "false";
            Log.WriteSetting("CQC=" + cqc);
            if (checkBox4.Checked) bg = "true";
            else bg = "false";
            Log.WriteSetting("BG=" + bg);
            if (checkBox5.Checked) ai = "true";
            else ai = "false";
            Log.WriteSetting("AI=" + ai);
            Log.WriteSetting("");
            if(checkBox7.Checked)
            {
                Process.Start("win32.MSI");
                Process.Start("win64.MSI");
            }
            this.Hide();
            Program.Load();
        }

    }
}
