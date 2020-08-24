using Microsoft.Extensions.Configuration;

namespace FoodSnap.InfrastructureTests
{
    public static class ConfigurationFactory
    {
        private static IConfigurationRoot configuration;

        public static IConfigurationRoot Make()
        {
            if (configuration == null)
            {
                configuration = new ConfigurationBuilder()
                    .AddJsonFile("config.json")
                    .Build();
            }

            return configuration;
        }
    }
}
