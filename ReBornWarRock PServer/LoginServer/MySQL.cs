using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ReBornWarRock_PServer.LoginServer;
using MySql.Data.MySqlClient;
using ReBornWarRock_PServer.LoginServer.Docs;

namespace ReBornWarRock_PServer.LoginServer
{
    public static class MYSQL
    {
        private static string strConnection;

        static void dbConnection_StateChange(object usr, StateChangeEventArgs ev)
        {
            if (ev.CurrentState == ConnectionState.Broken)
            {
                System.Threading.Thread.Sleep(1000);
                dbConnection.Close();
            }

            if (ev.CurrentState == ConnectionState.Closed)
            {
                System.Threading.Thread.Sleep(1000);

                ///Log.AppendText("Reconnecting to SQL Server 1...");
                dbConnection = new MySqlConnection(strConnection);
                dbConnection.StateChange += new System.Data.StateChangeEventHandler(dbConnection_StateChange);
                System.Threading.Thread.Sleep(2000);
                dbConnection.Open();
                Log.AppendText("Reconnection to database 1 successful.");
            }
        }

        private static MySqlConnection dbConnection;
        #region Database connection management
        /// <summary> 
        /// Opens connection to the MySQL database with the supplied parameters, and returns a 'true' boolean when the connection has succeeded. Requires MySQL MySql 5.1 driver to be installed. 
        /// </summary> 
        /// <param name="dbHost">The hostname/IP address where the database server is located.</param> 
        /// <param name="dbPort">The port the database server is running on.</param> 
        /// <param name="dbName">The name of the data</param> 
        /// <param name="dbUsername">The username for authentication with the data</param> 
        /// <param name="dbPassword">The pasword for authentication with the data</param> 
        public static bool openConnection(string dbHost, int dbPort, string dbName, string dbUsername, string dbPassword, int dbPoolsize)
        {
            try
            {
                //Message.WriteLine("Connecting to " + dbName + " at " + dbHost + ":" + dbPort + " for user '" + dbUsername + "'");
                strConnection = "Server=" + dbHost + ";Port=" + dbPort + ";Database=" + dbName + ";User=" + dbUsername + ";Password=" + dbPassword + ";Pooling=Yes;Min pool size=0;Max pool size=" + dbPoolsize + ";Connection timeout=1;";
                dbConnection = new MySqlConnection(strConnection);
                dbConnection.StateChange += new System.Data.StateChangeEventHandler(dbConnection_StateChange);
                dbConnection.Open();
                if (dbConnection.State == ConnectionState.Open)
                {
                    FormCalling.frm1.AppendColorLabelBox6("");
                    FormCalling.frm1.AppendLabelBox6("Connected");
                    //Log.AppendText("Connected At DataBase.");
                    return true;
                }
                else
                {
                    Log.AppendError("Failed to connect to " + dbName + " at " + dbHost + ":" + dbPort + " for user '" + dbUsername + "'");
                    return false;
                }
            }

            catch (Exception ex)
            {
                Log.AppendError("Failed to connect! Error thrown was: " + ex.Message);
                return false;
            }
        }
        /// <summary> 
        /// Closes connection with the MySQL data Any errors are ignored. 
        /// </summary> 
        public static void closeConnection()
        {
            Log.AppendText("Closing database connection...");
            try
            {
                dbConnection.Close();
                Log.AppendText("Database connection closed.");
            }
            catch { Log.AppendError("No database connection."); }
        }
        #endregion

        #region Database data manipulation

        public static void runQuery(string query)
        {
            QueryAsync(new Statment(query));
        }

        struct QueryObject<T>
        {
            public Action<T> OriginalCallback;
            public MySqlCommand MySQLCommand;

            public QueryObject(Action<T> callback, MySqlCommand cmd)
            {
                this.OriginalCallback = callback;
                this.MySQLCommand = cmd;
            }
        }

        private static void QueryAsync(Statment statement)
        {
            MySqlConnection c = new MySqlConnection(strConnection);
            c.Open();
            MySqlCommand cmd = c.CreateCommand();
            cmd.CommandText = statement.query;
            foreach (KeyValuePair<string, object> entry in statement.parameters)
                cmd.Parameters.AddWithValue(entry.Key, entry.Value);
            cmd.Prepare();
            cmd.BeginExecuteNonQuery(new AsyncCallback(QueryAsyncCallback), new QueryObject<bool>(null, cmd));
        }

        private static void QueryAsyncCallback(IAsyncResult iAr)
        {
            QueryObject<bool> qo = (QueryObject<bool>)iAr.AsyncState;
            try
            {
                MySqlConnection c = null;
                using (MySqlCommand cmd = qo.MySQLCommand)
                {
                    cmd.EndExecuteNonQuery(iAr);
                    c = cmd.Connection;
                }

                if (c != null)
                    c.Dispose();
                if (qo.OriginalCallback != null)
                    qo.OriginalCallback(true);
            }
            catch
            {
                if (qo.OriginalCallback != null)
                    qo.OriginalCallback(false);
            }
        }

        public static void runQueryNotAsync(string query)
        {
            Query(new Statment(query));
        }
        private static void Query(Statment statement)
        {
            using (MySqlConnection c = new MySqlConnection(strConnection))
            {
                c.Open();
                using (MySqlCommand cmd = c.CreateCommand())
                {
                    cmd.CommandText = statement.query;
                    foreach (KeyValuePair<string, object> entry in statement.parameters)
                        cmd.Parameters.AddWithValue(entry.Key, entry.Value);
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        #endregion

        #region Database data retrieval
        #region runRead
        /// <summary> 
        /// Performs a SQL query and returns the first selected field as string. Other fields are ignored. 
        /// </summary> 
        /// <param name="Query">The SQL query that selects a field.</param> 
        public static DataTable runRead(string Query)
        {
            return Read(new Statment(Query));
        }

        public static object runReadOnce(string var, string Query)
        {
            DataTable dt = Read(new Statment(Query + " LIMIT 1"));
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return row[var].ToString();
            }
            return new object();
        }

        private static DataTable Read(Statment statement)
        {
            DataTable retVal = new DataTable();
            using (MySqlConnection c = new MySqlConnection(strConnection))
            {
                c.Open();
                using (MySqlCommand cmd = c.CreateCommand())
                {
                    cmd.CommandText = statement.query;
                    foreach (KeyValuePair<string, object> entry in statement.parameters)
                        cmd.Parameters.AddWithValue(entry.Key, entry.Value);
                    cmd.Prepare();
                    retVal.Load(cmd.ExecuteReader());
                }
            }
            return retVal;
        }
        #endregion
        #endregion

        #region Data availability checks
        /// <summary> 
        /// Tries to find fields matching the query. When there is at least one result, it returns True and stops. 
        /// </summary> 
        /// <param name="Query">The SQL query that contains the seeked fields and conditions. LIMIT 1 is added.</param> 
        public static bool checkExists(string Query)
        {
            try { return new MySqlCommand(Query + " LIMIT 1", dbConnection).ExecuteReader().HasRows; }
            catch (Exception ex)
            {
                Log.AppendError("Error '" + ex.Message + "' at '" + Query + "'");
                return false;
            }
        }
        #endregion

        #region Misc
        /// <summary> 
        /// Returns a stripslashed copy of the input string.
        /// </summary> 
        /// <param name="Query">The string to add stripslashes to.</param>
        public static string Stripslash(string Query)
        {
            try { return Query.Replace(@"\", "\\").Replace("'", "\'").Replace("'", @"`"); }
            catch { return ""; }
        }
        #endregion
    }
}
