using System.Collections.Generic;
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
        private readonly ICapPublisher publisher;

        public EFUnitOfWork(AppDbContext context, ICapPublisher publisher)
        {
            this.publisher = publisher;
            this.context = context;
        }

        public IRestaurantRepository Restaurants => new EFRestaurantRepository(context);
        public IMenuRepository Menus => new EFMenuRepository(context);
        public IUserRepository Users => new EFUserRepository(context);
        public ICuisineRepository Cuisines => new EFCuisineRepository(context);
        public IBasketRepository Baskets => new EFBasketRepository(context);
        public IOrderRepository Orders => new EFOrderRepository(context);
        public IBillingAccountRepository BillingAccounts => new EFBillingAccountRepository(context);

        private readonly List<Event> events = new();

        public Task Publish(Event @event)
        {
            events.Add(@event);
            return Task.CompletedTask;
        }

        public async Task Commit()
        {
            if (events.Any())
            {
                await using var transaction = context.Database.BeginTransaction(publisher, autoCommit: false);

                foreach (var @event in events)
                {
                    await publisher.PublishAsync(@event.GetType().Name, @event);
                }

                await context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            else
            {
                await context.SaveChangesAsync();
            }
        }
    }
}
