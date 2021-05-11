using System.Threading.Tasks;
using Shouldly;
using Web.Services.Geocoding;
using Xunit;

namespace WebTests.Services.Geocoding
{
    public class DoogalGeocodingTests
    {
        private readonly DoogalGeocoder geocoder = new();

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
