using System;

namespace Application.Restaurants.GetRestaurantById
{
    public record GetRestaurantByIdQuery : IRequest<RestaurantDto>
    {
        public Guid Id { get; init; }
    }
}
