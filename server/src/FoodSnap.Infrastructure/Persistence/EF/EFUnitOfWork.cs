using System.Threading.Tasks;
using FoodSnap.Application;
using FoodSnap.Application.Events;
using FoodSnap.Application.Restaurants;
using FoodSnap.Application.Users;
using FoodSnap.Infrastructure.Persistence.EF.Repositories;

namespace FoodSnap.Infrastructure.Persistence.EF
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext context;

        public EFUnitOfWork(AppDbContext context)
        {
            this.context = context;
        }

        public IRestaurantRepository Restaurants => new EFRestaurantRepository(context);
        public IRestaurantManagerRepository RestaurantManagers => new EFRestaurantManagerRepository(context);
        public IUserRepository Users => new EFUserRepository(context);
        public IEventRepository Events => new EFEventRepository(context);

        public async Task Commit()
        {
            await context.SaveChangesAsync();
        }
    }
}
