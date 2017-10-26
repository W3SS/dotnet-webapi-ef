using System.Configuration;

namespace Common
{
    public class Configuration
    {
        public static string GetConnectionStringForKey(string key)
        {
            var connectionString = ConfigurationManager.ConnectionStrings[key].ConnectionString;

            return string.IsNullOrEmpty(connectionString) ? string.Empty : connectionString;
        }
    }
}
