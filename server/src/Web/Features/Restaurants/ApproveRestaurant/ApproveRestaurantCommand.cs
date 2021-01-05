using System;

namespace Web.Features.Restaurants.ApproveRestaurant
{
    public record ApproveRestaurantCommand : IRequest
    {
        public Guid RestaurantId { get; init; }
    }
}
