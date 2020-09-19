using System;

namespace FoodSnap.Application.Restaurants.AcceptRestaurantRegistration
{
    public class AcceptRestaurantRegistrationCommand : IRequest
    {
        public Guid RestaurantId { get; set; }
    }
}
