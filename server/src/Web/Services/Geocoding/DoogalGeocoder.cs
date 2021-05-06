using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using Web.Domain;

namespace Web.Services.Geocoding
{
    public class DoogalGeocoder : IGeocoder
    {
        private readonly HttpClient client = new();

        public async Task<Result<Coordinates>> LookupCoordinates(string postcode)
        {
            var response = await client.GetAsync($"https://www.doogal.co.uk/GetPostcode.ashx?postcode={postcode}");

            if (!response.IsSuccessStatusCode) return Error.BadRequest("Address not recognised.");

            var text = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(text)) return Error.BadRequest("Address not recognised.");

            var parts = text.Split("\t");
            var latitude = float.Parse(parts[1], CultureInfo.InvariantCulture);
            var longitude = float.Parse(parts[2], CultureInfo.InvariantCulture);

            return Result.Ok(new Coordinates(latitude, longitude));
        }
    }
}
