using System.Threading.Tasks;
using FoodSnap.Application.Restaurants;
using FoodSnap.Application.Users;

namespace FoodSnap.Application
{
    public interface IUnitOfWork
    {
        IRestaurantRepository RestaurantRepository { get; }
        IRestaurantApplicationRepository RestaurantApplicationRepository { get; }
        IRestaurantManagerRepository RestaurantManagerRepository { get; }
        IEventRepository EventRepository { get; }

        Task Commit();
    }
}
