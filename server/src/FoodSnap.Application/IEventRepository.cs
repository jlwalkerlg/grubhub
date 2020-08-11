using System.Threading.Tasks;

namespace FoodSnap.Application
{
    public interface IEventRepository
    {
        Task Add<TEvent>(TEvent ev) where TEvent : Event;
    }
}
