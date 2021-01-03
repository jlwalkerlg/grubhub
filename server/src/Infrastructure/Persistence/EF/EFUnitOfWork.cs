using System.Threading.Tasks;
using Application;
using Application.Events;
using Application.Menus;
using Application.Restaurants;
using Application.Users;
using Infrastructure.Persistence.EF.Repositories;

namespace Infrastructure.Persistence.EF
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext context;

        public EFUnitOfWork(AppDbContext context)
        {
            this.context = context;
        }

        public IRestaurantRepository Restaurants => new EFRestaurantRepository(context);
        public IMenuRepository Menus => new EFMenuRepository(context);
        public IRestaurantManagerRepository RestaurantManagers => new EFRestaurantManagerRepository(context);
        public IUserRepository Users => new EFUserRepository(context);
        public IEventRepository Events => new EFEventRepository(context);
        public ICuisineRepository Cuisines => new EFCuisineRepository(context);

        public async Task Commit()
        {
            await context.SaveChangesAsync();
        }
    }
}
