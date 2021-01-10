using System.Threading.Tasks;
using Web.Data.EF.Repositories;
using Web.Features.Events;
using Web.Features.Menus;
using Web.Features.Restaurants;
using Web.Features.Users;

namespace Web.Data.EF
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
        public IUserRepository Users => new EFUserRepository(context);
        public IEventRepository Events => new EFEventRepository(context);
        public ICuisineRepository Cuisines => new EFCuisineRepository(context);

        public async Task Commit()
        {
            await context.SaveChangesAsync();
        }
    }
}
