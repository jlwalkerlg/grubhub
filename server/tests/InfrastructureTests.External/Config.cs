using Microsoft.Extensions.Configuration;

namespace InfrastructureTests.External
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
    }
}
