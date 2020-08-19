using FoodSnap.Application.Events;

namespace FoodSnap.InfrstructureTests.Persistence.EF.Repositories.DummyEvent
{
    public class DummyEvent : Event
    {
        public string Name { get; }

        public DummyEvent(string name)
        {
            Name = name;
        }
    }
}
