using System;
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
                new UserId(Guid.NewGuid()),
                "Ian Brown",
                new Email("browny@ian.com"),
                "bellona");

            restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                manager.Id,
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address("1 Maine Road, Manchester, UK"),
                new Coordinates(0, 0));

            context.RestaurantManagers.Add(manager);
            context.Restaurants.Add(restaurant);
        }

        [Fact]
        public async Task It_Adds_A_Menu_And_Gets_It_By_Id()
        {
            var menu = new Menu(new MenuId(Guid.NewGuid()), restaurant.Id);
            menu.AddCategory("Pizza");

            var category = menu.Categories.Single();
            menu.AddItem(category.Id, "Margherita", "Cheese & tomato", new Money(9.99m));

            await repository.Add(menu);
            FlushContext();

            var found = await repository.GetById(menu.Id);

            Assert.Equal(menu, found);

            Assert.Single(menu.Categories);

            var foundCategory = found.Categories.First();
            Assert.Equal("Pizza", foundCategory.Name);

            Assert.Single(foundCategory.Items);

            var item = foundCategory.Items.First();
            Assert.Equal("Margherita", item.Name);
        }
    }
}
