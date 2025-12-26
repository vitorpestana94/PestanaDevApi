using PestanaDevApi.Utils;

namespace PestanaDevApi.AppConfig
{
    public class LocalSecretManagerConfig
    {
        /// <summary>
        /// </summary>
        /// <param name="environment"></param>
        /// <param name="config"></param>
        public static void Setup(string environment, IConfigurationBuilder config)
        {
            string[] devEnvironments = ["Development", "Local"];
            string fileName = devEnvironments.Contains(environment) ? $"config.{environment}.properties" : "config.properties";

            Dictionary<string, string> allConfigs = new();
            TxtFileHelper.ReadPropertyFile(ref allConfigs, "app", fileName);
            TxtFileHelper.ReadPropertyFile(ref allConfigs, "app-secret", fileName);

            config.AddInMemoryCollection(allConfigs.Select(kvp => new KeyValuePair<string, string?>(kvp.Key, kvp.Value)));
        }
    }
}
