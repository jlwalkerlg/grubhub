using System;

namespace FoodSnap.Web.Queries.Restaurants
{
    public class RestaurantDto
    {
        public Guid Id { get; set; }
        public Guid ManagerId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Town { get; set; }
        public string Postcode { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public string Status { get; set; }
    }
}
