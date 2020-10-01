using System.Net;
using System.Threading.Tasks;
using FoodSnap.Application.Services.Geocoding;
using System.IO;
using Newtonsoft.Json.Linq;
using FoodSnap.Application;
using FoodSnap.Domain;

namespace FoodSnap.Infrastructure.Geocoding
{
    public class GoogleGeocoder : IGeocoder
    {
        private readonly string key;

        public GoogleGeocoder(string key)
        {
            this.key = key;
        }

        public async Task<Result<Coordinates>> GetCoordinates(Address address)
        {
            var response = await SendRequest(address.Value);
            var json = ConvertResponseToJson(response);

            var jobj = JObject.Parse(json);

            var status = (string)jobj["status"];

            if (status != "OK")
            {
                return Result<Coordinates>.Fail(Error.ServerError(status));
            }

            var result = jobj["results"][0];

            return Result.Ok(
                new Coordinates(
                    (float)result["geometry"]["location"]["lat"],
                    (float)result["geometry"]["location"]["lng"]));
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
