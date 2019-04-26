using System.Collections.Generic;

namespace Models
{
    public class Configuration
    {
        private Dictionary<string, string> connectionStrings;

        public Configuration()
        {
            connectionStrings = new Dictionary<string, string>();
            connectionStrings.Add("SkimaDb", "mongodb://localhost:27017");
        }

        public string GetConnectionString(string name)
        {
            return connectionStrings[name];
        }
    }
}