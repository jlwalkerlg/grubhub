using Npgsql;
using System.Data;
using System.Threading.Tasks;
using Web;
using Web.Data;

namespace WebTests
{
    public class TestDbConnectionFactory : IDbConnectionFactory
    {
        private readonly Config config;

        public TestDbConnectionFactory(Config config)
        {
            this.config = config;
        }

        public async Task<IDbConnection> OpenConnection()
        {
            var connection = new NpgsqlConnection(config.DbConnectionString);

            await connection.OpenAsync();

            return connection;
        }
    }
}
