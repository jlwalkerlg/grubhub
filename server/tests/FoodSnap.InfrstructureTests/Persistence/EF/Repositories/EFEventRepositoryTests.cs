using System;
using System.Linq;
using System.Threading.Tasks;
using FoodSnap.Infrastructure.Persistence.EF.Repositories;
using FoodSnap.InfrstructureTests.Persistence.EF.Repositories.DummyEvent;
using Newtonsoft.Json;
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
            var name = "Chow Main";

            var ev = new DummyEvent(name);

            await repository.Add(ev);
            context.SaveChanges();

            var first = context.Events.First();
            var found = JsonConvert.DeserializeObject<DummyEvent>(first.Data);

            Assert.Equal(ev.Name, found.Name);
            // TODO: deserialize to the exact datetime
            Assert.True(ev.CreatedAt - found.CreatedAt < TimeSpan.FromSeconds(1));
        }
    }
}
