using System.Threading.Tasks;
using Web.Domain.Users;

namespace Web.Features.Users
{
    public interface IRestaurantManagerRepository
    {
        Task Add(RestaurantManager manager);
    }
}
