using System;

namespace FoodSnap.Application.Restaurants.GetRestaurantById
{
    public record GetRestaurantByIdQuery : IRequest<RestaurantDto>
    {
        public Guid Id { get; init; }
    }
}
