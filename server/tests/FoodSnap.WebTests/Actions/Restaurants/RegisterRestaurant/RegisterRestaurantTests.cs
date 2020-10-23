using System.Threading.Tasks;
using FoodSnap.Application.Restaurants.RegisterRestaurant;
using Xunit;

namespace FoodSnap.WebTests.Actions.Restaurants.RegisterRestaurant
{
    public class RegisterRestaurantTests : WebActionTestBase
    {
        public RegisterRestaurantTests(WebActionTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Returns_Handler_Errors()
        {
            var response = await Post("/restaurants/register", new RegisterRestaurantCommand
            {
                ManagerName = "Jordan Walker",
                ManagerEmail = "walker.jlg@gmail.com",
                ManagerPassword = "password123",
                RestaurantName = "Chow Main",
                RestaurantPhoneNumber = "01234567890",
                Address = "12 Maine Road, Madchester, MN12 1NM",
            });

            Assert.Equal(FailMiddlewareStub.Message, await response.GetErrorMessage());
        }

        [Fact]
        public async Task It_Returns_Validation_Errors()
        {
            var command = new RegisterRestaurantCommand
            {
                ManagerName = "",
                ManagerEmail = "",
                ManagerPassword = "",
                RestaurantName = "",
                RestaurantPhoneNumber = "",
                Address = ""
            };
            var response = await Post("/restaurants/register", command);

            Assert.Equal(422, (int)response.StatusCode);

            var errors = (await response.GetErrors());
            Assert.True(errors.ContainsKey("managerName"));
            Assert.True(errors.ContainsKey("managerEmail"));
            Assert.True(errors.ContainsKey("managerPassword"));
            Assert.True(errors.ContainsKey("restaurantName"));
            Assert.True(errors.ContainsKey("restaurantPhoneNumber"));
            Assert.True(errors.ContainsKey("address"));
        }
    }
}
