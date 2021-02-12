using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Respawn;
using System;
using Web;
using Web.Data.EF;
using Web.Features.Billing;
using Web.Services.Hashing;
using WebTests.Doubles;
using WebTests.TestData;
using Xunit;

namespace WebTests
{
    [Trait("Category", "IntegrationTest")]
    [Collection(nameof(IntegrationTestFixture))]
    public abstract class IntegrationTestBase
    {
        protected readonly IntegrationTestFixture fixture;

        public IntegrationTestBase(IntegrationTestFixture fixture)
        {
            this.fixture = fixture;

            fixture.ResetDatabase();
        }
    }

    [CollectionDefinition(nameof(IntegrationTestFixture))]
    public class IntegrationTestFixture : HttpTestFixture, ICollectionFixture<IntegrationTestFixture>
    {
        private readonly Checkpoint checkpoint;

        public IntegrationTestFixture() : base(new IntegrationTestWebApplicationFactory())
        {
            using (var scope = factory.Services.CreateScope())
            using (var db = scope.ServiceProvider.GetRequiredService<AppDbContext>())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
            }

            checkpoint = new Checkpoint()
            {
                DbAdapter = DbAdapter.Postgres,
                SchemasToInclude = new[]
                {
                    "public"
                },
            };
        }

        private IntegrationTestFixture(IntegrationTestWebApplicationFactory factory)
            : base(factory)
        {

        }

        public IntegrationTestFixture WithServices(Action<IServiceCollection> configureServices)
        {
            var factory = new IntegrationTestWebApplicationFactory(configureServices);
            return new IntegrationTestFixture(factory);
        }

        public void ResetDatabase()
        {
            using (var conn = new NpgsqlConnection(TestConfig.TestDbConnectionString))
            {
                conn.Open();
                checkpoint.Reset(conn).Wait();
            }
        }

        public void UseTestDbContext(Action<TestDbContext> action)
        {
            using (var scope = factory.Services.CreateScope())
            using (var db = scope.ServiceProvider.GetRequiredService<TestDbContext>())
            {
                action(db);
            };
        }

        public T UseTestDbContext<T>(Func<TestDbContext, T> action)
        {
            using (var scope = factory.Services.CreateScope())
            using (var db = scope.ServiceProvider.GetRequiredService<TestDbContext>())
            {
                return action(db);
            };
        }

        public void Insert(params object[] entities)
        {
            UseTestDbContext(db =>
            {
                db.AddRange(entities);
                db.SaveChanges();
            });
        }

        public string Hash(string unhashed)
        {
            using (var scope = factory.Services.CreateScope())
            {
                var hasher = scope.ServiceProvider.GetRequiredService<IHasher>();
                return hasher.Hash(unhashed);
            }
        }

        public bool VerifyHash(string unhashed, string hashed)
        {
            using (var scope = factory.Services.CreateScope())
            {
                var hasher = scope.ServiceProvider.GetRequiredService<IHasher>();
                return hasher.CheckMatch(unhashed, hashed);
            }
        }
    }

    public class IntegrationTestWebApplicationFactory : HttpTestWebApplicationFactory
    {
        private readonly Action<IServiceCollection> configureServices;

        public IntegrationTestWebApplicationFactory(
            Action<IServiceCollection> configureServices) : this()
        {
            this.configureServices = configureServices;
        }

        public IntegrationTestWebApplicationFactory() : base()
        {
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            var sp = services.BuildServiceProvider();

            var config = sp.GetRequiredService<Config>();
            config.DbConnectionString = TestConfig.TestDbConnectionString;

            services.AddDbContext<TestDbContext>(options =>
            {
                options.UseNpgsql(config.DbConnectionString, b =>
                {
                    b.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                });
            });

            if (configureServices != null)
            {
                configureServices(services);
            }
        }
    }
}
