using System.Data;
using System.Threading.Tasks;

namespace Web.Data
{
    public interface IDbConnectionFactory
    {
        Task<IDbConnection> OpenConnection();
    }
}
