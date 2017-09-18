using System.Collections.Generic;

namespace ReBornWarRock_PServer.LoginServer.Docs
{
    public class Statment
    {
        public string query { get; set; }
        public readonly Dictionary<string, object> parameters = new Dictionary<string, object>();
        public Statment(string query)
        {
            this.query = query;
        }

        public void AddValue(string key, object value)
        {
            if (!this.parameters.ContainsKey(key))
            {
                this.parameters.Add(key, value);
            }
        }
    }
}
