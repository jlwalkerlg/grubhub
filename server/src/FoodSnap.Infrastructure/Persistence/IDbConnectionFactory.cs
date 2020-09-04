using System.Data;
using System.Threading.Tasks;

namespace FoodSnap.Infrastructure.Persistence
{
    public interface IDbConnectionFactory
    {
        Task<IDbConnection> OpenConnection();
    }
}
