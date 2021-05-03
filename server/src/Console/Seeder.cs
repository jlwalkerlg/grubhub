using System.IO;
using System.Threading.Tasks;
using Npgsql;
using Web.Data;
using Web.Data.EF;

namespace Console
{
    public class Seeder
    {
        private readonly IDbConnectionFactory dbConnectionFactory;
        private readonly AppDbContext context;

        public Seeder(IDbConnectionFactory dbConnectionFactory, AppDbContext context)
        {
            this.dbConnectionFactory = dbConnectionFactory;
            this.context = context;
        }

        public async Task Seed()
        {
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            using var connection = await dbConnectionFactory.OpenConnection();

            var sql = await File.ReadAllTextAsync("grubhub.sql");
            var command = new NpgsqlCommand(sql, (NpgsqlConnection)connection);
            command.ExecuteNonQuery();
        }
    }
}
