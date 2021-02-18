using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Web.Envelopes;

namespace WebTests
{
    public static class HttpMessageExtensions
    {
        public static async Task<TData> GetData<TData>(this HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStreamAsync();

            var envelope = await JsonSerializer.DeserializeAsync<DataEnvelope<TData>>(
                json,
                new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                });

            return envelope.Data;
        }

        private static ErrorEnvelope GetErrorEnvelope(this HttpResponseMessage response)
        {
            var json = response.Content.ReadAsStringAsync().Result;
            return JsonSerializer.Deserialize<ErrorEnvelope>(
                json,
                new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                });
        }

        public static string GetErrorMessage(this HttpResponseMessage response)
        {
            var envelope = response.GetErrorEnvelope();
            return envelope.Message;
        }

        public static Dictionary<string, string> GetErrors(this HttpResponseMessage response)
        {
            var envelope = response.GetErrorEnvelope();
            return envelope.Errors;
        }
    }
}
