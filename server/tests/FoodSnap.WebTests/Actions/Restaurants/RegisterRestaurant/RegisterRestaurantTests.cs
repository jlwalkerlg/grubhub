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
            var response = await fixture.Post("/restaurants/register", new RegisterRestaurantCommand
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
                fixture.Login(user);
            });

            var user = await fixture.Get<UserDto>("/auth/user");
            Assert.Equal("Jordan Walker", user.Name);
            Assert.Equal("walker.jlg@gmail.com", user.Email);
            Assert.NotEqual("password123", user.Password);
            Assert.Equal(UserRole.RestaurantManager.ToString(), user.Role);

            var restaurant = await fixture.Get<RestaurantDto>($"/restaurants/{user.RestaurantId}");
            Assert.Equal(user.Id, restaurant.ManagerId);
            Assert.Equal("Chow Main", restaurant.Name);
            Assert.Equal("01234567890", restaurant.PhoneNumber);
            Assert.Equal(RestaurantStatus.PendingApproval.ToString(), restaurant.Status);
            Assert.Equal(GeocoderStub.Address, restaurant.Address);
            Assert.Equal(GeocoderStub.Latitude, restaurant.Latitude);
            Assert.Equal(GeocoderStub.Longitude, restaurant.Longitude);

            var menu = await fixture.Get<MenuDto>($"/restaurants/{restaurant.Id}/menu");
            Assert.Empty(menu.Categories);
        }

        private int IUnitOfWork(System.Func<object, Task> p)
        {
            throw new System.NotImplementedException();
        }

        [Fact]
        public async Task It_Returns_An_Error_On_Failure()
        {
            var invalidAddress = GeocoderStub.InvalidAddress;

            var response = await fixture.Post("/restaurants/register", new RegisterRestaurantCommand
            {
                ManagerName = "Jordan Walker",
                ManagerEmail = "walker.jlg@gmail.com",
                ManagerPassword = "password123",
                RestaurantName = "Chow Main",
                RestaurantPhoneNumber = "01234567890",
                Address = invalidAddress,
            });

            Assert.Equal(400, (int)response.StatusCode);

            var envelope = await response.ToErrorEnvelope();
            Assert.NotEmpty(envelope.Message);
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
            var response = await fixture.Post("/restaurants/register", command);

            Assert.Equal(422, (int)response.StatusCode);

            var errors = (await response.ToErrorEnvelope()).Errors;
            Assert.True(errors.ContainsKey("managerName"));
            Assert.True(errors.ContainsKey("managerEmail"));
            Assert.True(errors.ContainsKey("managerPassword"));
            Assert.True(errors.ContainsKey("restaurantName"));
            Assert.True(errors.ContainsKey("restaurantPhoneNumber"));
            Assert.True(errors.ContainsKey("address"));
        }
    }
}
