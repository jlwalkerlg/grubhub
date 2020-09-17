using System.Threading.Tasks;
using FoodSnap.WebTests.Functional;
using Xunit;

namespace FoodSnap.WebTests.Actions.Restaurants.Logout
{
    public class LogoutTests : FunctionalTestBase
    {
        public LogoutTests(WebAppFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task It_Succeeds()
        {
            var response = await PostJson("/auth/logout");

            response.EnsureSuccessStatusCode();
        }
    }
}
