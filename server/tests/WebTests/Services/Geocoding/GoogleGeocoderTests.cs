using System.Threading.Tasks;
using Web.Services.Geocoding;
using Xunit;

namespace WebTests.Services.Geocoding
{
    public class GoogleGeocoderTests
    {
        private readonly GoogleGeocoder geocoder;

        public GoogleGeocoderTests()
        {
            geocoder = new GoogleGeocoder(TestConfig.GoogleGeocodingApiKey);
        }

        [Fact]
        public async Task It_Converts_An_Address_Into_Coordinates()
        {
            var result = await geocoder.Geocode("1 Maine Road, Manchester, UK");

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value.FormattedAddress);
            Assert.NotEqual(default, result.Value.Coordinates.Latitude);
            Assert.NotEqual(default, result.Value.Coordinates.Longitude);
        }

        [Fact]
        public async Task It_Returns_An_Error_If_Geocoding_Fails()
        {
            var coordinatesResult = await geocoder.Geocode("random");

            Assert.False(coordinatesResult.IsSuccess);
        }
    }
}
