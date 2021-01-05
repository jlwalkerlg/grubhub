using Web.Data.EF;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Respawn;
using Xunit;

namespace WebTests.Data
{
    [Collection(nameof(RepositoryTestCollection))]
    public abstract class RepositoryTestBase
    {
        private static readonly DbContextOptions<AppDbContext> dbContextOptions;
        private static readonly Checkpoint checkpoint;
        protected readonly AppDbContext context;
        protected readonly TestDbConnectionFactory dbConnectionFactory;

        static RepositoryTestBase()
        {
            dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseNpgsql(TestConfig.InfrastructureTestDbConnectionString)
                .Options;

            using (var context = new AppDbContext(dbContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
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

        public RepositoryTestBase()
        {
            using (var conn = new NpgsqlConnection(TestConfig.InfrastructureTestDbConnectionString))
            {
                conn.Open();
                checkpoint.Reset(conn).Wait();
            }

            context = new AppDbContext(dbContextOptions);
            dbConnectionFactory = new TestDbConnectionFactory(TestConfig.InfrastructureTestDbConnectionString);
        }
    }

    [CollectionDefinition(nameof(RepositoryTestCollection))]
    public class RepositoryTestCollection
    {
    }
}
