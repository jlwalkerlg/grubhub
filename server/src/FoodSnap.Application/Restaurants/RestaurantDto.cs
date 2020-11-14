using System;

namespace FoodSnap.Application.Restaurants
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
    }
}
