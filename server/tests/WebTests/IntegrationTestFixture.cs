using Microsoft.Extensions.DependencyInjection;
using Respawn;
using Web.Data.EF;
using Xunit;

namespace WebTests
{
    [CollectionDefinition("IntegrationTest")]
    public class IntegrationTestFixture : ICollectionFixture<IntegrationTestFixture>
    {
        public IntegrationTestFixture()
        {
            Factory = new IntegrationTestWebApplicationFactory();

            using (var scope = Factory.Services.CreateScope())
            using (var db = scope.ServiceProvider.GetRequiredService<AppDbContext>())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
            }

            Checkpoint = new Checkpoint()
            {
                DbAdapter = DbAdapter.Postgres,
                SchemasToInclude = new[]
                {
                    "public"
                },
            };
        }

        public IntegrationTestWebApplicationFactory Factory { get; }
        public Checkpoint Checkpoint { get; }
    }
}
