using Xunit;

namespace WebTests.Actions
{
    public class WebActionTestBase : WebTestBase, IClassFixture<WebActionTestFixture>
    {
        public WebActionTestBase(WebActionTestFixture fixture) : base(fixture)
        {
        }
    }
}
