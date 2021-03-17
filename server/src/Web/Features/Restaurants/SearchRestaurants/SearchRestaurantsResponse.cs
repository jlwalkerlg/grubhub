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
            public OpeningTimesModel OpeningTimes { get; init; } = new();
            public decimal DeliveryFee { get; init; }
            public decimal MinimumDeliverySpend { get; init; }
            public int MaxDeliveryDistanceInKm { get; init; }
            public int EstimatedDeliveryTimeInMinutes { get; init; }
            public List<CuisineDto> Cuisines { get; init; } = new();
        }

        public class OpeningTimesModel
        {
            public OpeningHoursModel Monday { get; init; }
            public OpeningHoursModel Tuesday { get; init; }
            public OpeningHoursModel Wednesday { get; init; }
            public OpeningHoursModel Thursday { get; init; }
            public OpeningHoursModel Friday { get; init; }
            public OpeningHoursModel Saturday { get; init; }
            public OpeningHoursModel Sunday { get; init; }
        }

        public class OpeningHoursModel
        {
            public string Open { get; init; }
            public string Close { get; init; }
        }

        public class CuisineDto
        {
            public string Name { get; init; }
        }
    }
}
