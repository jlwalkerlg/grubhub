using System.Threading;
using System.Threading.Tasks;

namespace Web.Services.Events
{
    public interface IEventListener<in TEvent> where TEvent : Event
    {
        Task Handle(TEvent @event, CancellationToken cancellationToken);
    }
}
