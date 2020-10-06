using System.Linq;
using System;
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

        public EFMenuRepositoryTests(EFContextFixture fixture) : base(fixture)
        {
            repository = new EFMenuRepository(context);
        }

        [Fact]
        public async Task It_Adds_A_Menu_And_Gets_It_By_Id()
        {
            var manager = new RestaurantManager(
                new UserId(Guid.NewGuid()),
                "Ian Brown",
                new Email("browny@ian.com"),
                "bellona");

            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                manager.Id,
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address("1 Maine Road, Manchester, UK"),
                new Coordinates(0, 0));

            context.RestaurantManagers.Add(manager);
            context.Restaurants.Add(restaurant);

            var menu = new Menu(new MenuId(Guid.NewGuid()), restaurant.Id);
            menu.AddCategory("Pizza");
            menu.Categories.Single().AddItem("Margherita", "Cheese & tomato", new Money(9.99m));

            await repository.Add(menu);
            FlushContext();

            var found = await repository.GetById(menu.Id);

            Assert.Equal(menu.Id, found.Id);
            Assert.Equal(menu.RestaurantId, found.RestaurantId);
            Assert.Single(found.Categories);

            var category = menu.Categories.Single();
            Assert.Equal("Pizza", category.Name);
            Assert.Single(category.Items);

            var item = category.Items.Single();
            Assert.Equal("Margherita", item.Name);
            Assert.Equal("Cheese & tomato", item.Description);
            Assert.Equal(new Money(9.99m), item.Price);
        }
    }
}
