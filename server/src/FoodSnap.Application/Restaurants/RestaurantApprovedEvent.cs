using System;
using FoodSnap.Application.Events;

namespace FoodSnap.Application.Restaurants
{
    public class RestaurantApprovedEvent : Event
    {
        public RestaurantApprovedEvent(Guid restaurantId)
        {
            RestaurantId = restaurantId;
        }

        public Guid RestaurantId { get; }
    }
}
