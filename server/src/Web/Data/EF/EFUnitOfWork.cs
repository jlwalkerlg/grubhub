using System.Linq;
using System.Threading.Tasks;
using DotNetCore.CAP;
using Web.Data.EF.Repositories;
using Web.Features.Baskets;
using Web.Features.Billing;
using Web.Features.Cuisines;
using Web.Features.Menus;
using Web.Features.Orders;
using Web.Features.Restaurants;
using Web.Features.Users;
using Web.Services.Events;

namespace Web.Data.EF
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext context;
        private readonly EFOutbox outbox;

        public EFUnitOfWork(AppDbContext context, ICapPublisher publisher)
        {
            this.context = context;
            outbox = new EFOutbox(publisher, context);
        }

        public IRestaurantRepository Restaurants => new EFRestaurantRepository(context);
        public IMenuRepository Menus => new EFMenuRepository(context);
        public IUserRepository Users => new EFUserRepository(context);
        public ICuisineRepository Cuisines => new EFCuisineRepository(context);
        public IBasketRepository Baskets => new EFBasketRepository(context);
        public IOrderRepository Orders => new EFOrderRepository(context);
        public IBillingAccountRepository BillingAccounts => new EFBillingAccountRepository(context);
        public IOutbox Outbox => outbox;

        public async Task Commit()
        {
            if (outbox.Events.Any())
            {
                await outbox.Commit();
            }
            else
            {
                await context.SaveChangesAsync();
            }
        }
    }
}
