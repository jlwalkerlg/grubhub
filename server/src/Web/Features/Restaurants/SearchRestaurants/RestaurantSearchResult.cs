using System;
using System.Collections.Generic;
using Web.Features.Cuisines;

namespace Web.Features.Restaurants.SearchRestaurants
{
    public record RestaurantSearchResult
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
}
