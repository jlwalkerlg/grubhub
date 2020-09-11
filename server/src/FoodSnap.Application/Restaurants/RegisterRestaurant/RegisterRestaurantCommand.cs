namespace FoodSnap.Application.Restaurants.RegisterRestaurant
{
    public class RegisterRestaurantCommand : IRequest
    {
        public string ManagerName { get; set; }
        public string ManagerEmail { get; set; }
        public string ManagerPassword { get; set; }
        public string RestaurantName { get; set; }
        public string RestaurantPhoneNumber { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Town { get; set; }
        public string Postcode { get; set; }
    }
}
