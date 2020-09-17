using FoodSnap.Application.Events;

namespace FoodSnap.InfrastructureTests.Persistence.EF.Repositories
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
