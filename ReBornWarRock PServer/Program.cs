using ReBornWarRock_PServer.GameServer;
using ReBornWarRock_PServer.GameServer.Managers;
using ReBornWarRock_PServer.GameServer.Networking.Packets;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.Room;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;
using ReBornWarRock_PServer.LoginServer;
using ReBornWarRock_PServer.LoginServer.Connection;
using ReBornWarRock_PServer.LoginServer.Docs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReBornWarRock_PServer
{
    static class Program
    {
        /// <summary>
        /// Punto di ingresso principale dell'applicazione.
        /// </summary>

        /// <Definitions>

        private static Thread _ServerThread = null;
        private static Thread _CommandThread = null;
        public static ServerClient sClient;


        [STAThread]

        static void OpenConnection()
        {
            string settingsFile = IO.workingDirectory + @"/gsettings.ini";
            string Host = IO.readINI("Database", "host", settingsFile);
            int Port = Convert.ToInt32(IO.readINI("Database", "port", settingsFile));
            string Username = IO.readINI("Database", "username", settingsFile);
            string Password = IO.readINI("Database", "password", settingsFile);
            string Database = IO.readINI("Database", "database", settingsFile);
            if (DB.openConnection(Host, Port, Database, Username, Password) == false)
                {
                    DialogResult dialogResult = MessageBox.Show("Do You Want To Re-Try?", "Can't Connect to MySQL DataBase", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        OpenConnection();
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        MessageBox.Show("The Application will be close!", "Can't Connect to MySQL DataBase'", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Environment.Exit(0);
                    }
                }
                MYSQL.openConnection(Host, Port, Database, Username, Password, 100);
        }

        public static void Load()
         {
            string settingsFile = IO.workingDirectory + @"/gsettings.ini";
            if (System.IO.File.Exists(settingsFile) != false)
            {
                ConfigServer.SERVER_KEY_PASSWORD = IO.readINI("Server", "key", settingsFile);
                ConfigServer.SERVER_NAME = IO.readINI("Server", "name", settingsFile);
                ConfigServer.SERVER_IP = IO.readINI("Server", "ip", settingsFile);
                ConfigServer.ClientVersion = Convert.ToInt32(IO.readINI("Server", "clientversion", settingsFile));
                ConfigServer.EXPDinar = double.Parse(IO.readINI("Server", "expdinarrate", settingsFile));
                ConfigServer.Rates = Convert.ToInt32(IO.readINI("Server", "rates", settingsFile));
                ConfigServer.Debug = Convert.ToInt32(IO.readINI("Server", "debug", settingsFile));
                ConfigServer.KillEvent = Convert.ToInt32(IO.readINI("Server", "killevent", settingsFile));
                ConfigServer.CQC = bool.Parse(IO.readINI("Channels", "CQC", settingsFile));
                ConfigServer.BG = bool.Parse(IO.readINI("Channels", "BG", settingsFile));
                ConfigServer.AI = bool.Parse(IO.readINI("Channels", "AI", settingsFile));
                LevelCalculator.SetRates(ConfigServer.Rates);
                string Host = IO.readINI("Database", "host", settingsFile);
                int Port = Convert.ToInt32(IO.readINI("Database", "port", settingsFile));
                string Username = IO.readINI("Database", "username", settingsFile);
                string Password = IO.readINI("Database", "password", settingsFile);
                string Database = IO.readINI("Database", "database", settingsFile);

                OpenConnection();

                sClient = new ServerClient(5010);
                Username = Password = string.Empty;
                FormCalling.frm2.Show();
                
            }
        }
        static void Main()
        {
            DateTime current = DateTime.Now;
            string nTime = String.Format("{0:dd_MM_yy}", current);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Log.setup("Log_" + nTime + ".txt");

            string settingsFile = IO.workingDirectory + @"/gsettings.ini";
            string newitembin = IO.workingDirectory + @"/out.bin";

            if (File.Exists(newitembin))
            {
                File.Delete(IO.workingDirectory + @"/items.bin");
                File.Move(IO.workingDirectory + @"/out.bin", IO.workingDirectory + @"/items.bin");
            }

            if (File.Exists(settingsFile) == false)
            {
                Application.Run(new Form10());
            }

            if (File.Exists(settingsFile) != false)
            {
                ConfigServer.SERVER_KEY_PASSWORD = IO.readINI("Server", "key", settingsFile);
                ConfigServer.SERVER_NAME = IO.readINI("Server", "name", settingsFile);
                ConfigServer.SERVER_IP = IO.readINI("Server", "ip", settingsFile);
                ConfigServer.ClientVersion = Convert.ToInt32(IO.readINI("Server", "clientversion", settingsFile));
                ConfigServer.EXPDinar = double.Parse(IO.readINI("Server", "expdinarrate", settingsFile));
                ConfigServer.Rates = Convert.ToInt32(IO.readINI("Server", "rates", settingsFile));
                ConfigServer.Debug = Convert.ToInt32(IO.readINI("Server", "debug", settingsFile));
                ConfigServer.KillEvent = Convert.ToInt32(IO.readINI("Server", "killevent", settingsFile));
                ConfigServer.CQC = bool.Parse(IO.readINI("Channels", "CQC", settingsFile));
                ConfigServer.BG = bool.Parse(IO.readINI("Channels", "BG", settingsFile));
                ConfigServer.AI = bool.Parse(IO.readINI("Channels", "AI", settingsFile));
                LevelCalculator.SetRates(ConfigServer.Rates);
                string Host = IO.readINI("Database", "host", settingsFile);
                int Port = Convert.ToInt32(IO.readINI("Database", "port", settingsFile));
                string Username = IO.readINI("Database", "username", settingsFile);
                string Password = IO.readINI("Database", "password", settingsFile);
                string Database = IO.readINI("Database", "database", settingsFile);

                OpenConnection();

                Username = Password = string.Empty;

                // check if loginserver is open!
                sClient = new ServerClient(5010);
               /* if (sClient.connect("localhost", 5010) == false)
                {
                    Application.Exit();
                }*/
                Application.Run(new Form2());  
            }
            else Application.Exit();
            
        }
    }
}
