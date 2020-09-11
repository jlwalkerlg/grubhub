using FoodSnap.Application.Restaurants.RegisterRestaurant;

namespace FoodSnap.ApplicationTests.Restaurants.RegisterRestaurant
{
    public class RegisterRestaurantCommandBuilder
    {
        private RegisterRestaurantCommand command = new RegisterRestaurantCommand
        {
            ManagerName = "Jordan Walker",
            ManagerEmail = "walker.jlg@gmail.com",
            ManagerPassword = "password123",
            RestaurantName = "Chow Main",
            RestaurantPhoneNumber = "01234 567890",
            AddressLine1 = "19 Main Street",
            AddressLine2 = null,
            Town = "Manchester",
            Postcode = "WS12 1WS",
        };

        public RegisterRestaurantCommandBuilder SetManagerName(string managerName)
        {
            command.ManagerName = managerName;
            return this;
        }

        public RegisterRestaurantCommandBuilder SetManagerEmail(string managerEmail)
        {
            command.ManagerEmail = managerEmail;
            return this;
        }

        public RegisterRestaurantCommandBuilder SetManagerPassword(string managerPassword)
        {
            command.ManagerPassword = managerPassword;
            return this;
        }

        public RegisterRestaurantCommandBuilder SetRestaurantName(string restaurantName)
        {
            command.RestaurantName = restaurantName;
            return this;
        }

        public RegisterRestaurantCommandBuilder SetRestaurantPhoneNumber(string restaurantPhoneNumber)
        {
            command.RestaurantPhoneNumber = restaurantPhoneNumber;
            return this;
        }

        public RegisterRestaurantCommandBuilder SetAddressLine1(string addressLine1)
        {
            command.AddressLine1 = addressLine1;
            return this;
        }

        public RegisterRestaurantCommandBuilder SetAddressLine2(string addressLine2)
        {
            command.AddressLine2 = addressLine2;
            return this;
        }

        public RegisterRestaurantCommandBuilder SetTown(string town)
        {
            command.Town = town;
            return this;
        }

        public RegisterRestaurantCommandBuilder SetPostcode(string postcode)
        {
            command.Postcode = postcode;
            return this;
        }

        public RegisterRestaurantCommand Build()
        {
            return command;
        }
    }
}
