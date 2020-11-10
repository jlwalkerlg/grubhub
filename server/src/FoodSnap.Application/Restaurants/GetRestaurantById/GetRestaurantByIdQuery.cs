using System;

namespace FoodSnap.Application.Restaurants.GetRestaurantById
{
    public class GetRestaurantByIdQuery : IRequest<RestaurantDto>
    {
        public Guid Id { get; set; }
    }
}
