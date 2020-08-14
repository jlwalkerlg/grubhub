using System.Linq;
using System.Threading.Tasks;
using FoodSnap.Domain;
using FoodSnap.Domain.Restaurants;
using FoodSnap.Infrastructure.Persistence.EF.Repositories;
using Xunit;

namespace FoodSnap.InfrastructureTests.Persistence.EF.Repositories
{
    public class EFRestaurantApplicationRepositoryTests : EFRepositoryTestBase
    {
        private readonly EFRestaurantApplicationRepository repository;
        private readonly Restaurant restaurant;

        public EFRestaurantApplicationRepositoryTests(EFContextFixture fixture) : base(fixture)
        {
            repository = new EFRestaurantApplicationRepository(context);

            restaurant = new Restaurant(
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address(
                    "12 One Two",
                    "",
                    "Manchester",
                    new Postcode("MN12 1NM")),
                new Coordinates(0, 0));

            context.Restaurants.Add(restaurant);
        }

        [Fact]
        public async Task It_Adds_A_New_Application()
        {
            var application = new RestaurantApplication(restaurant.Id);

            await repository.Add(application);
            FlushContext();

            var found = context.RestaurantApplications
                .Where(x => x.Id == application.Id)
                .SingleOrDefault();

            Assert.NotNull(found);
            Assert.Equal(restaurant.Id, found.RestaurantId);
            Assert.Equal(application.Status, found.Status);
        }
    }
}
