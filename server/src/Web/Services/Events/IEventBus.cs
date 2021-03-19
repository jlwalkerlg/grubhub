using System.Threading.Tasks;

namespace Web.Services.Events
{
    public interface IEventBus
    {
        Task Publish(Event @event);
    }
}
