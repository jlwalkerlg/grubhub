using System.Threading.Tasks;
using FoodSnap.Application.Restaurants.RegisterRestaurant;
using FoodSnap.WebTests.Functional;
using Xunit;

namespace FoodSnap.WebTests.Actions.Restaurants.RegisterRestaurant
{
    public class RegisterRestaurantTests : FunctionalTestBase
    {
        public RegisterRestaurantTests(WebAppFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task It_Succeeds()
        {
            var response = await PostJson("/restaurants/register", new RegisterRestaurantCommand
            {
                ManagerName = "Jordan Walker",
                ManagerEmail = "test@email.com",
                ManagerPassword = "password123",
                RestaurantName = "Chow Main",
                RestaurantPhoneNumber = "01234567890",
                Address = "1 Maine Road, Manchester, UK"
            });

            response.EnsureSuccessStatusCode();
        }
    }
}
