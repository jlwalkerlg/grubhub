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
        public async Task It_Converts_An_Address_Into_Coordinates()
        {
            var address = new Address(
                "19 Bodmin Avenue",
                "Wrose",
                "Shipley",
                new Postcode("BD181LT"));

            var coordinatesResult = await geocoder.GetCoordinates(address);

            Assert.True(coordinatesResult.IsSuccess);
            Assert.NotEqual(default(float), coordinatesResult.Value.Latitude);
            Assert.NotEqual(default(float), coordinatesResult.Value.Longitude);
        }
    }
}
