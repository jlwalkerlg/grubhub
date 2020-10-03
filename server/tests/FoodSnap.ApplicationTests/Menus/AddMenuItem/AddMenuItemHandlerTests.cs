using System.Threading;
using System;
using System.Threading.Tasks;
using FoodSnap.Application.Menus.AddMenuItem;
using FoodSnap.ApplicationTests.Doubles;
using FoodSnap.Domain;
using FoodSnap.Domain.Menus;
using FoodSnap.Domain.Restaurants;
using FoodSnap.Domain.Users;
using Xunit;
using static FoodSnap.Application.Error;
using System.Linq;

namespace FoodSnap.ApplicationTests.Menus.AddMenuItem
{
    public class AddMenuItemHandlerTests
    {
        private readonly AuthenticatorSpy authenticatorSpy;
        private readonly UnitOfWorkSpy unitOfWorkSpy;
        private readonly AddMenuItemHandler handler;

        public AddMenuItemHandlerTests()
        {
            authenticatorSpy = new AuthenticatorSpy();
            unitOfWorkSpy = new UnitOfWorkSpy();

            handler = new AddMenuItemHandler(authenticatorSpy, unitOfWorkSpy);
        }

        [Fact]
        public async Task It_Adds_An_Item_To_The_Menu()
        {
            var authUser = new RestaurantManager(
                new UserId(Guid.NewGuid()),
                "Jordan Walker",
                new Email("walker.jlg@gmail.com"),
                "password123");

            authenticatorSpy.User = authUser;

            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                authUser.Id,
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address("1 Maine Road, Manchester, UK"),
                new Coordinates(1, 1));

            await unitOfWorkSpy.RestaurantRepositorySpy.Add(restaurant);

            var menu = new Menu(restaurant.Id);

            menu.AddCategory("Pizza");

            await unitOfWorkSpy.MenuRepositorySpy.Add(menu);

            var category = menu.Categories.Single();

            var command = new AddMenuItemCommand
            {
                MenuId = menu.Id,
                CategoryId = category.Id,
                Name = "Margherita",
                Description = "Cheese & tomato",
                Price = 9.99m,
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.Single(
                menu.Categories.First(x => x.Name == "Pizza").Items,
                item => item.Id == result.Value);
        }

        [Fact]
        public async Task It_Fails_If_Menu_Not_Found()
        {
            var authUser = new RestaurantManager(
                new UserId(Guid.NewGuid()),
                "Jordan Walker",
                new Email("walker.jlg@gmail.com"),
                "password123");

            authenticatorSpy.User = authUser;

            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                authUser.Id,
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address("1 Maine Road, Manchester, UK"),
                new Coordinates(1, 1));

            await unitOfWorkSpy.RestaurantRepositorySpy.Add(restaurant);

            var command = new AddMenuItemCommand
            {
                MenuId = Guid.NewGuid(),
                CategoryId = Guid.NewGuid(),
                Name = "Margherita",
                Description = "Cheese & tomato",
                Price = 9.99m,
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorType.NotFound, result.Error.Type);
        }

        [Fact]
        public async Task It_Fails_If_Category_Not_Found()
        {
            var authUser = new RestaurantManager(
                new UserId(Guid.NewGuid()),
                "Jordan Walker",
                new Email("walker.jlg@gmail.com"),
                "password123");

            authenticatorSpy.User = authUser;

            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                authUser.Id,
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address("1 Maine Road, Manchester, UK"),
                new Coordinates(1, 1));

            await unitOfWorkSpy.RestaurantRepositorySpy.Add(restaurant);

            var menu = new Menu(restaurant.Id);

            await unitOfWorkSpy.MenuRepositorySpy.Add(menu);

            var command = new AddMenuItemCommand
            {
                MenuId = menu.Id,
                CategoryId = Guid.NewGuid(),
                Name = "Margherita",
                Description = "Cheese & tomato",
                Price = 9.99m,
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorType.NotFound, result.Error.Type);
        }

        [Fact]
        public async Task It_Requires_Authorisation()
        {
            var authUser = new RestaurantManager(
                new UserId(Guid.NewGuid()),
                "Jordan Walker",
                new Email("walker.jlg@gmail.com"),
                "password123");

            authenticatorSpy.User = authUser;

            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                new UserId(Guid.NewGuid()),
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address("1 Maine Road, Manchester, UK"),
                new Coordinates(1, 1));

            await unitOfWorkSpy.RestaurantRepositorySpy.Add(restaurant);

            var menu = new Menu(restaurant.Id);

            menu.AddCategory("Pizza");

            await unitOfWorkSpy.MenuRepositorySpy.Add(menu);

            var category = menu.Categories.Single();

            var command = new AddMenuItemCommand
            {
                MenuId = menu.Id,
                CategoryId = category.Id,
                Name = "Margherita",
                Description = "Cheese & tomato",
                Price = 9.99m,
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorType.Unauthorised, result.Error.Type);
        }
    }
}
