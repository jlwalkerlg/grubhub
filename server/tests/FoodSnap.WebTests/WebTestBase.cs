using System.Threading.Tasks;
using Xunit;

namespace FoodSnap.WebTests
{
    [Collection(nameof(TestWebApplicationFixture))]
    public class WebTestBase : IAsyncLifetime
    {
        protected readonly TestWebApplicationFixture fixture;

        public WebTestBase(TestWebApplicationFixture fixture)
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
