using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Web.Domain.Billing;
using Web.Features.Billing;

namespace Web.Data.EF.Repositories
{
    public class EFBillingAccountRepository : IBillingAccountRepository
    {
        private readonly AppDbContext context;

        public EFBillingAccountRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task Add(BillingAccount account)
        {
            await context.BillingAccounts.AddAsync(account);
        }

        public async Task<BillingAccount> GetById(string id)
        {
            return await context.BillingAccounts
                .OrderBy(x => x.Id)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BillingAccount> GetByRestaurantId(Guid restaurantId)
        {
            return await context.BillingAccounts
                .OrderBy(x => x.RestaurantId)
                .SingleOrDefaultAsync(x => x.RestaurantId == restaurantId);
        }
    }
}
