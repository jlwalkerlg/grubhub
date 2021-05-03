using System.Threading.Tasks;
using DotNetCore.CAP;

namespace Web.Services.Events
{
    public interface IEventListener<in TEvent> : ICapSubscribe where TEvent : Event
    {
        Task Handle(TEvent @event);
    }
}
