using System.Linq;
using System.Threading.Tasks;
using FoodSnap.Infrastructure.Persistence.EF.Repositories;
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
            FlushContext();

            var found = context.Events.First().ToEvent<DummyEvent>();

            Assert.Equal(ev.Name, found.Name);
            Assert.True(ev.CreatedAt.Subtract(found.CreatedAt).Seconds < 1);
        }
    }
}
