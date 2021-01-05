using System;

namespace Web.Features.Restaurants.GetRestaurantById
{
    public record GetRestaurantByIdQuery : IRequest<RestaurantDto>
    {
        public Guid Id { get; init; }
    }
}
