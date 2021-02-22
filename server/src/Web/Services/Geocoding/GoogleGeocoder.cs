using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Web.Domain;

namespace Web.Services.Geocoding
{
    public class GoogleGeocoder : IGeocoder
    {
        private readonly string key;

        public GoogleGeocoder(Config config)
        {
            key = config.GoogleGeocodingApiKey;
        }

        public async Task<Result<GeocodingResult>> Geocode(string address)
        {
            var url = string.Format(
                "https://maps.googleapis.com/maps/api/geocode/json?address={0}&components=country:GB&key={1}",
                WebUtility.UrlEncode(address),
                key);

            var json = await GetResponseAsJson(url);
            var doc = JsonDocument.Parse(json);

            var status = doc.RootElement.GetProperty("status").GetString();

            if (status != "OK")
            {
                return Error.BadRequest("Address not recognised.");
            }

            var result = doc.RootElement
                .GetProperty("results")
                .EnumerateArray()
                .First();

            var location = result.GetProperty("geometry").GetProperty("location");

            return Result.Ok(
                new GeocodingResult()
                {
                    FormattedAddress = result.GetProperty("formatted_address").GetString(),
                    Coordinates = new Coordinates(
                        (float)location.GetProperty("lat").GetDouble(),
                        (float)location.GetProperty("lng").GetDouble()),
                });
        }

        public async Task<Result<Coordinates>> LookupCoordinates(string postcode)
        {
            var url = string.Format(
                "https://maps.googleapis.com/maps/api/geocode/json?components=postal_code={0}|country:GB&key={1}",
                WebUtility.UrlEncode(postcode),
                key);

            var json = await GetResponseAsJson(url);
            var doc = JsonDocument.Parse(json);

            var status = doc.RootElement.GetProperty("status").GetString();

            if (status != "OK")
            {
                return Error.BadRequest("Address not recognised.");
            }

            var result = doc.RootElement
                .GetProperty("results")
                .EnumerateArray()
                .First();

            var location = result.GetProperty("geometry").GetProperty("location");

            return Result.Ok(new Coordinates(
                (float)location.GetProperty("lat").GetDouble(),
                (float)location.GetProperty("lng").GetDouble())
            );
        }

        private async Task<string> GetResponseAsJson(string url)
        {
            var request = WebRequest.Create(url);
            request.Method = "GET";

            var response = await request.GetResponseAsync();

            using (var stream = response.GetResponseStream())
            {
                var reader = new StreamReader(stream);
                return reader.ReadToEnd();
            }
        }
    }
}
