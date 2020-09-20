using System.Threading.Tasks;
using FoodSnap.Domain.Menus;

namespace FoodSnap.Application.Menus
{
    public interface IMenuRepository
    {
        Task Add(Menu menu);
    }
}
