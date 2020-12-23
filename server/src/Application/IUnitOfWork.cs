using System.Threading.Tasks;
using Application.Events;
using Application.Menus;
using Application.Restaurants;
using Application.Users;

namespace Application
{
    public interface IUnitOfWork
    {
        IRestaurantRepository Restaurants { get; }
        IMenuRepository Menus { get; }
        IRestaurantManagerRepository RestaurantManagers { get; }
        IUserRepository Users { get; }
        IEventRepository Events { get; }

        Task Commit();
    }
}
