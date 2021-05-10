using System.Collections.Generic;
using System.Threading.Tasks;
using Web;
using Web.Data.EF;
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
    public class EFUnitOfWorkProxy : IUnitOfWork
    {
        private readonly EFUnitOfWork proxy;
        private readonly OutboxSpy outbox;

        public EFUnitOfWorkProxy(EFUnitOfWork proxy, OutboxSpy outbox)
        {
            this.proxy = proxy;
            this.outbox = outbox;
        }

        public IRestaurantRepository Restaurants => proxy.Restaurants;
        public IMenuRepository Menus => proxy.Menus;
        public IUserRepository Users => proxy.Users;
        public ICuisineRepository Cuisines => proxy.Cuisines;
        public IBasketRepository Baskets => proxy.Baskets;
        public IOrderRepository Orders => proxy.Orders;
        public IBillingAccountRepository BillingAccounts => proxy.BillingAccounts;

        public Task Publish(Event @event)
        {
            outbox.Events.Add(@event);
            return Task.CompletedTask;
        }

        public Task Commit() => proxy.Commit();
    }
}
