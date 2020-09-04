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
            RestaurantRepository = new EFRestaurantRepository(context);
            RestaurantManagerRepository = new EFRestaurantManagerRepository(context);
            EventRepository = new EFEventRepository(context);

            this.context = context;
        }

        public IRestaurantRepository RestaurantRepository { get; }
        public IRestaurantManagerRepository RestaurantManagerRepository { get; }
        public IEventRepository EventRepository { get; }

        public async Task Commit()
        {
            await context.SaveChangesAsync();
        }
    }
}
