using System.Data;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    public interface IDbConnectionFactory
    {
        Task<IDbConnection> OpenConnection();
    }
}
