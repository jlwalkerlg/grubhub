using Shouldly;
using System.Threading.Tasks;
using Web.Features.Users.GetAuthUser;
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

            var model = await response.GetData<GetAuthUserAction.UserModel>();

            model.Id.ShouldBe(user.Id);
            model.FirstName.ShouldBe(user.FirstName);
            model.LastName.ShouldBe(user.LastName);
            model.Email.ShouldBe(user.Email);
            model.RestaurantId.ShouldBe(restaurant.Id);
            model.RestaurantName.ShouldBe(restaurant.Name);
            model.Role.ShouldBe(user.Role);
        }

        [Fact]
        public async Task It_Fails_If_The_User_Is_Not_Found()
        {
            var response = await factory.GetAuthenticatedClient().Get("/auth/user");

            response.StatusCode.ShouldBe(404);
        }
    }
}
