using System;

namespace FoodSnap.Application.Restaurants.ApproveRestaurant
{
    public record ApproveRestaurantCommand : IRequest
    {
        public Guid RestaurantId { get; init; }
    }
}
