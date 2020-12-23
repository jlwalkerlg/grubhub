using System.Threading.Tasks;
using Domain.Users;

namespace Application.Users
{
    public interface IRestaurantManagerRepository
    {
        Task Add(RestaurantManager manager);
    }
}
