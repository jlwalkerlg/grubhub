using System.Threading.Tasks;

namespace Application.Events
{
    public interface IEventRepository
    {
        Task Add<TEvent>(TEvent ev) where TEvent : Event;
    }
}
