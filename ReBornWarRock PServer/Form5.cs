using ReBornWarRock_PServer.GameServer;
using ReBornWarRock_PServer.GameServer.Managers;
using ReBornWarRock_PServer.GameServer.Networking.Packets;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.Room;
using ReBornWarRock_PServer.GameServer.Virtual_Objects.User;
using ReBornWarRock_PServer.LoginServer.Packets.List_Packets;
using ReBornWarRock_PServer.LoginServer.Virtual.User;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReBornWarRock_PServer
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.Text == "kick" || listBox1.Text == "roominfo" || listBox1.Text == "notice")
            {
                textBox1.Visible = true;
            }
            else
                textBox1.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
           {
               while (Structure._RunningServer)
               {
                   string Command = listBox1.Text;
                   string args = textBox1.Text;
                   switch (listBox1.Text.ToLower())
                   {
                       case "notice":
                           {
                               foreach (virtualUser Client in UserManager.getAllUsers())
                                   Client.send(new PACKET_CHAT("Notice", PACKET_CHAT.ChatType.Notice1, args, 100, "Notice"));
                               return;
                           }
                       case "roominfo":
                           {
                               int RoomID = Convert.ToInt32(args[1]);
                               int RoomChannel = Convert.ToInt32(args[2]);
                               virtualRoom TargetRoom = RoomManager.getRoom(RoomChannel, RoomID);
                               if (TargetRoom == null) return;
                               Log.AppendText("SYSTEM >> Informazioni stanza N° " + RoomID);
                               Log.AppendText("SYSTEM >> Room Nome: " + TargetRoom.Name);
                               Log.AppendText("SYSTEM >> Room Status: " + (TargetRoom.RoomStatus == 2 ? "Play" : "Wait"));
                               Log.AppendText("SYSTEM >> Password: " + TargetRoom.Password + " / MapID: " + TargetRoom.MapID);
                               Log.AppendText("SYSTEM >> Type: " + TargetRoom.RoomType + " / Mode:" + TargetRoom.Mode);
                               Log.AppendText("SYSTEM >> Players: " + TargetRoom.Players.Count + "/" + TargetRoom.MaxPlayers + ", Spectators " + TargetRoom.Spectators.Count + "/10");
                               return;
                           }
                       case "kick":
                           {
                               foreach (virtualUser Client in UserManager.getAllUsers())
                               {
                                   if (Client == null) continue;

                                   if (Client.Nickname.ToLower().Equals(textBox1.Text.ToLower()) || Client.Username.ToLower().Equals(textBox1.Text.ToLower()))
                                   {
                                       Client.disconnect();
                                       Log.AppendText("User " + Client.Nickname + " è stato kikkato dal server!");
                                       return;
                                   }
                               }
                               Log.AppendError("User " + args[1] + " Non online o inesistente!");
                               return;
                           }
                       case "reload":
                           {
                               ItemManager.DecryptBinFile("items.bin");
                               ItemManager.LoadItems();
                               Log.AppendText("SYSTEM --> Item Manager is been reloaded!");
                               virtualMapData.Load();
                               Log.AppendText("SYSTEM --> MapData is been reloaded!");
                               return;
                           }
                       case "stop":
                           {
                               foreach (virtualUser Player in UserManager.getAllUsers())
                               {
                                   Player.send(new PACKET_CHAT("Server", PACKET_CHAT.ChatType.Notice1, "ATTENZIONE: Spiacente, è necessario riavviare il server!!!", 100, "NULL"));
                                   Player.send(new PACKET_CHAT("SYSTEM", PACKET_CHAT.ChatType.Room_ToAll, "SYSTEM: Spiacente, è necessario riavviare il server!!!", 999, "Server"));
                               }
                               Log.AppendText("SYSTEM --> SERVER STOPPING!!");
                               Thread.Sleep(500);
                               Structure.shutDown();
                               return;
                           }
                       case "/":
                           {
                               foreach (User Player in Structure.LogFromLauncher.Values)
                               {
                                    //Player.send(new PACKET_CUSTOM(args));
                                    Player.send(new PACKET_SERVER_LIST(Player));
                               }
                               return;
                           }

                       default:
                           {
                               Log.AppendText("comando sconosciuto, riprova");
                               return;
                           }
                   }
               }
           }
           catch { }
        }

        private void Form5_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormCalling.frm5 = new Form5();
        }
    }
}
