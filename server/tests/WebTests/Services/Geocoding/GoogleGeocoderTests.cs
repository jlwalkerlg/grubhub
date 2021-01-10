using Shouldly;
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

            result.ShouldBeSuccessful();
            result.Value.FormattedAddress.ShouldNotBeNull();
            result.Value.Coordinates.Latitude.ShouldNotBe(default(float));
            result.Value.Coordinates.Longitude.ShouldNotBe(default(float));
        }

        [Fact]
        public async Task It_Returns_An_Error_If_Geocoding_Fails()
        {
            var result = await geocoder.Geocode("not_a_real_address");

            result.ShouldBeAnError();
        }
    }
}
