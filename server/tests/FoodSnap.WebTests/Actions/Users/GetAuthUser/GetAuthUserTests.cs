using System.Threading.Tasks;
using Xunit;

namespace FoodSnap.WebTests.Actions.Users.GetAuthUser
{
    public class GetAuthUserTests : WebActionTestBase
    {
        public GetAuthUserTests(WebActionTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Requires_Authentication()
        {
            var response = await Get("/auth/user");

            Assert.Equal(401, (int)response.StatusCode);
        }

        [Fact]
        public async Task It_Returns_Handler_Errors()
        {
            await Login();

            var response = await Get("/auth/user");

            Assert.Equal(FailMiddlewareStub.Message, await response.GetErrorMessage());
        }
    }
}