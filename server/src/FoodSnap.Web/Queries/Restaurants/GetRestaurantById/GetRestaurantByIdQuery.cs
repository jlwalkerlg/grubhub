using System;
using FoodSnap.Application;

namespace FoodSnap.Web.Queries.Restaurants.GetRestaurantById
{
    public class GetRestaurantByIdQuery : IRequest<RestaurantDto>
    {
        public Guid Id { get; set; }
    }
}
