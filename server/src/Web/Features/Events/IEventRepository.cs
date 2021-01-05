using System.Threading.Tasks;

namespace Web.Features.Events
{
    public interface IEventRepository
    {
        Task Add<TEvent>(TEvent ev) where TEvent : Event;
    }
}
