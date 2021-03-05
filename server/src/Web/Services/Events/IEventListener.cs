namespace Web.Services.Events
{
    public interface IEventListener<TEvent> : IRequestHandler<TEvent> where TEvent : Event
    {
    }
}
