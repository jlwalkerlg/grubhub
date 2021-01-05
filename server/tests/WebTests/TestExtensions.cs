using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
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
