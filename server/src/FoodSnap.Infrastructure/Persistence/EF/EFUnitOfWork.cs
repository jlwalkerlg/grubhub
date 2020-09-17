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

        public EFUnitOfWork(
            AppDbContext context,
            EFRestaurantRepository restaurantRepository,
            EFRestaurantManagerRepository restaurantManagerRepository,
            EFEventRepository eventRepository)
        {
            this.context = context;
            RestaurantRepository = restaurantRepository;
            RestaurantManagerRepository = restaurantManagerRepository;
            EventRepository = eventRepository;
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
