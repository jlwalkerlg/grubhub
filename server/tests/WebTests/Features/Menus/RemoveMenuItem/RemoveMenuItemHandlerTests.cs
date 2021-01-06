using System;
using System.Linq;
using System.Threading.Tasks;
using Web.Domain;
using Web.Domain.Menus;
using Web.Domain.Restaurants;
using Web.Domain.Users;
using Web.Features.Menus.RemoveMenuItem;
using WebTests.Doubles;
using Xunit;
using static Web.Error;

namespace WebTests.Features.Menus.RemoveMenuItem
{
    public class RemoveMenuItemHandlerTests
    {
        private readonly AuthenticatorSpy authenticatorSpy;
        private readonly UnitOfWorkSpy unitOfWorkSpy;
        private readonly RemoveMenuItemHandler handler;

        public RemoveMenuItemHandlerTests()
        {
            authenticatorSpy = new AuthenticatorSpy();
            unitOfWorkSpy = new UnitOfWorkSpy();

            handler = new RemoveMenuItemHandler(authenticatorSpy, unitOfWorkSpy);
        }

        [Fact]
        public async Task It_Removes_A_Menu_Item()
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

            var command = new RemoveMenuItemCommand
            {
                RestaurantId = menu.RestaurantId.Value,
                CategoryName = "Pizza",
                ItemName = "Margherita",
            };

            var result = await handler.Handle(command, default);

            Assert.True(result);
            Assert.True(unitOfWorkSpy.Commited);

            var category = menu.Categories.Single();

            Assert.Empty(category.Items);
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

            var command = new RemoveMenuItemCommand
            {
                RestaurantId = Guid.NewGuid(),
                CategoryName = "Pizza",
                ItemName = "Margherita",
            };

            var result = await handler.Handle(command, default);

            Assert.False(result);
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

            var command = new RemoveMenuItemCommand
            {
                RestaurantId = menu.RestaurantId.Value,
                CategoryName = "Pizza",
                ItemName = "Margherita",
            };

            var result = await handler.Handle(command, default);

            Assert.False(result);
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

            var command = new RemoveMenuItemCommand
            {
                RestaurantId = menu.RestaurantId.Value,
                CategoryName = "Pizza",
                ItemName = "Margherita",
            };

            var result = await handler.Handle(command, default);

            Assert.False(result);
            Assert.Equal(ErrorType.NotFound, result.Error.Type);
        }
    }
}
