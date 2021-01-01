using Microsoft.Extensions.Configuration;

namespace SharedTests
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

        public static string GoogleGeocodingApiKey => config["GoogleGeocodingApiKey"];

        public static string InfrastructureTestDbConnectionString =>
            config["InfrastructureTestDbConnectionString"];

        public static string WebTestDbConnectionString =>
            config["WebTestDbConnectionString"];
    }
}
