using System.Threading.Tasks;
using Application;
using Application.Menus;
using Application.Restaurants;
using Application.Restaurants.RegisterRestaurant;
using Application.Users;
using Domain.Restaurants;
using Domain.Users;
using Infrastructure.Persistence.EF;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace WebTests.Actions.Restaurants.RegisterRestaurant
{
    public class RegisterRestaurantIntegrationTests : WebIntegrationTestBase
    {
        public RegisterRestaurantIntegrationTests(WebIntegrationTestFixture fixture) : base(fixture)
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

            await fixture.ExecuteService<AppDbContext>(async db =>
            {
                var eventDto = await db.Events.SingleAsync();
                Assert.Equal(typeof(RestaurantRegisteredEvent).ToString(), eventDto.EventType);

                var @event = eventDto.ToEvent<RestaurantRegisteredEvent>();
                Assert.Equal(user.Id, @event.ManagerId.Value);
                Assert.Equal(restaurant.Id, @event.RestaurantId.Value);
            });
        }
    }
}
