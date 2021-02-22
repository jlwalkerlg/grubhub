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
        public async Task Geocode_Finds_Address_Coordinates()
        {
            var result = await geocoder.Geocode("1 Maine Road, Manchester, UK");

            result.ShouldBeSuccessful();
            result.Value.FormattedAddress.ShouldNotBeNull();
            result.Value.Coordinates.Latitude.ShouldNotBe(default);
            result.Value.Coordinates.Longitude.ShouldNotBe(default);
        }

        [Fact]
        public async Task Geocode_Fails_If_Address_Geocoding_Fails()
        {
            var result = await geocoder.Geocode("not_a_real_address");

            result.ShouldBeAnError();
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
