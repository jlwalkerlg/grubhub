using System.Data;
using System.Threading.Tasks;
using Web.Data;
using Npgsql;

namespace InfrastructureTests.Persistence
{
    public class TestDbConnectionFactory : IDbConnectionFactory
    {
        private readonly string connectionString;

        public TestDbConnectionFactory(string connectionString)
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
