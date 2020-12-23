using System.Threading.Tasks;
using Xunit;

namespace WebTests.Integration
{
    [Collection(nameof(WebAppIntegrationTestFixture))]
    public class WebIntegrationTestBase : WebTestBase, IAsyncLifetime
    {
        protected readonly WebAppIntegrationTestFixture fixture;

        public WebIntegrationTestBase(WebAppIntegrationTestFixture fixture) : base(fixture)
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
