using System.Linq;
using System.Threading.Tasks;
using FoodSnap.Domain;
using FoodSnap.Domain.Users;
using FoodSnap.Infrastructure.Persistence.EF.Repositories;
using Xunit;

namespace FoodSnap.InfrastructureTests.Persistence.EF.Repositories
{
    public class EFRestaurantManagerRepositoryTests : EFRepositoryTestBase
    {
        private readonly EFRestaurantManagerRepository repository;

        public EFRestaurantManagerRepositoryTests(EFContextFixture fixture) : base(fixture)
        {
            repository = new EFRestaurantManagerRepository(context);
        }

        [Fact]
        public async Task It_Adds_A_Manager()
        {
            var manager = new RestaurantManager(
                "Jordan Walker",
                new Email("test@email.com"),
                "password123");

            await repository.Add(manager);
            FlushContext();

            var found = context.RestaurantManagers.First();

            Assert.Equal(manager.Id, found.Id);
            Assert.Equal(manager.Name, found.Name);
            Assert.Equal(manager.Email, found.Email);
            Assert.Equal(manager.Password, found.Password);
        }
    }
}
