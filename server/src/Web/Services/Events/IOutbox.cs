using System.Threading.Tasks;

namespace Web.Services.Events
{
    public interface IOutbox
    {
        Task Add(Event @event);
    }
}
