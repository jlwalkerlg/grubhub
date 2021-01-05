using System.Threading;
using System;
using System.Threading.Tasks;
using Web.Features.Menus.RemoveMenuCategory;
using Web.Domain.Menus;
using Web.Domain.Restaurants;
using Xunit;
using static Web.Error;
using Web.Domain.Users;
using Web.Domain;
using WebTests.Services.Authentication;
using WebTests.Doubles;

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
        public async Task It_Removes_A_Category_From_The_Menu()
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

            await unitOfWorkSpy.RestaurantManagers.Add(manager);
            await unitOfWorkSpy.Restaurants.Add(restaurant);
            await unitOfWorkSpy.Menus.Add(menu);

            authenticatorSpy.SignIn(manager);

            var command = new RemoveMenuCategoryCommand
            {
                RestaurantId = menu.RestaurantId.Value,
                CategoryName = "Pizza",
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.True(unitOfWorkSpy.Commited);
            Assert.False(menu.ContainsCategory("Pizza"));
        }

        [Fact]
        public async Task It_Fails_If_Menu_Not_Found()
        {
            var command = new RemoveMenuCategoryCommand
            {
                RestaurantId = Guid.NewGuid(),
                CategoryName = "Pizza",
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorType.NotFound, result.Error.Type);
        }

        [Fact]
        public async Task It_Fails_If_Category_Doesnt_Exist()
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

            await unitOfWorkSpy.RestaurantManagers.Add(manager);
            await unitOfWorkSpy.Restaurants.Add(restaurant);
            await unitOfWorkSpy.Menus.Add(menu);

            authenticatorSpy.SignIn(manager);

            var command = new RemoveMenuCategoryCommand
            {
                RestaurantId = menu.RestaurantId.Value,
                CategoryName = "Pizza",
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorType.NotFound, result.Error.Type);
        }

        [Fact]
        public async Task Unauthorised_If_Restaurant_Doesnt_Belong_To_Auth_User()
        {
            var manager = new RestaurantManager(
                new UserId(Guid.NewGuid()),
                "Jordan Walker",
                new Email("walker.jlg@gmail.com"),
                "password123");

            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                new UserId(Guid.NewGuid()),
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address("12 Maine Road, Manchester, UK, MN12 1NM"),
                new Coordinates(1, 1));

            var menu = new Menu(restaurant.Id);

            await unitOfWorkSpy.RestaurantManagers.Add(manager);
            await unitOfWorkSpy.Restaurants.Add(restaurant);
            await unitOfWorkSpy.Menus.Add(menu);

            var command = new RemoveMenuCategoryCommand
            {
                RestaurantId = menu.RestaurantId.Value,
                CategoryName = "Pizza",
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorType.Unauthorised, result.Error.Type);
        }
    }
}
