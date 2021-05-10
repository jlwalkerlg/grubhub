using System;
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
        private static readonly Dictionary<Type, IList<Type>> EventToListenersMap = new();

        private readonly AppDbContext context;
        private readonly ICapPublisher publisher;

        static EFUnitOfWork()
        {
            foreach (var type in typeof(Startup).Assembly.GetTypes())
            {
                foreach (var iface in type.GetInterfaces())
                {
                    if (!iface.IsGenericType) continue;
                    if (iface.GetGenericTypeDefinition() != typeof(IEventListener<>)) continue;

                    var eventType = iface.GetGenericArguments().Single();

                    if (!EventToListenersMap.ContainsKey(eventType))
                    {
                        EventToListenersMap.Add(eventType, new List<Type>());
                    }

                    EventToListenersMap[eventType].Add(type);
                }
            }
        }

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

        private List<Event> events { get; } = new();

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

                    foreach (var listener in EventToListenersMap[@event.GetType()])
                    {
                        await publisher.PublishAsync(listener.Name + "." + @event.GetType().Name, @event);
                    }
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
