using ReBornWarRock_PServer.GameServer.Managers;
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
    public partial class Form9 : Form
    {
        public Form9()
        {
            InitializeComponent();
        }

        public void CreateList()
        {
            int[] Numbers = DB.runReadColumn("SELECT id FROM users", 0, null);
            ArrayList myarray = new ArrayList();
            comboBox1.Items.Clear();
            for(int key = 0; key < ItemManager.combobox.Count; ++key)
            {
                comboBox2.Items.Add(ItemManager.combobox[key]);
            }
            //comboBox2 = ItemManager.combobox;
            for (int key = 0; key < Numbers.Length; ++key)
            {
                string[] strArray = DB.runReadRow("SELECT username, rank, nickname FROM users WHERE id=" + Numbers[key].ToString());
                if (strArray[1] != "0" && strArray[0] != "") comboBox1.Items.Add(strArray[2]);
            }
        }

        private void Form9_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormCalling.frm9 = new Form9();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private int GetIDByNickName(string name)
        {
            int[] Numbers = DB.runReadColumn("SELECT id FROM users", 0, null);
            for (int key = 0; key < Numbers.Length; ++key)
            {
                string[] strArray = DB.runReadRow("SELECT username, rank, nickname, id FROM users WHERE id=" + Numbers[key].ToString());
                if (strArray[2] == name) return int.Parse(strArray[3]);
            }

            return -1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "" || comboBox2.Text != "" || comboBox3.Text != "")
            {
                string NickName = comboBox1.Text;
                int ID = GetIDByNickName(comboBox1.Text);
                string Code = ItemManager.getItemByName(comboBox2.Text);
                int Days = int.Parse(comboBox3.Text);
                DB.runQuery("INSERT INTO outbox (ownerid, itemcode, days) VALUES ('" + ID + "', '" + Code + "', '" + Days + "')");
                MessageBox.Show("The item was sended in outbox!", "Send Fuction Succesfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else MessageBox.Show("Some field has been empty!", "Send Fuction Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }
    }
}
