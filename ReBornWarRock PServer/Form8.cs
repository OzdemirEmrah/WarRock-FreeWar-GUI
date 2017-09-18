using ReBornWarRock_PServer.GameServer.Managers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReBornWarRock_PServer
{
    public partial class Form8 : Form
    {
        public Form8()
        {
            InitializeComponent();
        }
        public void CreateList()
        {
            radioButton1.Checked = true; 
            for (int key = 0; key < ItemManager.combobox.Count; ++key)
            {
                comboBox1.Items.Add(ItemManager.combobox[key]);
            }
            //string Code = ItemManager.getItemByName(comboBox2.Text);
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            string sVal = textBox1.Text;
            if (textBox1.Text.Length > 18) return;
            if (!string.IsNullOrEmpty(sVal) && e.KeyCode != Keys.Back)
            {
                sVal = sVal.Replace("-", "");
                string newst = Regex.Replace(sVal, ".{4}", "$0-");
                textBox1.Text = newst;
                textBox1.SelectionStart = textBox1.Text.Length;
            }
        }
        //C=>994210609 30992 asdaasda-asda-asda 
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar != '\b') && ((sender as TextBox).Text.Length > 18))
            {
                e.Handled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            string Random = RandomString(16).ToLower();
            string RandomSplitted = "";
            RandomSplitted = Regex.Replace(Random, ".{4}", "$0-");
            //RandomSplitted = Regex.Replace(Random, ".{4}", "$0-");
            string[] Splitting = RandomSplitted.Split('-');
            textBox1.Text = Splitting[0] + "-" + Splitting[1] + "-" + Splitting[2]+ "-" + Splitting[3];
        }


        private void Private()
        {
            string[] Splitting = textBox1.Text.Split('-');
            string All = Splitting[0] + Splitting[1] + "-" + Splitting[2] + "-" + Splitting[3];
            string Code = ItemManager.getItemByName(comboBox1.Text);
            string Item = "0";
            int Active = 1;
            if (checkBox1.Checked) Item = "1";
            if (comboBox3.Text == "False") Active = 0;
            DB.runQuery("INSERT INTO coupons (coupon, code, days, amount, active, item, public) VALUES ('" + All + "','" + Code + "','" + comboBox2.Text + "','" + textBox2.Text + "','" + Active + " ',' " + Item + " ',' " + 0 + "')");
        }
        private void Public()
        {
            string[] Splitting = textBox1.Text.Split('-');
            string All = Splitting[0] + Splitting[1] + "-" + Splitting[2] + "-" + Splitting[3];
            string Code = ItemManager.getItemByName(comboBox1.Text);
            string Item = "0";
            int Active = 1;
            if (checkBox1.Checked) Item = "1";
            if (comboBox3.Text == "False") Active = 0;
            DB.runQuery("INSERT INTO coupons (coupon, code, days, amount, active, item, public) VALUES ('" + All + "','" + Code + "','" + comboBox2.Text + "','" + textBox2.Text + "','" + Active + " ',' " + Item + " ',' " + 1 + "')");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked) Public();
            else if (radioButton2.Checked) Private();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (textBox2.Visible = label4.Visible = true) textBox2.Visible = label4.Visible = false;
            else textBox2.Visible = label4.Visible = true;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
