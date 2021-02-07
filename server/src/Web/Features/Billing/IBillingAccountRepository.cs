using System.Threading.Tasks;
using Web.Domain.Billing;
using Web.Domain.Restaurants;

namespace Web.Features.Billing
{
    public interface IBillingAccountRepository
    {
        public Task Add(BillingAccount account);
        Task<BillingAccount> GetByRestaurantId(RestaurantId restaurantId);
        Task<BillingAccount> GetById(BillingAccountId id);
    }
}
