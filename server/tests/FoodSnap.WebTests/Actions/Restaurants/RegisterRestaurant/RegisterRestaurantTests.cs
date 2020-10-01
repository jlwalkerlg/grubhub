using System.Threading.Tasks;
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
            var response = await PostJson("/restaurants/register", new
            {
                managerName = "Jordan Walker",
                managerEmail = "test@email.com",
                managerPassword = "password123",
                restaurantName = "Chow Main",
                restaurantPhoneNumber = "01234567890",
                address = "1 Maine Road, Manchester, UK"
            });

            response.EnsureSuccessStatusCode();
        }
    }
}
