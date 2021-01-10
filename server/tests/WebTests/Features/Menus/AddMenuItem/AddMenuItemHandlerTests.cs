using System;
using System.Threading.Tasks;
using Shouldly;
using Web.Domain;
using Web.Domain.Menus;
using Web.Domain.Restaurants;
using Web.Domain.Users;
using Web.Features.Menus.AddMenuItem;
using WebTests.Doubles;
using Xunit;
using Web;

namespace WebTests.Features.Menus.AddMenuItem
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
        public async Task It_Fails_If_The_Menu_Is_Not_Found()
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
                new Address("1 Maine Road, Manchester, UK"),
                new Coordinates(1, 1));

            await unitOfWorkSpy.RestaurantManagerRepositorySpy.Add(manager);
            await unitOfWorkSpy.RestaurantRepositorySpy.Add(restaurant);

            authenticatorSpy.SignIn(manager);

            var command = new AddMenuItemCommand()
            {
                RestaurantId = restaurant.Id,
                CategoryName = "Pizza",
                ItemName = "Margherita",
                Description = "Cheese & tomato",
                Price = 9.99m,
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.NotFound);
        }

        [Fact]
        public async Task It_Fails_If_The_User_Is_Unauthorised()
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
                new Address("1 Maine Road, Manchester, UK"),
                new Coordinates(1, 1));

            var menu = new Menu(restaurant.Id);

            menu.AddCategory("Pizza");

            await unitOfWorkSpy.RestaurantManagerRepositorySpy.Add(manager);
            await unitOfWorkSpy.RestaurantRepositorySpy.Add(restaurant);
            await unitOfWorkSpy.MenuRepositorySpy.Add(menu);

            authenticatorSpy.SignIn(Guid.NewGuid());

            var command = new AddMenuItemCommand()
            {
                RestaurantId = menu.RestaurantId,
                CategoryName = "Pizza",
                ItemName = "Margherita",
                Description = "Cheese & tomato",
                Price = 9.99m,
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.Unauthorised);
        }

        [Fact]
        public async Task It_Fails_If_The_Category_Is_Not_Found()
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
                new Address("1 Maine Road, Manchester, UK"),
                new Coordinates(1, 1));

            var menu = new Menu(restaurant.Id);

            await unitOfWorkSpy.RestaurantManagerRepositorySpy.Add(manager);
            await unitOfWorkSpy.MenuRepositorySpy.Add(menu);
            await unitOfWorkSpy.RestaurantRepositorySpy.Add(restaurant);

            authenticatorSpy.SignIn(manager);

            var command = new AddMenuItemCommand()
            {
                RestaurantId = menu.RestaurantId,
                CategoryName = "Pizza",
                ItemName = "Margherita",
                Description = "Cheese & tomato",
                Price = 9.99m,
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.NotFound);
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
                new Address("1 Maine Road, Manchester, UK"),
                new Coordinates(1, 1));

            var menu = new Menu(restaurant.Id);

            var category = menu.AddCategory("Pizza");

            category.AddItem("Margherita", "Cheese & tomato", new Money(9.99m));

            await unitOfWorkSpy.RestaurantManagerRepositorySpy.Add(manager);
            await unitOfWorkSpy.RestaurantRepositorySpy.Add(restaurant);
            await unitOfWorkSpy.MenuRepositorySpy.Add(menu);

            authenticatorSpy.SignIn(manager);

            var command = new AddMenuItemCommand()
            {
                RestaurantId = menu.RestaurantId,
                CategoryName = "Pizza",
                ItemName = "Margherita",
                Description = "Cheese & tomato",
                Price = 9.99m,
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.BadRequest);
        }
    }
}
