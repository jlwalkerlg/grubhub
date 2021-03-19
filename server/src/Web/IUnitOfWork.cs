using System;
using System.Data;
using System.Threading.Tasks;
using Web.Features.Baskets;
using Web.Features.Billing;
using Web.Features.Cuisines;
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
        ICuisineRepository Cuisines { get; }
        IBasketRepository Baskets { get; }
        IOrderRepository Orders { get; }
        IBillingAccountRepository BillingAccounts { get; }

        void Subscribe(Func<IDbTransaction, Task> subscriber);
        Task Commit();
    }
}
