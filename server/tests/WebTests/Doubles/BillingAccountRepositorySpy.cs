using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Domain.Billing;
using Web.Features.Billing;

namespace WebTests.Doubles
{
    public class BillingAccountRepositorySpy : IBillingAccountRepository
    {
        public List<BillingAccount> Accounts { get; } = new();

        public Task Add(BillingAccount account)
        {
            Accounts.Add(account);
            return Task.CompletedTask;
        }

        public Task<BillingAccount> GetById(string id)
        {
            return Task.FromResult(
                Accounts.SingleOrDefault(x => x.Id.Value == id));
        }
    }
}
