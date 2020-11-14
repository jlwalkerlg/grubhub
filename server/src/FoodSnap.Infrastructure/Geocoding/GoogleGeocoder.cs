using System.Net;
using System.Threading.Tasks;
using FoodSnap.Application.Services.Geocoding;
using System.IO;
using FoodSnap.Application;
using System.Text.Json;
using System.Linq;

namespace FoodSnap.Infrastructure.Geocoding
{
    public class GoogleGeocoder : IGeocoder
    {
        private readonly string key;

        public GoogleGeocoder(string key)
        {
            this.key = key;
        }

        public async Task<Result<GeocodingResult>> Geocode(string address)
        {
            var response = await SendRequest(address);
            var json = ConvertResponseToJson(response);
            var doc = JsonDocument.Parse(json);

            var status = doc.RootElement.GetProperty("status").GetString();

            if (status != "OK")
            {
                return Result<GeocodingResult>.Fail(Error.Internal(status));
            }

            var result = doc.RootElement
                .GetProperty("results")
                .EnumerateArray()
                .First();

            var location = result.GetProperty("geometry").GetProperty("location");

            return Result.Ok(new GeocodingResult
            {
                FormattedAddress = result.GetProperty("formatted_address").GetString(),
                Latitude = (float)location.GetProperty("lat").GetDouble(),
                Longitude = (float)location.GetProperty("lng").GetDouble(),
            });
        }

        private Task<WebResponse> SendRequest(string formattedAddress)
        {
            var url = string.Format(
                "https://maps.googleapis.com/maps/api/geocode/json?address={0}&components=country:GB&key={1}",
                WebUtility.UrlEncode(formattedAddress),
                key);

            var request = WebRequest.Create(url);
            request.Method = "GET";

            return request.GetResponseAsync();
        }

        private string ConvertResponseToJson(WebResponse response)
        {
            using (var stream = response.GetResponseStream())
            {
                var reader = new StreamReader(stream);
                return reader.ReadToEnd();
            }
        }
    }
}
