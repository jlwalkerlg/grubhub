using System.Threading.Tasks;
using FoodSnap.Infrastructure.Persistence.EF;
using FoodSnap.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Respawn;
using Xunit;

namespace FoodSnap.WebTests.Integration
{
    [CollectionDefinition(nameof(WebAppIntegrationTestFixture))]
    public class WebAppIntegrationTestCollection : ICollectionFixture<WebAppIntegrationTestFixture>
    {
    }

    public class WebAppIntegrationTestFixture : WebAppTestFixture
    {
        private readonly IntegrationTestWebAppFactory factory;
        private readonly WebConfig config;
        private readonly Checkpoint checkpoint;

        public WebAppIntegrationTestFixture()
        {
            factory = new IntegrationTestWebAppFactory();

            config = factory.Services.GetRequiredService<WebConfig>();

            using (var db = factory.Services.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
            }

            checkpoint = new Checkpoint
            {
                DbAdapter = DbAdapter.Postgres,
                SchemasToInclude = new[]
                {
                    "public"
                },
            };
        }

        public override TestWebAppFactory Factory => factory;

        public async Task ResetDatabase()
        {
            using (var conn = new NpgsqlConnection(config.DbConnectionString))
            {
                await conn.OpenAsync();
                await checkpoint.Reset(conn);
            }
        }

        public async Task InsertDb(params object[] entities)
        {
            await ExecuteService<AppDbContext>(async db =>
            {
                await db.AddRangeAsync(entities);
                await db.SaveChangesAsync();
            });
        }
    }

    public class IntegrationTestWebAppFactory : TestWebAppFactory
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.ConfigureServices((ctx, services) =>
            {
                var sp = services.BuildServiceProvider();

                var config = sp.GetRequiredService<WebConfig>();
                config.DbConnectionString = ctx.Configuration["TestDbConnectionString"];
            });
        }
    }
}
