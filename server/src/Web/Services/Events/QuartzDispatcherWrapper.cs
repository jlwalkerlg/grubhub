using System.Threading;
using System.Threading.Tasks;

namespace Web.Services.Events
{
    public abstract class ListenerWrapperBase
    {
        public abstract Task Dispatch(object @event, object listener, CancellationToken cancellationToken);
    }

    public class ListenerWrapper<TEvent> : ListenerWrapperBase where TEvent : Event
    {
        public override Task Dispatch(object @event, object listener, CancellationToken cancellationToken)
        {
            return ((IEventListener<TEvent>)listener).Handle((TEvent)@event);
        }
    }
}
