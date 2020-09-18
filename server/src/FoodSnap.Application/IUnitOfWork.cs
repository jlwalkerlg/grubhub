using System.Threading.Tasks;
using FoodSnap.Application.Events;
using FoodSnap.Application.Restaurants;
using FoodSnap.Application.Users;

namespace FoodSnap.Application
{
    public interface IUnitOfWork
    {
        IRestaurantRepository Restaurants { get; }
        IRestaurantManagerRepository RestaurantManagers { get; }
        IUserRepository Users { get; }
        IEventRepository Events { get; }

        Task Commit();
    }
}
