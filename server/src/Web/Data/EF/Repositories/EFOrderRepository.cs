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

        public async Task<Order> GetById(OrderId orderId)
        {
            return await context.Orders
                .Where(x => x.Id == orderId)
                .Include(x => x.Items)
                .OrderBy(x => x.Id)
                .SingleOrDefaultAsync();
        }
    }
}
