using MediatR;
using Web.Features.Events;

namespace Web
{
    public interface IEventListener<T> : INotificationHandler<T> where T : Event
    {
    }
}
