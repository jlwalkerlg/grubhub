using System.Data;
using System.Threading.Tasks;
using Web.Data;

namespace WebTests.Doubles
{
    public class NoopDbConnectionFactory : IDbConnectionFactory
    {
        public Task<IDbConnection> OpenConnection()
        {
            return null;
        }
    }
}
