using Npgsql;
using System.Data;
using System.Threading.Tasks;

namespace Web.Data
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly Config config;

        public DbConnectionFactory(Config config)
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
