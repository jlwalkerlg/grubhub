using Npgsql;
using System.Data;
using System.Threading.Tasks;
using Web.Data;

namespace WebTests.Data
{
    public class TestDbConnectionFactory : IDbConnectionFactory
    {
        public async Task<IDbConnection> OpenConnection()
        {
            var connection = new NpgsqlConnection(
                TestConfig.TestDbConnectionString);

            await connection.OpenAsync();

            return connection;
        }
    }
}
