using System;
using FoodSnap.Application.Events;

namespace FoodSnap.Application.Restaurants
{
    public class RestaurantAcceptedEvent : Event
    {
        public RestaurantAcceptedEvent(Guid restaurantId)
        {
            RestaurantId = restaurantId;
        }

        public Guid RestaurantId { get; }
    }
}
