using System.Threading.Tasks;

namespace Web.Services.Events
{
    public interface IEventStore
    {
        Task Store(Event @event);
    }
}
