using System.Threading.Tasks;
using Application;
using Application.Events;
using Application.Menus;
using Application.Restaurants;
using Application.Users;
using ApplicationTests.Events;
using ApplicationTests.Menus;
using ApplicationTests.Restaurants;
using ApplicationTests.Users;

namespace ApplicationTests
{
    public class UnitOfWorkSpy : IUnitOfWork
    {
        public IRestaurantRepository Restaurants => RestaurantRepositorySpy;
        public RestaurantRepositorySpy RestaurantRepositorySpy { get; } = new();

        public IMenuRepository Menus => MenuRepositorySpy;
        public MenuRepositorySpy MenuRepositorySpy { get; } = new();

        public IRestaurantManagerRepository RestaurantManagers => RestaurantManagerRepositorySpy;
        public RestaurantManagerRepositorySpy RestaurantManagerRepositorySpy { get; } = new();

        public IUserRepository Users => UserRepositorySpy;
        public UserRepositorySpy UserRepositorySpy { get; } = new();

        public IEventRepository Events => EventRepositorySpy;
        public EventRepositorySpy EventRepositorySpy { get; } = new();

        public bool Commited { get; private set; } = false;

        public Task Commit()
        {
            Commited = true;
            return Task.CompletedTask;
        }
    }
}
