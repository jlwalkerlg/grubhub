using Microsoft.Extensions.Configuration;

namespace FoodSnap.InfrastructureTests
{
    public static class ConfigurationFactory
    {
        private static IConfiguration configuration;

        public static IConfiguration Make()
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
