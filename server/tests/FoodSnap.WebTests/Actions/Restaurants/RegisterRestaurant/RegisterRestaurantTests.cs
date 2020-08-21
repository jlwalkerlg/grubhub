using System.Threading.Tasks;
using FoodSnap.WebTests.Functional;
using Xunit;
using Xunit.Abstractions;

namespace FoodSnap.WebTests.Actions.Restaurants.RegisterRestaurant
{
    public class RegisterRestaurantTests : FunctionalTestBase
    {
        private readonly ITestOutputHelper output;

        public RegisterRestaurantTests(WebAppFactory factory, ITestOutputHelper output) : base(factory)
        {
            this.output = output;
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
                addressLine1 = "1 Mean Street",
                addressLine2 = "",
                town = "Manchester",
                postcode = "MN12 1NM",
            });

            output.WriteLine(response.Content.ReadAsStringAsync().Result);

            response.EnsureSuccessStatusCode();
        }
    }
}
