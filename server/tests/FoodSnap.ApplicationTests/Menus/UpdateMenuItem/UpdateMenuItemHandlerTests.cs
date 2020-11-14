using System.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;
using FoodSnap.Application.Menus.UpdateMenuItem;
using FoodSnap.ApplicationTests.Services.Authentication;
using FoodSnap.Domain;
using FoodSnap.Domain.Menus;
using FoodSnap.Domain.Restaurants;
using FoodSnap.Domain.Users;
using Xunit;
using static FoodSnap.Application.Error;
using Xunit.Abstractions;

namespace FoodSnap.ApplicationTests.Menus.UpdateMenuItem
{
    public class UpdateMenuItemHandlerTests
    {
        private readonly AuthenticatorSpy authenticatorSpy;
        private readonly UnitOfWorkSpy unitOfWorkSpy;
        private readonly UpdateMenuItemHandler handler;
        private readonly ITestOutputHelper output;

        public UpdateMenuItemHandlerTests(ITestOutputHelper output)
        {
            authenticatorSpy = new AuthenticatorSpy();
            unitOfWorkSpy = new UnitOfWorkSpy();

            handler = new UpdateMenuItemHandler(authenticatorSpy, unitOfWorkSpy);
            this.output = output;
        }

        [Fact]
        public async Task It_Updates_A_Menu_Item()
        {
            var authUser = new RestaurantManager(
                new UserId(Guid.NewGuid()),
                "Jordan Walker",
                new Email("walker.jlg@gmail.com"),
                "password123");
            await unitOfWorkSpy.UserRepositorySpy.Add(authUser);

            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                authUser.Id,
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address("12 Maine Road, Manchester, UK, MN12 1NM"),
                new Coordinates(1, 1));
            await unitOfWorkSpy.RestaurantRepositorySpy.Add(restaurant);

            var menu = new Menu(restaurant.Id);
            menu.AddCategory("Pizza");
            menu.Categories.Single().AddItem("Margherita", "Cheese & tomato", new Money(9.99m));
            await unitOfWorkSpy.MenuRepositorySpy.Add(menu);

            authenticatorSpy.SignIn(authUser);

            var command = new UpdateMenuItemCommand
            {
                RestaurantId = menu.RestaurantId.Value,
                CategoryName = "Pizza",
                OldItemName = "Margherita",
                NewItemName = "Hawaiian",
                Description = "Ham & pineapple",
                Price = 11.99m,
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.True(unitOfWorkSpy.Commited);

            var category = menu.Categories.Single();
            var item = category.Items.Single();

            Assert.Equal("Hawaiian", item.Name);
            Assert.Equal("Ham & pineapple", item.Description);
            Assert.Equal(new Money(11.99m), item.Price);
        }

        [Fact]
        public async Task It_Fails_If_Menu_Not_Found()
        {
            var authUser = new RestaurantManager(
                new UserId(Guid.NewGuid()),
                "Jordan Walker",
                new Email("walker.jlg@gmail.com"),
                "password123");
            await unitOfWorkSpy.UserRepositorySpy.Add(authUser);

            authenticatorSpy.SignIn(authUser);

            var command = new UpdateMenuItemCommand
            {
                RestaurantId = Guid.NewGuid(),
                CategoryName = "Pizza",
                OldItemName = "Margherita",
                NewItemName = "Hawaiian",
                Description = "Ham & pineapple",
                Price = 11.99m,
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

            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                authUser.Id,
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address("12 Maine Road, Manchester, UK, MN12 1NM"),
                new Coordinates(1, 1));

            var menu = new Menu(restaurant.Id);

            await unitOfWorkSpy.UserRepositorySpy.Add(authUser);
            await unitOfWorkSpy.RestaurantRepositorySpy.Add(restaurant);
            await unitOfWorkSpy.MenuRepositorySpy.Add(menu);

            authenticatorSpy.SignIn(authUser);

            var command = new UpdateMenuItemCommand
            {
                RestaurantId = menu.RestaurantId.Value,
                CategoryName = "Pizza",
                OldItemName = "Margherita",
                NewItemName = "Hawaiian",
                Description = "Ham & pineapple",
                Price = 11.99m,
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorType.NotFound, result.Error.Type);
        }

        [Fact]
        public async Task It_Fails_If_Item_Not_Found()
        {
            var authUser = new RestaurantManager(
                new UserId(Guid.NewGuid()),
                "Jordan Walker",
                new Email("walker.jlg@gmail.com"),
                "password123");

            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                authUser.Id,
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address("12 Maine Road, Manchester, UK, MN12 1NM"),
                new Coordinates(1, 1));

            var menu = new Menu(restaurant.Id);
            menu.AddCategory("Pizza");

            await unitOfWorkSpy.UserRepositorySpy.Add(authUser);
            await unitOfWorkSpy.RestaurantRepositorySpy.Add(restaurant);
            await unitOfWorkSpy.MenuRepositorySpy.Add(menu);

            authenticatorSpy.SignIn(authUser);

            var command = new UpdateMenuItemCommand
            {
                RestaurantId = menu.RestaurantId.Value,
                CategoryName = "Pizza",
                OldItemName = "Margherita",
                NewItemName = "Hawaiian",
                Description = "Ham & pineapple",
                Price = 11.99m,
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

            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                authUser.Id,
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address("12 Maine Road, Manchester, UK, MN12 1NM"),
                new Coordinates(1, 1));

            var menu = new Menu(restaurant.Id);
            menu.AddCategory("Pizza");
            menu.Categories.Single().AddItem("Margherita", "Cheese & tomato", new Money(9.99m));
            menu.Categories.Single().AddItem("Hawaiian", "Ham & pineapple", new Money(11.99m));

            await unitOfWorkSpy.UserRepositorySpy.Add(authUser);
            await unitOfWorkSpy.RestaurantRepositorySpy.Add(restaurant);
            await unitOfWorkSpy.MenuRepositorySpy.Add(menu);

            authenticatorSpy.SignIn(authUser);

            var command = new UpdateMenuItemCommand
            {
                RestaurantId = menu.RestaurantId.Value,
                CategoryName = "Pizza",
                OldItemName = "Margherita",
                NewItemName = "Hawaiian",
                Description = "Ham & pineapple",
                Price = 11.99m,
            };

            var result = await handler.Handle(command, CancellationToken.None);
            output.WriteLine(result.Error.Message);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorType.BadRequest, result.Error.Type);
        }
    }
}
