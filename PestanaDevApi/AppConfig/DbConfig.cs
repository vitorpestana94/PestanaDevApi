using System.Data;
using System.Net;
using System.Security;
using MySqlConnector;

namespace PestanaDevApi.AppConfig
{
    public class DbConfig
    {
        public static void Setup(IConfiguration configuration, IServiceCollection services)
        {
            string connectionString = MountConnectionString(configuration);

            services.AddScoped<IDbConnection>(opt => new MySqlConnection(connectionString));
        }

        /// <summary>
        /// Monta a connection string para conexão ao banco
        /// </summary>
        /// <param name="configuration"></param>
        /// <exception cref="ArgumentException"></exception>
        private static string MountConnectionString(IConfiguration configuration)
        {
            string[] urlsplitted = configuration["database.url"]!.Split(":");
            string host = urlsplitted.ElementAtOrDefault(0) ?? throw new ArgumentException("Database host is required");
            string port = urlsplitted.ElementAtOrDefault(1) ?? throw new ArgumentException("Database port is required");

            string user = configuration["database.username"] ?? throw new ArgumentException("Database username is required");
            SecureString pass = new NetworkCredential("", configuration["database.password"] ?? throw new ArgumentException("Database password is required")).SecurePassword;
            string database = configuration["database.databasename"] ?? throw new ArgumentException("Database name is required");

            string connectionString = $"Server={host}; Port={port}; Database={database}; Uid={user}; Pwd={new NetworkCredential("", pass).Password}; SslMode=None;";
            pass.Dispose();

            return connectionString;
        }
    }
}
