using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using Web;
using Web.Domain;
using Web.Domain.Menus;
using Web.Domain.Restaurants;
using Web.Domain.Users;
using Web.Features.Menus.UpdateMenuItem;
using WebTests.Doubles;
using Xunit;

namespace WebTests.Features.Menus.UpdateMenuItem
{
    public class UpdateMenuItemHandlerTests
    {
        private readonly AuthenticatorSpy authenticatorSpy;
        private readonly UnitOfWorkSpy unitOfWorkSpy;
        private readonly UpdateMenuItemHandler handler;

        public UpdateMenuItemHandlerTests()
        {
            authenticatorSpy = new AuthenticatorSpy();
            unitOfWorkSpy = new UnitOfWorkSpy();

            handler = new UpdateMenuItemHandler(authenticatorSpy, unitOfWorkSpy);
        }

        [Fact]
        public async Task It_Fails_If_The_Menu_Is_Not_Found()
        {
            var manager = new RestaurantManager(
                new UserId(Guid.NewGuid()),
                "Jordan Walker",
                new Email("walker.jlg@gmail.com"),
                "password123");

            await unitOfWorkSpy.UserRepositorySpy.Add(manager);

            authenticatorSpy.SignIn(manager);

            var command = new UpdateMenuItemCommand()
            {
                RestaurantId = Guid.NewGuid(),
                CategoryName = "Pizza",
                OldItemName = "Margherita",
                NewItemName = "Hawaiian",
                Description = "Ham & pineapple",
                Price = 11.99m,
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.NotFound);
        }

        [Fact]
        public async Task It_Fails_If_The_Category_Doesnt_Exist()
        {
            var manager = new RestaurantManager(
                new UserId(Guid.NewGuid()),
                "Jordan Walker",
                new Email("walker.jlg@gmail.com"),
                "password123");

            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                manager.Id,
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address("12 Maine Road, Manchester, UK, MN12 1NM"),
                new Coordinates(1, 1));

            var menu = new Menu(restaurant.Id);

            await unitOfWorkSpy.UserRepositorySpy.Add(manager);
            await unitOfWorkSpy.RestaurantRepositorySpy.Add(restaurant);
            await unitOfWorkSpy.MenuRepositorySpy.Add(menu);

            authenticatorSpy.SignIn(manager);

            var command = new UpdateMenuItemCommand()
            {
                RestaurantId = menu.RestaurantId,
                CategoryName = "Pizza",
                OldItemName = "Margherita",
                NewItemName = "Hawaiian",
                Description = "Ham & pineapple",
                Price = 11.99m,
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.BadRequest);
        }

        [Fact]
        public async Task It_Fails_If_The_Item_Doesnt_Exist()
        {
            var manager = new RestaurantManager(
                new UserId(Guid.NewGuid()),
                "Jordan Walker",
                new Email("walker.jlg@gmail.com"),
                "password123");

            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                manager.Id,
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address("12 Maine Road, Manchester, UK, MN12 1NM"),
                new Coordinates(1, 1));

            var menu = new Menu(restaurant.Id);
            menu.AddCategory("Pizza");

            await unitOfWorkSpy.UserRepositorySpy.Add(manager);
            await unitOfWorkSpy.RestaurantRepositorySpy.Add(restaurant);
            await unitOfWorkSpy.MenuRepositorySpy.Add(menu);

            authenticatorSpy.SignIn(manager);

            var command = new UpdateMenuItemCommand
            {
                RestaurantId = menu.RestaurantId,
                CategoryName = "Pizza",
                OldItemName = "Margherita",
                NewItemName = "Hawaiian",
                Description = "Ham & pineapple",
                Price = 11.99m,
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.BadRequest);
        }

        [Fact]
        public async Task It_Fails_If_The_Item_Already_Exists()
        {
            var manager = new RestaurantManager(
                new UserId(Guid.NewGuid()),
                "Jordan Walker",
                new Email("walker.jlg@gmail.com"),
                "password123");

            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                manager.Id,
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address("12 Maine Road, Manchester, UK, MN12 1NM"),
                new Coordinates(1, 1));

            var menu = new Menu(restaurant.Id);
            menu.AddCategory("Pizza");
            menu.Categories.Single().AddItem("Margherita", "Cheese & tomato", new Money(9.99m));
            menu.Categories.Single().AddItem("Hawaiian", "Ham & pineapple", new Money(11.99m));

            await unitOfWorkSpy.UserRepositorySpy.Add(manager);
            await unitOfWorkSpy.RestaurantRepositorySpy.Add(restaurant);
            await unitOfWorkSpy.MenuRepositorySpy.Add(menu);

            authenticatorSpy.SignIn(manager);

            var command = new UpdateMenuItemCommand()
            {
                RestaurantId = menu.RestaurantId,
                CategoryName = "Pizza",
                OldItemName = "Margherita",
                NewItemName = "Hawaiian",
                Description = "Ham & pineapple",
                Price = 11.99m,
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.BadRequest);
        }
    }
}
