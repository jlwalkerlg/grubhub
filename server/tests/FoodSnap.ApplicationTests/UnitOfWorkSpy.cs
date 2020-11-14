using System.Threading.Tasks;
using FoodSnap.Application;
using FoodSnap.Application.Events;
using FoodSnap.Application.Menus;
using FoodSnap.Application.Restaurants;
using FoodSnap.Application.Users;
using FoodSnap.ApplicationTests.Events;
using FoodSnap.ApplicationTests.Menus;
using FoodSnap.ApplicationTests.Restaurants;
using FoodSnap.ApplicationTests.Users;

namespace FoodSnap.ApplicationTests
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
