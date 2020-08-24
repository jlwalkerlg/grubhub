using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodSnap.Application.Services.Geocoding;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using FoodSnap.Application;

namespace FoodSnap.Infrastructure.Geocoding
{
    public class GoogleGeocoder : IGeocoder
    {
        private readonly string key;

        public GoogleGeocoder(string key)
        {
            this.key = key;
        }

        public async Task<Result<CoordinatesDto>> GetCoordinates(AddressDto address)
        {
            var formattedAddress = FormatAddress(address);
            var response = await SendRequest(formattedAddress);
            var json = ConvertResponseToJson(response);

            var jobj = (JObject)JsonConvert.DeserializeObject(json);

            var status = (string)jobj["status"];

            if (status != "OK")
            {
                Result.Fail(new Error(status));
            }

            var result = jobj["results"][0];

            return Result.Ok(new CoordinatesDto
            {
                Latitude = (float)result["geometry"]["location"]["lat"],
                Longitude = (float)result["geometry"]["location"]["lat"]
            });
        }

        private string FormatAddress(AddressDto address)
        {
            var components = new List<string>
            {
                address.Line1,
                address.Line2,
                address.Town,
                address.Postcode,
            }
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();

            return string.Join(", ", components);
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
