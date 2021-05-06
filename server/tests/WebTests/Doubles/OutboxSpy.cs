using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Services.Events;

namespace WebTests.Doubles
{
    public class OutboxSpy
    {
        public List<Event> Events { get; } = new();
    }
}
