using System;
using System.Threading.Tasks;
using Shouldly;
using Web.Domain;
using Web.Domain.Menus;
using Web.Domain.Restaurants;
using Web.Domain.Users;
using Web.Features.Menus.RemoveMenuCategory;
using WebTests.Doubles;
using Xunit;
using Web;

namespace WebTests.Features.Menus.RemoveMenuCategory
{
    public class RemoveMenuCategoryHandlerTests
    {
        private readonly UnitOfWorkSpy unitOfWorkSpy;
        private readonly AuthenticatorSpy authenticatorSpy;
        private readonly RemoveMenuCategoryHandler handler;

        public RemoveMenuCategoryHandlerTests()
        {
            unitOfWorkSpy = new UnitOfWorkSpy();
            authenticatorSpy = new AuthenticatorSpy();

            handler = new RemoveMenuCategoryHandler(unitOfWorkSpy, authenticatorSpy);
        }

        [Fact]
        public async Task It_Fails_If_The_Menu_Is_Not_Found()
        {
            var command = new RemoveMenuCategoryCommand()
            {
                RestaurantId = Guid.NewGuid(),
                CategoryName = "Pizza",
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
                new Address("12 Maine Road, Manchester, UK, MN12 1NM"),
                new Coordinates(1, 1));

            var menu = new Menu(restaurant.Id);

            await unitOfWorkSpy.Users.Add(manager);
            await unitOfWorkSpy.Restaurants.Add(restaurant);
            await unitOfWorkSpy.Menus.Add(menu);

            authenticatorSpy.SignIn(Guid.NewGuid());

            var command = new RemoveMenuCategoryCommand()
            {
                RestaurantId = menu.RestaurantId,
                CategoryName = "Pizza",
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
                new Address("12 Maine Road, Manchester, UK, MN12 1NM"),
                new Coordinates(1, 1));

            var menu = new Menu(restaurant.Id);

            await unitOfWorkSpy.Users.Add(manager);
            await unitOfWorkSpy.Restaurants.Add(restaurant);
            await unitOfWorkSpy.Menus.Add(menu);

            authenticatorSpy.SignIn(manager);

            var command = new RemoveMenuCategoryCommand()
            {
                RestaurantId = menu.RestaurantId,
                CategoryName = "Pizza",
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.BadRequest);
        }
    }
}
