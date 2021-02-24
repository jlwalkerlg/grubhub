namespace Web.Services.Events
{
    public interface IEventListener<T> : IRequestHandler<HandleEventCommand<T>> where T : Event
    {
    }
}
