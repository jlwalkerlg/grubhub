using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Shouldly;
using Web;
using Web.Envelopes;

namespace WebTests
{
    public static class TestExtensions
    {
        public static void ShouldBeSuccessful(this Result result)
        {
            result.IsSuccess.ShouldBe(true);
        }

        public static void ShouldBeAnError(this Result result)
        {
            result.IsSuccess.ShouldBe(false);
        }

        public static void ShouldBe(this HttpStatusCode code, int expected)
        {
            ((int)code).ShouldBe(expected);
        }

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
