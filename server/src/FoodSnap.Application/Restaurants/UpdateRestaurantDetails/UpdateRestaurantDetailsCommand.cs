using System;
using FoodSnap.Application.Services.Authentication;

namespace FoodSnap.Application.Restaurants.UpdateRestaurantDetails
{
    [Authenticate]
    public record UpdateRestaurantDetailsCommand : IRequest
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string PhoneNumber { get; init; }
    }
}
