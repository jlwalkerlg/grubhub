using Npgsql;
using System.Data;
using System.Threading.Tasks;

namespace Web.Data
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly string connectionString;

        public DbConnectionFactory(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<IDbConnection> OpenConnection()
        {
            var connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync();

            return connection;
        }
    }
}
