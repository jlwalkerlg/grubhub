using FoodSnap.Infrastructure.Persistence.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace FoodSnap.InfrastructureTests.Persistence.EF
{
    public class EFContextFixture
    {
        private static string connectionString;

        static EFContextFixture()
        {
            var config = new ConfigurationBuilder()
               .AddJsonFile("config.json")
               .Build();

            connectionString = config["TestDbConnectionString"];
        }

        public EFContextFixture()
        {
            using (var context = CreateContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }
        }

        public AppDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseNpgsql(connectionString)
                .Options;

            return new AppDbContext(options);
        }
    }

    [CollectionDefinition("EF")]
    public class EFCollectionFixture : ICollectionFixture<EFContextFixture>
    {
    }
}
