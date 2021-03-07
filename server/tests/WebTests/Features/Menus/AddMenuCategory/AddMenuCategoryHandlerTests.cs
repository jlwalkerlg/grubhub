using Shouldly;
using System;
using System.Threading.Tasks;
using Web;
using Web.Domain;
using Web.Domain.Menus;
using Web.Domain.Restaurants;
using Web.Domain.Users;
using Web.Features.Menus.AddMenuCategory;
using WebTests.Doubles;
using Xunit;

namespace WebTests.Features.Menus.AddMenuCategory
{
    public class AddMenuCategoryHandlerTests
    {
        private readonly AuthenticatorSpy authenticatorSpy;
        private readonly UnitOfWorkSpy unitOfWorkSpy;
        private readonly AddMenuCategoryHandler handler;

        public AddMenuCategoryHandlerTests()
        {
            authenticatorSpy = new AuthenticatorSpy();
            unitOfWorkSpy = new UnitOfWorkSpy();

            handler = new AddMenuCategoryHandler(authenticatorSpy, unitOfWorkSpy);
        }

        [Fact]
        public async Task It_Fails_If_The_Restaurant_Is_Not_Found()
        {
            await authenticatorSpy.SignIn();

            var command = new AddMenuCategoryCommand()
            {
                RestaurantId = Guid.NewGuid(),
                Name = "Pizza",
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
                "Jordan",
                "Walker",
                new Email("walker.jlg@gmail.com"),
                "password123");

            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                manager.Id,
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address(
                    "12 Maine Road",
                    null,
                    "Manchester",
                    new Postcode("MN12 1NM")),
                new Coordinates(1, 1));

            var menu = new Menu(restaurant.Id);

            await unitOfWorkSpy.UserRepositorySpy.Add(manager);
            await unitOfWorkSpy.RestaurantRepositorySpy.Add(restaurant);
            await unitOfWorkSpy.MenuRepositorySpy.Add(menu);

            await authenticatorSpy.SignIn(Guid.NewGuid());

            var command = new AddMenuCategoryCommand()
            {
                RestaurantId = menu.RestaurantId.Value,
                Name = "Pizza",
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.Unauthorised);
        }

        [Fact]
        public async Task It_Fails_If_The_Category_Already_Exists()
        {
            var manager = new RestaurantManager(
                new UserId(Guid.NewGuid()),
                "Jordan",
                "Walker",
                new Email("walker.jlg@gmail.com"),
                "password123");

            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                manager.Id,
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address(
                    "12 Maine Road",
                    null,
                    "Manchester",
                    new Postcode("MN12 1NM")),
                new Coordinates(1, 1));

            var menu = new Menu(restaurant.Id);

            menu.AddCategory(Guid.NewGuid(), "Pizza");

            await unitOfWorkSpy.UserRepositorySpy.Add(manager);
            await unitOfWorkSpy.RestaurantRepositorySpy.Add(restaurant);
            await unitOfWorkSpy.MenuRepositorySpy.Add(menu);

            await authenticatorSpy.SignIn(manager);

            var command = new AddMenuCategoryCommand()
            {
                RestaurantId = menu.RestaurantId.Value,
                Name = "Pizza",
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.BadRequest);
        }
    }
}
