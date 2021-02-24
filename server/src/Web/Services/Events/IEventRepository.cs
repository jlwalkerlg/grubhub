using System.Threading.Tasks;

namespace Web.Services.Events
{
    public interface IEventRepository
    {
        Task Add<TEvent>(TEvent ev) where TEvent : Event;
    }
}
