using System.Threading.Tasks;
using Web.Features.Baskets;
using Web.Features.Billing;
using Web.Features.Cuisines;
using Web.Features.Events;
using Web.Features.Menus;
using Web.Features.Orders;
using Web.Features.Restaurants;
using Web.Features.Users;

namespace Web
{
    public interface IUnitOfWork
    {
        IRestaurantRepository Restaurants { get; }
        IMenuRepository Menus { get; }
        IUserRepository Users { get; }
        IEventRepository Events { get; }
        ICuisineRepository Cuisines { get; }
        IBasketRepository Baskets { get; }
        IOrderRepository Orders { get; }
        IBillingAccountRepository BillingAccounts { get; }

        Task Commit();
    }
}
