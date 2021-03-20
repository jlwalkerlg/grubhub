using System.Threading;
using System.Threading.Tasks;

namespace Web.Services.Events
{
    public interface IEventBus
    {
        Task Publish<TEvent>(TEvent @event, CancellationToken cancellationToken) where TEvent : Event;
    }
}
