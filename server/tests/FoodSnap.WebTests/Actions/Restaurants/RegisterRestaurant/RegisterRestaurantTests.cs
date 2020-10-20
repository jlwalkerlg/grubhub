using System.Threading.Tasks;
using FoodSnap.Application;
using FoodSnap.Application.Restaurants.RegisterRestaurant;
using FoodSnap.Domain.Restaurants;
using FoodSnap.Domain.Users;
using FoodSnap.Web.Actions.Menus;
using FoodSnap.Web.Actions.Restaurants;
using FoodSnap.Web.Actions.Users;
using FoodSnap.WebTests.Doubles;
using Xunit;

namespace FoodSnap.WebTests.Actions.Restaurants.RegisterRestaurant
{
    public class RegisterRestaurantTests : WebTestBase
    {
        public RegisterRestaurantTests(TestWebApplicationFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Registers_A_Restaurant_And_A_Manager()
        {
            var response = await Post("/restaurants/register", new RegisterRestaurantCommand
            {
                ManagerName = "Jordan Walker",
                ManagerEmail = "walker.jlg@gmail.com",
                ManagerPassword = "password123",
                RestaurantName = "Chow Main",
                RestaurantPhoneNumber = "01234567890",
                Address = "1 Maine Road, Manchester, UK"
            });

            Assert.Equal(201, (int)response.StatusCode);

            await fixture.ExecuteService<IUnitOfWork>(async uow =>
            {
                var user = await uow.Users.GetByEmail("walker.jlg@gmail.com");
                await Login(user);
            });

            var user = await Get<UserDto>("/auth/user");
            Assert.Equal("walker.jlg@gmail.com", user.Email);
            Assert.Equal(UserRole.RestaurantManager.ToString(), user.Role);

            var restaurant = await Get<RestaurantDto>($"/restaurants/{user.RestaurantId}");
            Assert.Equal(user.Id, restaurant.ManagerId);
            Assert.Equal(RestaurantStatus.PendingApproval.ToString(), restaurant.Status);

            var menu = await Get<MenuDto>($"/restaurants/{restaurant.Id}/menu");
            Assert.Empty(menu.Categories);
        }

        [Fact]
        public async Task It_Returns_An_Error_On_Failure()
        {
            var invalidAddress = GeocoderStub.InvalidAddress;

            var response = await Post("/restaurants/register", new RegisterRestaurantCommand
            {
                ManagerName = "Jordan Walker",
                ManagerEmail = "walker.jlg@gmail.com",
                ManagerPassword = "password123",
                RestaurantName = "Chow Main",
                RestaurantPhoneNumber = "01234567890",
                Address = invalidAddress,
            });

            Assert.Equal(400, (int)response.StatusCode);
            Assert.NotNull(await response.GetErrorMessage());
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
