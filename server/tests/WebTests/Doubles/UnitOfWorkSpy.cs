using System.Threading.Tasks;
using Web;
using Web.Features.Cuisines;
using Web.Features.Events;
using Web.Features.Menus;
using Web.Features.Orders;
using Web.Features.Restaurants;
using Web.Features.Users;

namespace WebTests.Doubles
{
    public class UnitOfWorkSpy : IUnitOfWork
    {
        public IRestaurantRepository Restaurants => RestaurantRepositorySpy;
        public RestaurantRepositorySpy RestaurantRepositorySpy { get; } = new();

        public IMenuRepository Menus => MenuRepositorySpy;
        public MenuRepositorySpy MenuRepositorySpy { get; } = new();

        public IUserRepository Users => UserRepositorySpy;
        public UserRepositorySpy UserRepositorySpy { get; } = new();

        public IEventRepository Events => EventRepositorySpy;
        public EventRepositorySpy EventRepositorySpy { get; } = new();

        public bool Commited { get; private set; } = false;

        public ICuisineRepository Cuisines => CuisineRepositorySpy;
        public CuisineRepositorySpy CuisineRepositorySpy { get; } = new();

        public IOrderRepository Orders => OrderRepositorySpy;
        public OrderRepositorySpy OrderRepositorySpy { get; } = new();

        public Task Commit()
        {
            Commited = true;
            return Task.CompletedTask;
        }
    }
}
