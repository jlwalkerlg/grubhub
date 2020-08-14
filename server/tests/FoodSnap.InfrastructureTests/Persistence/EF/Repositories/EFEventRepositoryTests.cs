using System.Linq;
using System.Threading.Tasks;
using FoodSnap.Infrastructure.Persistence.EF.Repositories;
using FoodSnap.InfrstructureTests.Persistence.EF.Repositories.DummyEvent;
using Xunit;

namespace FoodSnap.InfrastructureTests.Persistence.EF.Repositories
{
    public class EFEventRepositoryTests : EFRepositoryTestBase
    {
        private readonly EFEventRepository repository;

        public EFEventRepositoryTests(EFContextFixture fixture) : base(fixture)
        {
            repository = new EFEventRepository(context);
        }

        [Fact]
        public async Task It_Adds_A_Event()
        {
            var ev = new DummyEvent("Chow Main");

            await repository.Add(ev);
            context.SaveChanges();

            var foundDto = context.Events.First();
            var found = (DummyEvent)foundDto.Event;

            Assert.Equal(ev.Name, found.Name);
            Assert.Equal(ev.CreatedAt, found.CreatedAt);
            Assert.Equal(ev.CreatedAt, foundDto.CreatedAt);
        }
    }
}
