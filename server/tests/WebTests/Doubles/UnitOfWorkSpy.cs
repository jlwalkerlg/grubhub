using System.Threading.Tasks;
using Web;
using Web.Features.Baskets;
using Web.Features.Billing;
using Web.Features.Cuisines;
using Web.Features.Menus;
using Web.Features.Orders;
using Web.Features.Restaurants;
using Web.Features.Users;
using Web.Services.Events;

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

        public ICuisineRepository Cuisines => CuisineRepositorySpy;
        public CuisineRepositorySpy CuisineRepositorySpy { get; } = new();

        public IBasketRepository Baskets => BasketRepositorySpy;
        public BasketRepositorySpy BasketRepositorySpy { get; } = new();

        public IOrderRepository Orders => OrderRepositorySpy;
        public OrderRepositorySpy OrderRepositorySpy { get; } = new();

        public IBillingAccountRepository BillingAccounts => BillingAccountsRepositorySpy;
        public BillingAccountRepositorySpy BillingAccountsRepositorySpy { get; } = new();

        public IEventStore Events => EventStoreSpy;
        public EventStoreSpy EventStoreSpy { get; } = new();

        public bool Commited { get; private set; } = false;

        public Task Commit()
        {
            Commited = true;
            return Task.CompletedTask;
        }
    }
}
