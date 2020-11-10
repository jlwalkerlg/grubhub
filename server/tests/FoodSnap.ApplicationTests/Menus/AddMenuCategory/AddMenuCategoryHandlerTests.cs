using System.Linq;
using System.Threading;
using System;
using System.Threading.Tasks;
using FoodSnap.Application.Menus.AddMenuCategory;
using FoodSnap.ApplicationTests.Services.Authentication;
using FoodSnap.Domain;
using FoodSnap.Domain.Menus;
using FoodSnap.Domain.Restaurants;
using FoodSnap.Domain.Users;
using Xunit;
using static FoodSnap.Shared.Error;

namespace FoodSnap.ApplicationTests.Menus.AddMenuCategory
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
        public async Task It_Adds_A_Category_To_The_Menu()
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

            var command = new AddMenuCategoryCommand
            {
                RestaurantId = menu.RestaurantId.Value,
                Name = "Pizza",
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.True(result.IsSuccess);

            Assert.Single(menu.Categories);

            var category = menu.Categories.Single();
            Assert.Equal("Pizza", category.Name);
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

            var command = new AddMenuCategoryCommand
            {
                RestaurantId = Guid.NewGuid(),
                Name = "Pizza",
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorType.NotFound, result.Error.Type);
        }

        [Fact]
        public async Task It_Fails_If_Category_Already_Exists()
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

            var command = new AddMenuCategoryCommand
            {
                RestaurantId = menu.RestaurantId.Value,
                Name = "Pizza",
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
            await unitOfWorkSpy.MenuRepositorySpy.Add(menu);

            var command = new AddMenuCategoryCommand
            {
                RestaurantId = menu.RestaurantId.Value,
                Name = "Pizza",
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorType.Unauthorised, result.Error.Type);
        }
    }
}
