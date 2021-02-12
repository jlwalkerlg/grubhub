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
            var config = new Config()
            {
                GoogleGeocodingApiKey = TestConfig.GoogleGeocodingApiKey,
            };

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
        public async Task Geocode_Finds_Address_Details_Coordinates()
        {
            var result = await geocoder.Geocode(
                new AddressDetails(
                    "19 Bodmin Avenue",
                    "Wrose",
                    "Shipley",
                    "BD18 1LT"
                )
            );

            result.ShouldBeSuccessful();
            result.Value.FormattedAddress.ShouldNotBeNull();
            result.Value.Coordinates.Latitude.ShouldNotBe(default);
            result.Value.Coordinates.Longitude.ShouldNotBe(default);
        }

        [Theory]
        [InlineData("Not a valid address", null, "Shipley", "BD18 1LT")]
        [InlineData("19 Bodmin Avenue", "Wrose", "Shipley", "MN12 1NM")]
        public async Task Geocode_Fails_If_Address_Details_Geocoding_Fails(
            string line1, string line2, string city, string postcode)
        {
            var result = await geocoder.Geocode(
                new AddressDetails(
                    line1,
                    line2,
                    city,
                    postcode
                )
            );

            result.ShouldBeAnError();
        }
    }
}
