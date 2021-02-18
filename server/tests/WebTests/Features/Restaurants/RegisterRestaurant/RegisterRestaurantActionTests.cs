using Shouldly;
using System.Threading.Tasks;
using Web.Features.Restaurants.RegisterRestaurant;
using Xunit;

namespace WebTests.Features.Restaurants.RegisterRestaurant
{
    public class RegisterRestaurantActionTests : ActionTestBase
    {
        public RegisterRestaurantActionTests(ActionTestWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task It_Returns_Validation_Errors()
        {
            var command = new RegisterRestaurantCommand()
            {
                ManagerName = "",
                ManagerEmail = "",
                ManagerPassword = "",
                RestaurantName = "",
                RestaurantPhoneNumber = "",
                Address = ""
            };

            var response = await GetClient().Post(
                "/restaurants/register",
                command);

            response.StatusCode.ShouldBe(422);

            var errors = (response.GetErrors());

            errors.ShouldContainKey("managerName");
            errors.ShouldContainKey("managerEmail");
            errors.ShouldContainKey("managerPassword");
            errors.ShouldContainKey("restaurantName");
            errors.ShouldContainKey("restaurantPhoneNumber");
            errors.ShouldContainKey("address");
        }

        [Fact]
        public async Task It_Returns_Handler_Errors()
        {
            var command = new RegisterRestaurantCommand()
            {
                ManagerName = "Jordan Walker",
                ManagerEmail = "walker.jlg@gmail.com",
                ManagerPassword = "password123",
                RestaurantName = "Chow Main",
                RestaurantPhoneNumber = "01234567890",
                Address = "12 Maine Road, Madchester, MN12 1NM",
            };

            var response = await GetClient().Post(
                "/restaurants/register",
                command);

            response.StatusCode.ShouldBe(400);
        }
    }
}
