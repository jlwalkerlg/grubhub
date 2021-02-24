using System.Threading.Tasks;
using Xunit;

namespace WebTests.Features.Users.GetAuthUser
{
    public class GetAuthUserActionTests : ActionTestBase
    {
        public GetAuthUserActionTests(ActionTestWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task It_Fails_If_The_User_Not_Authenticated()
        {
            var response = await factory.GetClient().Get("/auth/user");

            response.StatusCode.ShouldBe(401);
        }
    }
}
