using System.Threading.Tasks;
using Xunit;

namespace FoodSnap.WebTests.Actions.Users.GetAuthUser
{
    public class GetAuthUserTests : StubbedWebTestBase
    {
        public GetAuthUserTests(StubbedWebAppTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Requires_Authentication()
        {
            var response = await Get("/auth/user");

            Assert.Equal(401, (int)response.StatusCode);
        }
    }
}
