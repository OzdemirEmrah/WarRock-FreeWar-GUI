using ReBornWarRock_PServer.LoginServer;
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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        public void Closing()
        {
            this.Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string UserName = textBox1.Text.ToString();
            string Password = textBox2.Text.ToString();
            int UserID = 0;

            try
            {
                UserID = int.Parse(MYSQL.runReadOnce("id", "SELECT * FROM users WHERE username='" + UserName + "'").ToString());
            }
            catch { UserID = 0; MessageBox.Show("UserName Not Found", "Error LogIn",MessageBoxButtons.OK,MessageBoxIcon.Error );
            }
            if (UserID > 0)
            {
                DataTable dt = MYSQL.runRead("SELECT id, username, password, salt, rank FROM users WHERE id=" + UserID.ToString());
            DataRow row = dt.Rows[0];

            string Salt = row["salt"].ToString();
            string md5Password = Structure.convertToMD5(Structure.convertToMD5(Password) + Structure.convertToMD5(Salt));

                if (row["password"].ToString() == md5Password)
                {
                    if (int.Parse(row["rank"].ToString()) == 6)
                    {
                        this.Visible = false;
                        FormCalling.frm1.Show();
                        FormCalling.frm1.Enabled = true;
                    }
                    else MessageBox.Show("Insufficient Rank To Log In The Core Panel", "Error LogIn", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else MessageBox.Show("Wrong Password", "Error LogIn", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (textBox2.Text == "" && textBox1.Text == "")
                {
                    MessageBox.Show("Insert Password And UserName", "Error LogIn", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (textBox2.Text != "") button1.PerformClick();
                else MessageBox.Show("Insert Password", "Error LogIn", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
    }


        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (textBox2.Text == "" && textBox1.Text == "")
                {
                    MessageBox.Show("Insert Password And UserName", "Error LogIn", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (textBox1.Text == "") MessageBox.Show("Insert UserName", "Error LogIn", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (textBox2.Text != "" && textBox1.Text != "") button1.PerformClick();
            }
        }
    }
}
