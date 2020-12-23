using System;

namespace Application.Restaurants.ApproveRestaurant
{
    public record ApproveRestaurantCommand : IRequest
    {
        public Guid RestaurantId { get; init; }
    }
}
