using System.Threading.Tasks;
using Web.Domain.Billing;

namespace Web.Features.Billing
{
    public interface IBillingAccountRepository
    {
        public Task Add(BillingAccount account);
        Task<BillingAccount> GetById(string id);
    }
}
