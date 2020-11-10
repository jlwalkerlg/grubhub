using System;

namespace FoodSnap.Application.Restaurants
{
    public class RestaurantDto
    {
        public Guid Id { get; set; }
        public Guid ManagerId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public string Status { get; set; }
    }
}
