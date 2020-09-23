using System;

namespace FoodSnap.Application.Restaurants.UpdateRestaurantDetails
{
    public class UpdateRestaurantDetailsCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
    }
}
