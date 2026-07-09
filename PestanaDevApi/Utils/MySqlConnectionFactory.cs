using MySql.Data.MySqlClient;
using PestanaDevApi.AppConfig;
using PestanaDevApi.Interfaces.Factories;
using System.Data;

namespace PestanaDevApi.Utils
{
    public class MySqlConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        public MySqlConnectionFactory(IConfiguration config)
        {
            _connectionString = DbConfig.GetConnectionString(config);
        }

        public IDbConnection CreateConnection() => new MySqlConnection(_connectionString);
    }
}
