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
            // TODO: use a shared base class that sets up DI container instead of this
            var settings = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Testing.json")
                .Build()
                .GetSection("Geocoding")
                .Get<GeocodingSettings>();

            geocoder = new GoogleGeocoder(settings);
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
