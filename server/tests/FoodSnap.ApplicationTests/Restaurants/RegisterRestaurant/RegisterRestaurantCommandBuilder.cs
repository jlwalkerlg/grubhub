using FoodSnap.Application.Restaurants.RegisterRestaurant;

namespace FoodSnap.ApplicationTests.Restaurants.RegisterRestaurant
{
    public class RegisterRestaurantCommandBuilder
    {
        private RegisterRestaurantCommand command = new()
        {
            ManagerName = "Jordan Walker",
            ManagerEmail = "walker.jlg@gmail.com",
            ManagerPassword = "password123",
            RestaurantName = "Chow Main",
            RestaurantPhoneNumber = "01234 567890",
            Address = "1 Maine Road, Manchester, UK"
        };

        public RegisterRestaurantCommandBuilder SetManagerName(string managerName)
        {
            command = command with { ManagerName = managerName };
            return this;
        }

        public RegisterRestaurantCommandBuilder SetManagerEmail(string managerEmail)
        {
            command = command with { ManagerEmail = managerEmail };
            return this;
        }

        public RegisterRestaurantCommandBuilder SetManagerPassword(string managerPassword)
        {
            command = command with { ManagerPassword = managerPassword };
            return this;
        }

        public RegisterRestaurantCommandBuilder SetRestaurantName(string restaurantName)
        {
            command = command with { RestaurantName = restaurantName };
            return this;
        }

        public RegisterRestaurantCommandBuilder SetRestaurantPhoneNumber(string restaurantPhoneNumber)
        {
            command = command with { RestaurantPhoneNumber = restaurantPhoneNumber };
            return this;
        }

        public RegisterRestaurantCommandBuilder SetAddress(string address)
        {
            command = command with { Address = address };
            return this;
        }

        public RegisterRestaurantCommand Build()
        {
            return command;
        }
    }
}
