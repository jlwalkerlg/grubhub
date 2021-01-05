using System;
using System.Threading;
using System.Threading.Tasks;
using Web.Domain;
using Web.Domain.Menus;
using Web.Domain.Restaurants;
using Web.Domain.Users;
using Web.Features.Menus.RenameMenuCategory;
using WebTests.Doubles;
using Xunit;
using static Web.Error;

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
        public async Task It_Renames_A_Menu_Category()
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

            var command = new RenameMenuCategoryCommand
            {
                RestaurantId = restaurant.Id.Value,
                OldName = "Pizza",
                NewName = "Curry",
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.False(menu.ContainsCategory("Pizza"));
            Assert.True(menu.ContainsCategory("Curry"));
        }

        [Fact]
        public async Task It_Fails_If_Menu_Not_Found()
        {
            var command = new RenameMenuCategoryCommand
            {
                RestaurantId = Guid.NewGuid(),
                OldName = "Pizza",
                NewName = "Curry",
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorType.NotFound, result.Error.Type);
        }

        [Fact]
        public async Task It_Fails_If_Category_Not_Found()
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

            var command = new RenameMenuCategoryCommand
            {
                RestaurantId = restaurant.Id.Value,
                OldName = "Pizza",
                NewName = "Curry",
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorType.NotFound, result.Error.Type);
        }

        [Fact]
        public async Task It_Fails_If_Category_Already_Exists()
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
            menu.AddCategory("Curry");

            await unitOfWorkSpy.RestaurantManagers.Add(manager);
            await unitOfWorkSpy.Restaurants.Add(restaurant);
            await unitOfWorkSpy.Menus.Add(menu);

            authenticatorSpy.SignIn(manager);

            var command = new RenameMenuCategoryCommand
            {
                RestaurantId = restaurant.Id.Value,
                OldName = "Pizza",
                NewName = "Curry",
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorType.BadRequest, result.Error.Type);
        }

        [Fact]
        public async Task Unauthorised_If_Not_The_Restaurant_Manager()
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
            menu.AddCategory("Pizza");

            await unitOfWorkSpy.RestaurantManagers.Add(manager);
            await unitOfWorkSpy.Restaurants.Add(restaurant);
            await unitOfWorkSpy.Menus.Add(menu);

            authenticatorSpy.SignIn(manager);

            var command = new RenameMenuCategoryCommand
            {
                RestaurantId = restaurant.Id.Value,
                OldName = "Pizza",
                NewName = "Curry",
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorType.Unauthorised, result.Error.Type);
        }
    }
}
