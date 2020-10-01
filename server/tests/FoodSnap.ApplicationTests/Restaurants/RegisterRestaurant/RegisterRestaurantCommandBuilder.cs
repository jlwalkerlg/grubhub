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
            Address = "1 Maine Road, Manchester, UK"
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

        public RegisterRestaurantCommandBuilder SetAddress(string address)
        {
            command.Address = address;
            return this;
        }

        public RegisterRestaurantCommand Build()
        {
            return command;
        }
    }
}
