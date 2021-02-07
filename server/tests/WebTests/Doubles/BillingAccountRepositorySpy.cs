using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Domain.Billing;
using Web.Domain.Restaurants;
using Web.Features.Billing;

namespace WebTests.Doubles
{
    public class BillingAccountRepositorySpy : IBillingAccountRepository
    {
        public List<BillingAccount> Accounts { get; set; } = new();

        public Task Add(BillingAccount account)
        {
            Accounts.Add(account);
            return Task.CompletedTask;
        }

        public Task<BillingAccount> GetById(BillingAccountId id)
        {
            return Task.FromResult(
                Accounts.SingleOrDefault(x => x.Id == id));
        }

        public Task<BillingAccount> GetByRestaurantId(RestaurantId restaurantId)
        {
            return Task.FromResult(
                Accounts.SingleOrDefault(x => x.RestaurantId == restaurantId));
        }
    }
}
