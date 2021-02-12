using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Web.Domain.Orders;
using Web.Features.Orders;

namespace Web.Data.EF.Repositories
{
    public class EFOrderRepository : IOrderRepository
    {
        private readonly AppDbContext context;

        public EFOrderRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task Add(Order order)
        {
            await context.Orders.AddAsync(order);
        }

        public Task<Order> GetByPaymentIntentId(string paymentIntentId)
        {
            return context.Orders
                .Where(x => x.PaymentIntentId == paymentIntentId)
                .Include(x => x.Items)
                .OrderBy(x => x.Id)
                .SingleOrDefaultAsync();
        }
    }
}
