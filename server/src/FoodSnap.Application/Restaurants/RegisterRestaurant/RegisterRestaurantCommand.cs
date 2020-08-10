namespace FoodSnap.Application.Restaurants.RegisterRestaurant
{
    public class RegisterRestaurantCommand : IRequest
    {
        public string ManagerName { get; }
        public string ManagerEmail { get; }
        public string ManagerPassword { get; }
        public string RestaurantName { get; }
        public string RestaurantPhoneNumber { get; }
        public string AddressLine1 { get; }
        public string AddressLine2 { get; }
        public string Town { get; }
        public string Postcode { get; }

        public RegisterRestaurantCommand(
            string managerName,
            string managerEmail,
            string managerPassword,
            string restaurantName,
            string restaurantPhoneNumber,
            string addressLine1,
            string addressLine2,
            string town,
            string postcode)
        {
            ManagerName = managerName;
            ManagerEmail = managerEmail;
            ManagerPassword = managerPassword;
            RestaurantName = restaurantName;
            RestaurantPhoneNumber = restaurantPhoneNumber;
            AddressLine1 = addressLine1;
            AddressLine2 = addressLine2;
            Town = town;
            Postcode = postcode;
        }
    }
}
