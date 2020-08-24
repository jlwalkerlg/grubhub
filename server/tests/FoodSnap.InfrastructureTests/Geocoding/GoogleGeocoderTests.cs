using System.Threading.Tasks;
using FoodSnap.Application.Services.Geocoding;
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
            var address = new AddressDto
            {
                Line1 = "19 Bodmin Avenue",
                Line2 = "Wrose",
                Town = "Shipley",
                Postcode = "BD181LT"
            };

            var coordinates = await geocoder.GetCoordinates(address);

            Assert.NotEqual(default(float), coordinates.Latitude);
            Assert.NotEqual(default(float), coordinates.Longitude);
        }
    }
}
