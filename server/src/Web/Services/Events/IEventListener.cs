using MediatR;

namespace Web.Services.Events
{
    public interface IEventListener<in TEvent> : INotificationHandler<TEvent> where TEvent : Event
    {
    }
}
