using System;

namespace FoodSnap.Application.Restaurants.ApproveRestaurant
{
    public class ApproveRestaurantCommand : IRequest
    {
        public Guid RestaurantId { get; set; }
    }
}
