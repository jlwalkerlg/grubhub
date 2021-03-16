using System;
using System.Collections.Generic;

namespace Web.Features.Restaurants.SearchRestaurants
{
    public class SearchRestaurantsResponse
    {
        public List<RestaurantModel> Restaurants { get; init; }
        public int Count { get; init; }

        public class RestaurantModel
        {
            public Guid Id { get; init; }
            public string Name { get; init; }
            public float Latitude { get; init; }
            public float Longitude { get; init; }
            public OpeningTimesDto OpeningTimes { get; init; } = new();
            public decimal DeliveryFee { get; init; }
            public decimal MinimumDeliverySpend { get; init; }
            public int MaxDeliveryDistanceInKm { get; init; }
            public int EstimatedDeliveryTimeInMinutes { get; init; }
            public List<CuisineDto> Cuisines { get; init; } = new();
        }

        public class CuisineDto
        {
            public string Name { get; init; }
        }
    }
}
