using System.Linq;
using System.Threading;
using System;
using System.Threading.Tasks;
using FoodSnap.Application.Menus.AddMenuItem;
using FoodSnap.ApplicationTests.Services.Authentication;
using FoodSnap.Domain;
using FoodSnap.Domain.Menus;
using FoodSnap.Domain.Restaurants;
using FoodSnap.Domain.Users;
using Xunit;
using static FoodSnap.Application.Error;

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
            authenticatorSpy.SignIn(authUser);

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

            var command = new AddMenuItemCommand
            {
                RestaurantId = menu.RestaurantId.Value,
                CategoryName = "Pizza",
                ItemName = "Margherita",
                Description = "Cheese & tomato",
                Price = 9.99m,
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.True(result.IsSuccess);

            var category = menu.Categories.Single();

            Assert.Single(category.Items);

            var item = category.Items.Single();

            Assert.Equal("Margherita", item.Name);
            Assert.Equal("Cheese & tomato", item.Description);
            Assert.Equal(new Money(9.99m), item.Price);
        }

        [Fact]
        public async Task It_Fails_If_Menu_Not_Found()
        {
            var authUser = new RestaurantManager(
                new UserId(Guid.NewGuid()),
                "Jordan Walker",
                new Email("walker.jlg@gmail.com"),
                "password123");
            authenticatorSpy.SignIn(authUser);

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
                RestaurantId = restaurant.Id.Value,
                CategoryName = "Pizza",
                ItemName = "Margherita",
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
            authenticatorSpy.SignIn(authUser);

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
                RestaurantId = menu.RestaurantId.Value,
                CategoryName = "Pizza",
                ItemName = "Margherita",
                Description = "Cheese & tomato",
                Price = 9.99m,
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorType.NotFound, result.Error.Type);
        }

        [Fact]
        public async Task It_Fails_If_Item_Already_Exists()
        {
            var authUser = new RestaurantManager(
                new UserId(Guid.NewGuid()),
                "Jordan Walker",
                new Email("walker.jlg@gmail.com"),
                "password123");
            authenticatorSpy.SignIn(authUser);

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
            menu.Categories.Single().AddItem("Margherita", "Cheese & tomato", new Money(9.99m));
            await unitOfWorkSpy.MenuRepositorySpy.Add(menu);

            var command = new AddMenuItemCommand
            {
                RestaurantId = menu.RestaurantId.Value,
                CategoryName = "Pizza",
                ItemName = "Margherita",
                Description = "Cheese & tomato",
                Price = 9.99m,
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorType.BadRequest, result.Error.Type);
        }

        [Fact]
        public async Task It_Requires_Authorisation()
        {
            var authUser = new RestaurantManager(
                new UserId(Guid.NewGuid()),
                "Jordan Walker",
                new Email("walker.jlg@gmail.com"),
                "password123");
            authenticatorSpy.SignIn(authUser);

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

            var command = new AddMenuItemCommand
            {
                RestaurantId = menu.RestaurantId.Value,
                CategoryName = "Pizza",
                ItemName = "Margherita",
                Description = "Cheese & tomato",
                Price = 9.99m,
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorType.Unauthorised, result.Error.Type);
        }
    }
}
