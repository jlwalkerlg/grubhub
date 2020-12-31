using Microsoft.Extensions.Configuration;

namespace InfrastructureTests
{
    public class Config
    {
        static IConfigurationRoot config;

        static Config()
        {
            config = new ConfigurationBuilder()
                .AddJsonFile("config.json")
                .Build();
        }

        public static string TestDbConnectionString => config["InfrastructureTestDbConnectionString"];
    }
}
