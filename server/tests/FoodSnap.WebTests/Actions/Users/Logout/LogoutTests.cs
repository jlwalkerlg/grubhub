using System.Threading.Tasks;
using FoodSnap.WebTests.Functional;
using Xunit;
using Xunit.Abstractions;

namespace FoodSnap.WebTests.Actions.Restaurants.Logout
{
    public class LogoutTests : FunctionalTestBase
    {
        private readonly ITestOutputHelper output;

        public LogoutTests(WebAppFactory factory, Xunit.Abstractions.ITestOutputHelper output) : base(factory)
        {
            this.output = output;
        }

        [Fact]
        public async Task It_Succeeds()
        {
            var response = await PostJson("/auth/logout");

            output.WriteLine(await response.Content.ReadAsStringAsync());

            response.EnsureSuccessStatusCode();
        }
    }
}
