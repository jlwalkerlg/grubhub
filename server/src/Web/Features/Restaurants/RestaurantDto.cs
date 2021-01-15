using System;
using System.Collections.Generic;
using Web.Features.Cuisines;
using Web.Features.Menus;

namespace Web.Features.Restaurants
{
    public record RestaurantDto
    {
        public Guid Id { get; init; }
        public Guid ManagerId { get; init; }
        public string Name { get; init; }
        public string PhoneNumber { get; init; }
        public string Address { get; init; }
        public float Latitude { get; init; }
        public float Longitude { get; init; }
        public string Status { get; init; }
        public OpeningTimesDto OpeningTimes { get; init; } = new();
        public decimal DeliveryFee { get; init; }
        public decimal MinimumDeliverySpend { get; init; }
        public int MaxDeliveryDistanceInKm { get; init; }
        public int EstimatedDeliveryTimeInMinutes { get; init; }
        public List<CuisineDto> Cuisines { get; init; } = new();
        public MenuDto Menu { get; init; }
    }
}
