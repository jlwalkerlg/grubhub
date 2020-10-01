using System.Linq;
using System.Threading.Tasks;
using FoodSnap.Domain;
using FoodSnap.Domain.Menus;
using FoodSnap.Domain.Restaurants;
using FoodSnap.Domain.Users;
using FoodSnap.Infrastructure.Persistence.EF.Repositories;
using Xunit;

namespace FoodSnap.InfrastructureTests.Persistence.EF.Repositories
{
    public class EFMenuRepositoryTests : EFRepositoryTestBase
    {
        private readonly EFMenuRepository repository;

        private readonly Restaurant restaurant;

        public EFMenuRepositoryTests(EFContextFixture fixture) : base(fixture)
        {
            repository = new EFMenuRepository(context);

            var manager = new RestaurantManager(
                "Ian Brown",
                new Email("browny@ian.com"),
                "bellona");

            restaurant = new Restaurant(
                manager.Id,
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address("1 Maine Road, Manchester, UK"),
                new Coordinates(0, 0));

            context.RestaurantManagers.Add(manager);
            context.Restaurants.Add(restaurant);
        }

        [Fact]
        public async Task It_Adds_A_Menu()
        {
            var menu = new Menu(restaurant.Id);

            await repository.Add(menu);
            FlushContext();

            var found = context.Menus.Single();

            Assert.Equal(menu, found);
        }
    }
}
