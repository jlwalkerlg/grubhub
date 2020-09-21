using System;
using FoodSnap.Application;

namespace FoodSnap.Web.Queries.Restaurants.GetRestaurantByManagerId
{
    public class GetRestaurantByManagerIdQuery : IRequest<RestaurantDto>
    {
        public Guid ManagerId { get; set; }
    }
}
