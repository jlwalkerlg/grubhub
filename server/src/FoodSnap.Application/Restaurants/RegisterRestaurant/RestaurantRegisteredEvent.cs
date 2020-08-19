using System;
using FoodSnap.Application.Events;

namespace FoodSnap.Application.Restaurants.RegisterRestaurant
{
    public class RestaurantRegisteredEvent : Event
    {
        public Guid RestaurantId { get; }
        public Guid ManagerId { get; }

        public RestaurantRegisteredEvent(Guid restaurantId, Guid managerId)
        {
            RestaurantId = restaurantId;
            ManagerId = managerId;
        }
    }
}
