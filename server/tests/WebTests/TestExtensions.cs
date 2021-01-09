using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Shouldly;
using Web;
using Web.Domain.Restaurants;
using Web.Envelopes;
using Web.Features.Restaurants;

namespace WebTests
{
    public static class TestExtensions
    {
        public static bool IsEqual(this Restaurant restaurant, RestaurantDto restaurantDto)
        {
            if (restaurant.Id.Value != restaurantDto.Id) return false;

            foreach (var cuisine in restaurant.Cuisines)
            {
                if (!restaurantDto.Cuisines.Any(x => x.Name == cuisine.Name))
                {
                    return false;
                }
            }

            return true;
        }

        public static void ShouldBe(this HttpStatusCode code, int expected)
        {
            ((int)code).ShouldBe(expected);
        }

        public static void ShouldBeAnError(this Result result)
        {
            result.IsSuccess.ShouldBe(false);
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
