using Microsoft.Extensions.Configuration;

namespace WebTests
{
    public class TestConfig
    {
        private static readonly IConfigurationRoot config;

        static TestConfig()
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
