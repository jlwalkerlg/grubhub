using System.Threading.Tasks;
using Infrastructure.Persistence.EF;
using Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Respawn;
using Xunit;

namespace WebTests.Actions
{
    [CollectionDefinition(nameof(WebIntegrationTestFixture))]
    public class WebIntegrationTestCollection : ICollectionFixture<WebIntegrationTestFixture>
    {
    }

    public class WebIntegrationTestFixture : WebTestFixture
    {
        private readonly WebIntegrationTestAppFactory factory;
        private readonly WebConfig config;
        private readonly Checkpoint checkpoint;

        public WebIntegrationTestFixture()
        {
            factory = new WebIntegrationTestAppFactory();

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

        public override WebTestAppFactory Factory => factory;

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

    public class WebIntegrationTestAppFactory : WebTestAppFactory
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
