using System;
using FoodSnap.Application;

namespace FoodSnap.Web.Queries.GetRestaurantByManagerId
{
    public class GetRestaurantByManagerIdQuery : IRequest<RestaurantDto>
    {
        public GetRestaurantByManagerIdQuery(Guid managerId)
        {
            ManagerId = managerId;
        }

        public Guid ManagerId { get; }
    }
}
