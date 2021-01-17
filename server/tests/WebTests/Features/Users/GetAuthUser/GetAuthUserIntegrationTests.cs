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
            var user = new User();

            var restaurant = new Restaurant()
            {
                ManagerId = user.Id,
            };

            fixture.Insert(user, restaurant);

            var response = await fixture.GetAuthenticatedClient(user.Id).Get("/auth/user");

            response.StatusCode.ShouldBe(200);

            var userDto = await response.GetData<UserDto>();

            userDto.Id.ShouldBe(user.Id);
            userDto.Name.ShouldBe(user.Name);
            userDto.Email.ShouldBe(user.Email);
            userDto.RestaurantId.ShouldBe(restaurant.Id);
            userDto.RestaurantName.ShouldBe(restaurant.Name);
            userDto.Role.ShouldBe(user.Role);
        }

        [Fact]
        public async Task It_Fails_If_The_User_Not_Authenticated()
        {
            var response = await fixture.GetClient().Get("/auth/user");

            response.StatusCode.ShouldBe(401);
        }

        [Fact]
        public async Task It_Fails_If_The_User_Is_Not_Found()
        {
            var response = await fixture.GetAuthenticatedClient().Get("/auth/user");

            response.StatusCode.ShouldBe(404);
        }
    }
}
