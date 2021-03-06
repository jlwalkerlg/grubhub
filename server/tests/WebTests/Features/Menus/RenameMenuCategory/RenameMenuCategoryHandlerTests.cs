using Shouldly;
using System;
using System.Threading.Tasks;
using Web;
using Web.Domain;
using Web.Domain.Menus;
using Web.Domain.Restaurants;
using Web.Domain.Users;
using Web.Features.Menus.RenameMenuCategory;
using WebTests.Doubles;
using Xunit;

namespace WebTests.Features.Menus.RenameMenuCategory
{
    public class RenameMenuCategoryHandlerTests
    {
        private readonly UnitOfWorkSpy unitOfWorkSpy;
        private readonly AuthenticatorSpy authenticatorSpy;
        private readonly RenameMenuCategoryHandler handler;

        public RenameMenuCategoryHandlerTests()
        {
            unitOfWorkSpy = new UnitOfWorkSpy();
            authenticatorSpy = new AuthenticatorSpy();

            handler = new RenameMenuCategoryHandler(unitOfWorkSpy, authenticatorSpy);
        }

        [Fact]
        public async Task It_Fails_If_The_Menu_Is_Not_Found()
        {
            var command = new RenameMenuCategoryCommand()
            {
                RestaurantId = Guid.NewGuid(),
                CategoryId = Guid.NewGuid(),
                NewName = "Curry",
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
                new Address(
                    "12 Maine Road",
                    null,
                    "Manchester",
                    new Postcode("MN12 1NM")),
                new Coordinates(1, 1));

            var menu = new Menu(restaurant.Id);
            var category = menu.AddCategory(Guid.NewGuid(), "Pizza").Value;

            await unitOfWorkSpy.Users.Add(manager);
            await unitOfWorkSpy.Restaurants.Add(restaurant);
            await unitOfWorkSpy.Menus.Add(menu);

            await authenticatorSpy.SignIn(Guid.NewGuid());

            var command = new RenameMenuCategoryCommand()
            {
                RestaurantId = restaurant.Id,
                CategoryId = category.Id,
                NewName = "Curry",
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.Unauthorised);
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
                new Address(
                    "12 Maine Road",
                    null,
                    "Manchester",
                    new Postcode("MN12 1NM")),
                new Coordinates(1, 1));

            var menu = new Menu(restaurant.Id);

            await unitOfWorkSpy.Users.Add(manager);
            await unitOfWorkSpy.Restaurants.Add(restaurant);
            await unitOfWorkSpy.Menus.Add(menu);

            await authenticatorSpy.SignIn(manager);

            var command = new RenameMenuCategoryCommand()
            {
                RestaurantId = restaurant.Id,
                CategoryId = Guid.NewGuid(),
                NewName = "Curry",
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.BadRequest);
        }

        [Fact]
        public async Task It_Fails_If_The_Category_Already_Exists()
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
                new Address(
                    "12 Maine Road",
                    null,
                    "Manchester",
                    new Postcode("MN12 1NM")),
                new Coordinates(1, 1));

            var menu = new Menu(restaurant.Id);
            var category = menu.AddCategory(Guid.NewGuid(), "Pizza").Value;

            menu.AddCategory(Guid.NewGuid(), "Curry");

            await unitOfWorkSpy.Users.Add(manager);
            await unitOfWorkSpy.Restaurants.Add(restaurant);
            await unitOfWorkSpy.Menus.Add(menu);

            await authenticatorSpy.SignIn(manager);

            var command = new RenameMenuCategoryCommand()
            {
                RestaurantId = restaurant.Id,
                CategoryId = category.Id,
                NewName = "Curry",
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.BadRequest);
        }
    }
}
