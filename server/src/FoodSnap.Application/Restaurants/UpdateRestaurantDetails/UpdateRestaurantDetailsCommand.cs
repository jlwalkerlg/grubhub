using System;
using FoodSnap.Application.Services.Authentication;

namespace FoodSnap.Application.Restaurants.UpdateRestaurantDetails
{
    [Authenticate]
    public class UpdateRestaurantDetailsCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
    }
}
