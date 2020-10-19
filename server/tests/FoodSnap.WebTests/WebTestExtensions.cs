using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using FoodSnap.Web.Envelopes;

namespace FoodSnap.WebTests
{
    public static class WebTestExtensions
    {
        public static async Task<ErrorEnvelope> ToErrorEnvelope(this HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ErrorEnvelope>(json, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            });
        }
    }
}
