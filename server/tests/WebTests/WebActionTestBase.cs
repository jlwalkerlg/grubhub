using Xunit;

namespace WebTests
{
    public class WebActionTestBase : WebTestBase, IClassFixture<WebActionTestFixture>
    {
        public WebActionTestBase(WebActionTestFixture fixture) : base(fixture)
        {
        }
    }
}
