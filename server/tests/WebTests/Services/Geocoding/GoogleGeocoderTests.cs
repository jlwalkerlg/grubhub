using Shouldly;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Web;
using Web.Services.Geocoding;
using Xunit;
using System.Net.Http;
using System;

namespace WebTests.Services.Geocoding
{
    public class GoogleGeocoderTests : IClassFixture<TestWebApplicationFactory>, IDisposable
    {
        private readonly GoogleGeocoder geocoder;
        private readonly HttpClient httpClient = new();

        public GoogleGeocoderTests(TestWebApplicationFactory factory)
        {
            var settings = factory.Services.GetRequiredService<GeocodingSettings>();

            geocoder = new GoogleGeocoder(settings, httpClient);
        }

        public void Dispose()
        {
            httpClient.Dispose();
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
