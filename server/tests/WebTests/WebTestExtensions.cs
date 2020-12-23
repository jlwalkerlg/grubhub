using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Web.Envelopes;

namespace WebTests
{
    public static class WebTestExtensions
    {
        private static async Task<ErrorEnvelope> ToErrorEnvelope(this HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ErrorEnvelope>(json, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            });
        }

        public static async Task<string> GetErrorMessage(this HttpResponseMessage response)
        {
            var envelope = await response.ToErrorEnvelope();
            return envelope.Message;
        }

        public static async Task<Dictionary<string, string>> GetErrors(this HttpResponseMessage response)
        {
            var envelope = await response.ToErrorEnvelope();
            return envelope.Errors;
        }
    }
}
