using System.Data;
using System.Net;
using System.Security;
using PestanaDevApi.Interfaces.Factories;
using PestanaDevApi.QuartzJobs;
using PestanaDevApi.Utils;
using Consts = PestanaDevApi.Constants.QuartzConstants;
using Quartz;

namespace PestanaDevApi.AppConfig
{
    public class DbConfig
    {
        public static void Setup(IConfiguration configuration, IServiceCollection services)
        {
            services.AddSingleton<IDbConnectionFactory, MySqlConnectionFactory>();
        }

        public static void SetupQuartz(IConfiguration configuration, IServiceCollection services)
        {
            string connectionString = GetConnectionString(configuration, isQuartz: true);

            services.AddQuartz(options =>
            {
                options.Properties["quartz.serializer.type"] = "json";

                options.SchedulerName = Consts.SchedulerName;
                options.SchedulerId = Consts.SchedulerId;

                options.UseDefaultThreadPool(tp =>
                {
                    tp.MaxConcurrency = 10;
                });

                options.UsePersistentStore(store =>
                {
                    store.UseProperties = true;
                    store.UseMySql(connectionString);
                });

                JobKey DeleteUnfreshConfirmationCodesJobKey = JobKey.Create(nameof(DeleteUnfreshConfirmationCodesJob));

                options.AddJob<DeleteUnfreshConfirmationCodesJob>(DeleteUnfreshConfirmationCodesJobKey)
                .AddTrigger(trigger => trigger.ForJob(DeleteUnfreshConfirmationCodesJobKey)
                .WithCronSchedule(Consts.DeleteUnfreshConfirmationCodesJob, cron =>
                {
                    cron.InTimeZone(TimeZoneInfo.Utc);
                })); // Every 15 minutes

                JobKey DeleteExpiredRefreshTokensJobKey = JobKey.Create(nameof(DeleteExpiredRefreshTokensJob));

                options.AddJob<DeleteExpiredRefreshTokensJob>(DeleteExpiredRefreshTokensJobKey)
                .AddTrigger(trigger => trigger.ForJob(DeleteExpiredRefreshTokensJobKey)
                .WithCronSchedule(Consts.DeleteExpiredRefreshTokensJob, cron =>
                {
                    cron.InTimeZone(TimeZoneInfo.Utc);
                })); // Every day at 3 AM
            });

            services.AddQuartzHostedService(options => 
            { 
                options.WaitForJobsToComplete = true;
                options.AwaitApplicationStarted = true;
            });
        }

        /// <summary>
        /// Monta a connection string para conexão ao banco
        /// </summary>
        /// <param name="configuration"></param>
        /// <exception cref="ArgumentException"></exception>
        public static string GetConnectionString(IConfiguration configuration, bool isQuartz = false)
        {
            (string host, string port, string user, string database, SecureString pass) = GetDefaultParams(configuration, isQuartz);
            
            string connectionString = $"Server={host}; Port={port}; Database={database}; Uid={user}; Pwd={new NetworkCredential("", pass).Password}; SslMode=Preferred; Pooling=true; Minimum Pool Size=0; Maximum Pool Size=100; Connection Timeout=15;";

            pass.Dispose();

            return connectionString;
        }

        #region Private Methods
        private static (string host, string port, string user, string dataBase, SecureString pass) GetDefaultParams(IConfiguration configuration, bool isQuartz = false)
        {
            string[] urlsplitted = configuration["database.url"]!.Split(":");
            string host = urlsplitted.ElementAtOrDefault(0) ?? throw new ArgumentException("Database host is required");
            string port = urlsplitted.ElementAtOrDefault(1) ?? throw new ArgumentException("Database port is required");

            string user = configuration["database.username"] ?? throw new ArgumentException("Database username is required");
            SecureString pass = new NetworkCredential("", configuration["database.password"] ?? throw new ArgumentException("Database password is required")).SecurePassword;
            string database = (isQuartz ? configuration["database.quartz.databasename"] : configuration["database.databasename"]) ?? throw new ArgumentException("Database name is required");

            return (host, port, user, database, pass);
        }
        #endregion
    }
}
