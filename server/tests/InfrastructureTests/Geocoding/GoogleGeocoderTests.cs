using System.Threading.Tasks;
using Infrastructure.Geocoding;
using Xunit;

namespace InfrastructureTests.Geocoding
{
    public class GoogleGeocoderTests
    {
        private readonly GoogleGeocoder geocoder;

        public GoogleGeocoderTests()
        {
            var config = ConfigurationFactory.Make();
            geocoder = new GoogleGeocoder(config["GoogleGeocodingApiKey"]);
        }

        [Fact]
        [Trait("Category", "ExternalService")]
        public async Task It_Converts_An_Address_Into_Coordinates()
        {
            var result = await geocoder.Geocode("1 Maine Road, Manchester, UK");

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value.FormattedAddress);
            Assert.NotEqual(default(float), result.Value.Coordinates.Latitude);
            Assert.NotEqual(default(float), result.Value.Coordinates.Longitude);
        }

        [Fact]
        [Trait("Category", "ExternalService")]
        public async Task It_Returns_An_Error_If_Geocoding_Fails()
        {
            var coordinatesResult = await geocoder.Geocode("random");

            Assert.False(coordinatesResult.IsSuccess);
        }
    }
}
