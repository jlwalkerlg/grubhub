using System.Threading.Tasks;
using Web.Data.EF;
using Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Respawn;
using Xunit;

namespace WebTests
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

    [CollectionDefinition(nameof(WebIntegrationTestFixture))]
    public class WebIntegrationTestCollection : ICollectionFixture<WebIntegrationTestFixture>
    {
    }

    public class WebIntegrationTestFixture : WebTestFixture
    {
        private readonly WebIntegrationTestAppFactory factory;
        private readonly Config config;
        private readonly Checkpoint checkpoint;

        public WebIntegrationTestFixture()
        {
            factory = new WebIntegrationTestAppFactory();

            config = factory.Services.GetRequiredService<Config>();

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

                var config = sp.GetRequiredService<Config>();
                config.DbConnectionString = TestConfig.WebTestDbConnectionString;
            });
        }
    }
}
