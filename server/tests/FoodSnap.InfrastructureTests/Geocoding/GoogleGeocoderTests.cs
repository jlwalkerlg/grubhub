using System.Threading.Tasks;
using FoodSnap.Domain;
using FoodSnap.Infrastructure.Geocoding;
using Xunit;

namespace FoodSnap.InfrastructureTests.Geocoding
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
            var address = new Address("1 Maine Road, Manchester, UK");

            var coordinatesResult = await geocoder.GetCoordinates(address);

            Assert.True(coordinatesResult.IsSuccess);
            Assert.NotEqual(default(float), coordinatesResult.Value.Latitude);
            Assert.NotEqual(default(float), coordinatesResult.Value.Longitude);
        }

        [Fact]
        [Trait("Category", "ExternalService")]
        public async Task It_Returns_An_Error_If_Geocoding_Fails()
        {
            var address = new Address("random");

            var coordinatesResult = await geocoder.GetCoordinates(address);

            Assert.False(coordinatesResult.IsSuccess);
        }
    }
}
