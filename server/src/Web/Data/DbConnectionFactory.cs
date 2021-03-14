using Npgsql;
using System.Data;
using System.Threading.Tasks;

namespace Web.Data
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly DatabaseSettings settings;

        public DbConnectionFactory(DatabaseSettings settings)
        {
            this.settings = settings;
        }

        public async Task<IDbConnection> OpenConnection()
        {
            var connection = new NpgsqlConnection(settings.ConnectionString);
            await connection.OpenAsync();

            return connection;
        }
    }
}
