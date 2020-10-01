using System.Linq;
using System.Threading.Tasks;
using FoodSnap.Domain;
using FoodSnap.Domain.Restaurants;
using FoodSnap.Domain.Users;
using FoodSnap.Infrastructure.Persistence.EF.Repositories;
using Xunit;

namespace FoodSnap.InfrastructureTests.Persistence.EF.Repositories
{
    public class EFRestaurantRepositoryTests : EFRepositoryTestBase
    {
        private readonly EFRestaurantManagerRepository restaurantManagerRepository;
        private readonly EFRestaurantRepository restaurantRepository;

        public EFRestaurantRepositoryTests(EFContextFixture fixture) : base(fixture)
        {
            restaurantManagerRepository = new EFRestaurantManagerRepository(context);
            restaurantRepository = new EFRestaurantRepository(context);
        }

        [Fact]
        public async Task It_Adds_A_Restaurant()
        {
            var manager = new RestaurantManager(
                "Mr Chow",
                new Email("mr@chow.com"),
                "wongkarwai");

            var restaurant = new Restaurant(
                manager.Id,
                "Chow Main",
                new PhoneNumber("01234 567890"),
                new Address("1 Maine Road, Manchester, UK"),
                new Coordinates(0, 0)
            );

            await restaurantManagerRepository.Add(manager);
            await restaurantRepository.Add(restaurant);
            FlushContext();

            Assert.Single(context.Restaurants);

            var found = context.Restaurants.First();

            Assert.Equal(restaurant, found);
        }

        [Fact]
        public async Task It_Gets_A_Restaurant_By_Id()
        {
            var manager = new RestaurantManager(
                "Mr Chow",
                new Email("mr@chow.com"),
                "wongkarwai");

            var restaurant = new Restaurant(
                manager.Id,
                "Chow Main",
                new PhoneNumber("01234 567890"),
                new Address("1 Maine Road, Manchester, UK"),
                new Coordinates(0, 0)
            );

            context.RestaurantManagers.Add(manager);
            context.Restaurants.Add(restaurant);
            FlushContext();

            var found = await restaurantRepository.GetById(restaurant.Id);

            Assert.True(restaurant.Equals(found));
        }
    }
}
