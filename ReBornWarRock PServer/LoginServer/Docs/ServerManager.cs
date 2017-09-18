using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using ReBornWarRock_PServer.LoginServer.Virtual.Server;
using System.Data;

namespace ReBornWarRock_PServer.LoginServer.Docs
{
    class ServerManager
    {
        private static Hashtable _Servers = new Hashtable();
        private static HashSet<int> _ActiveServers;
        private static int _Limit = 0;

        public static void setup(int Limit)
        {
            _Limit = Limit;
            _ActiveServers = new HashSet<int>();

        }

        public static ArrayList getServers()
        {
            return new ArrayList(_Servers.Values);
        }

        public static bool addServer(Server Server)
        {
            int ServerID = 0;

            for (int I = 1; I < _Limit; I++)
            {
                if (_ActiveServers.Contains(I) == false)
                {
                    ServerID = I;
                    break;
                }
            }

            if (ServerID > 0)
            {
                Server.newPacket(ServerID);
                _Servers.Add(ServerID, Server);
                Log.AppendText("Server ID: " + ServerID + " added to the server pool!");
                return true;
            }

            return false;
        }

        public static void removeServer(int ID)
        {
            if (_Servers.ContainsKey(ID))
            {
                _Servers.Remove(ID);
            }
        }
    }

    class ServersInformations
    {
        public static Dictionary<int, Servers> collected = new Dictionary<int, Servers>();

        public static void LOAD()
        {
            if (FormCalling.frm1.checkBox1.Checked) return;
            try
            {
                DataTable Serv = MYSQL.runRead("SELECT * FROM servers WHERE visible='1' ORDER BY serverid ASC");
            for (int i = 0; i < Serv.Rows.Count; i++)
            {
                DataRow row = Serv.Rows[i];
                int serverId = int.Parse(row["serverid"].ToString());
                string name = row["name"].ToString();
                string ip = row["ip"].ToString();
                int flag = int.Parse(row["flag"].ToString());
                int minrank = int.Parse(row["minrank"].ToString());
                int slot = int.Parse(row["slot"].ToString());

                Servers s = new Servers(serverId, name, ip, flag, minrank, slot);

                collected.Add(i, s);
            }
            Log.AppendText("Loaded " + Serv.Rows.Count + " servers!");
            }
            catch { Log.AppendError("Can't Read Server Infos Check Server Table On DataBase"); }
        }
    }

    /// <summary>
    /// Server Informations such as ip, flag, min rank for access are stored here
    /// </summary>
    class Servers
    {
        public int id;
        public string name;
        public string ip;
        public int flag;
        public int minrank;
        public int slot;

        public Servers(int serverid, string name, string ip, int flag, int rank, int slot)
        {
            this.id = serverid;
            this.name = name;
            this.ip = ip;
            this.flag = flag;
            this.minrank = rank;
            this.slot = slot;
        }
    }


}
