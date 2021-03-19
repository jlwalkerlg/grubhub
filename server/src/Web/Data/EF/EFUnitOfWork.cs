using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using Web.Data.EF.Repositories;
using Web.Features.Baskets;
using Web.Features.Billing;
using Web.Features.Cuisines;
using Web.Features.Menus;
using Web.Features.Orders;
using Web.Features.Restaurants;
using Web.Features.Users;

namespace Web.Data.EF
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext context;
        private readonly List<Func<IDbTransaction, Task>> subscribers = new();

        public EFUnitOfWork(AppDbContext context)
        {
            this.context = context;
        }

        public IRestaurantRepository Restaurants => new EFRestaurantRepository(context);
        public IMenuRepository Menus => new EFMenuRepository(context);
        public IUserRepository Users => new EFUserRepository(context);
        public ICuisineRepository Cuisines => new EFCuisineRepository(context);
        public IBasketRepository Baskets => new EFBasketRepository(context);
        public IOrderRepository Orders => new EFOrderRepository(context);
        public IBillingAccountRepository BillingAccounts => new EFBillingAccountRepository(context);

        public void Subscribe(Func<IDbTransaction, Task> subscriber)
        {
            subscribers.Add(subscriber);
        }

        public async Task Commit()
        {
            if (subscribers.Any())
            {
                await using var dbContextTransaction = await context.Database.BeginTransactionAsync();
                var transaction = dbContextTransaction.GetDbTransaction();

                foreach (var subscriber in subscribers)
                {
                    await subscriber(transaction);
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
