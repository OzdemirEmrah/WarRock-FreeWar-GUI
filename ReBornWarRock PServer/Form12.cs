using ReBornWarRock_PServer.GameServer.Managers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReBornWarRock_PServer
{
    public partial class Form12 : Form
    {
        public Form12()
        {
            InitializeComponent();
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

        private void button1_Click(object sender, EventArgs e)
        {
            ConfigServer.DinarRate = double.Parse(textBox2.Text, CultureInfo.InvariantCulture);
            ConfigServer.ExpRate = double.Parse(textBox1.Text, CultureInfo.InvariantCulture);
            UserManager.sendToServer(new GameServer.Networking.Packets.PACKET_CHAT(" NOTICE: ", GameServer.Networking.Packets.PACKET_CHAT.ChatType.Notice1,"The rates have been changed: Exp = " + ConfigServer.ExpRate.ToString() +"; Dinar = "+ ConfigServer.DinarRate.ToString() +";", 100, "NULL"));
            MessageBox.Show("Rates have been edited", "Rates Edited", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
