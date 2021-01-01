using System.Threading.Tasks;
using Xunit;

namespace WebTests.Actions
{
    [Collection(nameof(WebIntegrationTestFixture))]
    public class WebIntegrationTestBase : WebTestBase, IAsyncLifetime
    {
        protected readonly WebIntegrationTestFixture fixture;

        public WebIntegrationTestBase(WebIntegrationTestFixture fixture) : base(fixture)
        {
            this.fixture = fixture;
        }

        public async Task InitializeAsync()
        {
            await fixture.ResetDatabase();
        }

        public async Task DisposeAsync()
        {
            await Task.CompletedTask;
        }
    }
}
