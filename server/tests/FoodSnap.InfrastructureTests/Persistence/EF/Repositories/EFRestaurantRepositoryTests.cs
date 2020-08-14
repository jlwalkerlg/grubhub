using System.Linq;
using System.Threading.Tasks;
using FoodSnap.Domain;
using FoodSnap.Domain.Restaurants;
using FoodSnap.Infrastructure.Persistence.EF.Repositories;
using Xunit;

namespace FoodSnap.InfrastructureTests.Persistence.EF.Repositories
{
    public class EFRestaurantRepositoryTests : EFRepositoryTestBase
    {
        private readonly EFRestaurantRepository repository;

        public EFRestaurantRepositoryTests(EFContextFixture fixture) : base(fixture)
        {
            repository = new EFRestaurantRepository(context);
        }

        [Fact]
        public async Task It_Adds_A_Restaurant()
        {
            var restaurant = new Restaurant(
                "Chow Main",
                new PhoneNumber("01234 567890"),
                new Address(
                    "19 Gold Road",
                    null,
                    "Manchester",
                    new Postcode("MN12 1NM")
                ),
                new Coordinates(0, 0)
            );

            await repository.Add(restaurant);
            FlushContext();

            Assert.Single(context.Restaurants);

            var found = context.Restaurants.First();

            Assert.Equal(restaurant.Id, found.Id);
            Assert.Equal(restaurant.Name, found.Name);
            Assert.Equal(restaurant.PhoneNumber, found.PhoneNumber);
            Assert.Equal(restaurant.Address, found.Address);
            Assert.Equal(restaurant.Coordinates, found.Coordinates);
        }
    }
}
