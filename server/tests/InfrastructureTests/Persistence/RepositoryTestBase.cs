using Infrastructure.Persistence.EF;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Respawn;
using Xunit;

namespace InfrastructureTests.Persistence
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
                .UseNpgsql(Config.TestDbConnectionString)
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
            using (var conn = new NpgsqlConnection(Config.TestDbConnectionString))
            {
                conn.Open();
                checkpoint.Reset(conn).Wait();
            }

            context = new AppDbContext(dbContextOptions);
            dbConnectionFactory = new TestDbConnectionFactory(Config.TestDbConnectionString);
        }
    }

    [CollectionDefinition(nameof(RepositoryTestCollection))]
    public class RepositoryTestCollection
    {
    }
}
