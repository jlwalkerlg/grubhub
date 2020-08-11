using System.Threading.Tasks;
using FoodSnap.Application;
using FoodSnap.Application.Restaurants;
using FoodSnap.ApplicationTests.Restaurants;

namespace FoodSnap.ApplicationTests
{
    public class UnitOfWorkSpy : IUnitOfWork
    {
        public IRestaurantRepository RestaurantRepository => RestaurantRepositorySpy;
        public RestaurantRepositorySpy RestaurantRepositorySpy { get; }

        public IRestaurantManagerRepository RestaurantManagerRepository => RestaurantManagerRepositorySpy;
        public RestaurantManagerRepositorySpy RestaurantManagerRepositorySpy { get; }

        public IEventRepository EventRepository => EventRepositorySpy;
        public EventRepositorySpy EventRepositorySpy { get; }

        public bool Commited { get; private set; } = false;

        public UnitOfWorkSpy()
        {
            RestaurantRepositorySpy = new RestaurantRepositorySpy();
            RestaurantManagerRepositorySpy = new RestaurantManagerRepositorySpy();
            EventRepositorySpy = new EventRepositorySpy();
        }

        public Task Commit()
        {
            Commited = true;
            return Task.CompletedTask;
        }
    }
}
