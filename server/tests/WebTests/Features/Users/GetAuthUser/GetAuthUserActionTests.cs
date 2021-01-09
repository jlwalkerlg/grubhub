using System.Threading.Tasks;
using Xunit;

namespace WebTests.Features.Users.GetAuthUser
{
    public class GetAuthUserActionTests : HttpTestBase
    {
        public GetAuthUserActionTests(HttpTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Requires_Authentication()
        {
            var response = await fixture.GetClient().Get("/auth/user");

            response.StatusCode.ShouldBe(401);
        }
    }
}
