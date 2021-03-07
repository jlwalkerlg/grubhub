using Shouldly;
using System.Threading.Tasks;
using Web.Features.Users;
using WebTests.TestData;
using Xunit;

namespace WebTests.Features.Users.GetAuthUser
{
    public class GetAuthUserIntegrationTests : IntegrationTestBase
    {
        public GetAuthUserIntegrationTests(IntegrationTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Returns_The_Authenticated_User()
        {
            var user = new User()
            {
                Role = Web.Domain.Users.UserRole.RestaurantManager,
            };

            var restaurant = new Restaurant()
            {
                Manager = user,
            };

            Insert(user, restaurant);

            var response = await factory.GetAuthenticatedClient(user.Id).Get("/auth/user");

            response.StatusCode.ShouldBe(200);

            var userDto = await response.GetData<UserDto>();

            userDto.Id.ShouldBe(user.Id);
            userDto.FirstName.ShouldBe(user.FirstName);
            userDto.LastName.ShouldBe(user.LastName);
            userDto.Email.ShouldBe(user.Email);
            userDto.RestaurantId.ShouldBe(restaurant.Id);
            userDto.RestaurantName.ShouldBe(restaurant.Name);
            userDto.Role.ShouldBe(user.Role);
        }

        [Fact]
        public async Task It_Fails_If_The_User_Is_Not_Found()
        {
            var response = await factory.GetAuthenticatedClient().Get("/auth/user");

            response.StatusCode.ShouldBe(404);
        }
    }
}
