using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebTests
{
    public static class HttpMessageExtensions
    {
        public static async Task<TData> GetData<TData>(this HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrEmpty(json)) return default;

            return JsonSerializer.Deserialize<TData>(
                json,
                new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                });
        }

        public static string GetErrorMessage(this HttpResponseMessage response)
        {
            var details = response.GetData<ProblemDetails>().Result;
            return details.Detail;
        }

        public static Dictionary<string, string> GetErrors(this HttpResponseMessage response)
        {
            var details = response.GetData<ValidationProblemDetails>().Result;

            var errors = new Dictionary<string, string>();

            foreach (var error in details.Errors)
            {
                errors.Add(error.Key, error.Value.First());
            }

            return errors;
        }
    }
}
