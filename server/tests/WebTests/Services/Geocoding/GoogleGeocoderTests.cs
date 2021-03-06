using Microsoft.Extensions.Configuration;
using Shouldly;
using System.Threading.Tasks;
using Web;
using Web.Services.Geocoding;
using Xunit;

namespace WebTests.Services.Geocoding
{
    public class GoogleGeocoderTests
    {
        private readonly GoogleGeocoder geocoder;

        public GoogleGeocoderTests()
        {
            var config = new Config();

            new ConfigurationBuilder()
                .AddJsonFile("config.json")
                .Build()
                .Bind(config);

            geocoder = new GoogleGeocoder(config);
        }

        [Fact]
        public async Task LookupCoordinates_Finds_Coordinates()
        {
            var result = await geocoder.LookupCoordinates("BD18 1LT");

            result.ShouldBeSuccessful();
            result.Value.Latitude.ShouldNotBe(default);
            result.Value.Longitude.ShouldNotBe(default);
        }
    }
}
