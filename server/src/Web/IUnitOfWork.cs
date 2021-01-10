using System.Threading.Tasks;
using Web.Features.Events;
using Web.Features.Menus;
using Web.Features.Restaurants;
using Web.Features.Users;

namespace Web
{
    public interface IUnitOfWork
    {
        IRestaurantRepository Restaurants { get; }
        IMenuRepository Menus { get; }
        IUserRepository Users { get; }
        IEventRepository Events { get; }
        ICuisineRepository Cuisines { get; }

        Task Commit();
    }
}
