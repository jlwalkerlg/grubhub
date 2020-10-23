using Xunit;

namespace FoodSnap.WebTests.Actions
{
    public class WebActionTestBase : WebTestBase, IClassFixture<WebActionTestFixture>
    {
        public WebActionTestBase(WebActionTestFixture fixture) : base(fixture)
        {
        }
    }
}
