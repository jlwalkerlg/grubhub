using FoodSnap.Application.Restaurants.RegisterRestaurant;

namespace FoodSnap.ApplicationTests.Restaurants.RegisterRestaurant
{
    public class RegisterRestaurantCommandBuilder
    {
        private string managerName = "Jordan Walker";
        private string managerEmail = "walker.jlg@gmail.com";
        private string managerPassword = "password123";
        private string restaurantName = "Chow Main";
        private string restaurantPhoneNumber = "01234 567890";
        private string addressLine1 = "19 Main Street";
        private string addressLine2 = null;
        private string town = "Manchester";
        private string postcode = "WS12 1WS";

        public RegisterRestaurantCommandBuilder SetManagerName(string managerName)
        {
            this.managerName = managerName;
            return this;
        }

        public RegisterRestaurantCommandBuilder SetManagerEmail(string managerEmail)
        {
            this.managerEmail = managerEmail;
            return this;
        }

        public RegisterRestaurantCommandBuilder SetManagerPassword(string managerPassword)
        {
            this.managerPassword = managerPassword;
            return this;
        }

        public RegisterRestaurantCommandBuilder SetRestaurantName(string restaurantName)
        {
            this.restaurantName = restaurantName;
            return this;
        }

        public RegisterRestaurantCommandBuilder SetRestaurantPhoneNumber(string restaurantPhoneNumber)
        {
            this.restaurantPhoneNumber = restaurantPhoneNumber;
            return this;
        }

        public RegisterRestaurantCommandBuilder SetAddressLine1(string addressLine1)
        {
            this.addressLine1 = addressLine1;
            return this;
        }

        public RegisterRestaurantCommandBuilder SetAddressLine2(string addressLine2)
        {
            this.addressLine2 = addressLine2;
            return this;
        }

        public RegisterRestaurantCommandBuilder SetTown(string town)
        {
            this.town = town;
            return this;
        }

        public RegisterRestaurantCommandBuilder SetPostcode(string postcode)
        {
            this.postcode = postcode;
            return this;
        }

        public RegisterRestaurantCommand Build()
        {
            return new RegisterRestaurantCommand(
                managerName,
                managerEmail,
                managerPassword,
                restaurantName,
                restaurantPhoneNumber,
                addressLine1,
                addressLine2,
                town,
                postcode);
        }
    }
}
