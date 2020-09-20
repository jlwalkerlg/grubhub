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
        public RestaurantRepositorySpy RestaurantRepositorySpy { get; }

        public IMenuRepository Menus => MenuRepositorySpy;
        public MenuRepositorySpy MenuRepositorySpy { get; }

        public IRestaurantManagerRepository RestaurantManagers => RestaurantManagerRepositorySpy;
        public RestaurantManagerRepositorySpy RestaurantManagerRepositorySpy { get; }

        public IUserRepository Users => UserRepositorySpy;
        public UserRepositorySpy UserRepositorySpy { get; }

        public IEventRepository Events => EventRepositorySpy;
        public EventRepositorySpy EventRepositorySpy { get; }

        public bool Commited { get; private set; } = false;

        public UnitOfWorkSpy()
        {
            RestaurantRepositorySpy = new RestaurantRepositorySpy();
            MenuRepositorySpy = new MenuRepositorySpy();
            RestaurantManagerRepositorySpy = new RestaurantManagerRepositorySpy();
            UserRepositorySpy = new UserRepositorySpy();
            EventRepositorySpy = new EventRepositorySpy();
        }

        public Task Commit()
        {
            Commited = true;
            return Task.CompletedTask;
        }
    }
}
