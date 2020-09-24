using FoodSnap.Application.Events;

namespace FoodSnap.InfrastructureTests.Persistence
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
