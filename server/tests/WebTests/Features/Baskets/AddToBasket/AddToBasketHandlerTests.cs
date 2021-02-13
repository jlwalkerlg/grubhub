using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Web;
using Web.Domain;
using Web.Domain.Menus;
using Web.Domain.Baskets;
using Web.Domain.Restaurants;
using Web.Domain.Users;
using Web.Features.Baskets.AddToBasket;
using WebTests.Doubles;
using Xunit;

namespace WebTests.Features.Baskets.AddToBasket
{
    public class AddToBasketHandlerTests
    {
        private readonly AuthenticatorSpy authenticatorSpy;
        private readonly UnitOfWorkSpy unitOfWorkSpy;
        private readonly AddToBasketHandler handler;

        public AddToBasketHandlerTests()
        {
            authenticatorSpy = new AuthenticatorSpy();

            unitOfWorkSpy = new UnitOfWorkSpy();

            handler = new AddToBasketHandler(
                authenticatorSpy,
                unitOfWorkSpy);
        }

        [Fact]
        public async Task It_Fails_If_The_Menu_Is_Not_Found()
        {
            await authenticatorSpy.SignIn();

            var command = new AddToBasketCommand()
            {
                RestaurantId = Guid.NewGuid(),
                MenuItemId = Guid.NewGuid(),
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.NotFound);
        }

        [Fact]
        public async Task It_Fails_If_The_Menu_Item_Is_Not_Found()
        {
            var menu = new Menu(new RestaurantId(Guid.NewGuid()));

            await unitOfWorkSpy.Menus.Add(menu);

            await authenticatorSpy.SignIn();

            var command = new AddToBasketCommand()
            {
                RestaurantId = menu.RestaurantId,
                MenuItemId = Guid.NewGuid(),
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.NotFound);
        }

        [Fact]
        public async Task It_Sets_The_Quantity_If_The_Basket_Item_Already_Exists()
        {
            var menu = new Menu(new RestaurantId(Guid.NewGuid()));
            var menuCategory = menu.AddCategory(Guid.NewGuid(), "Pizza").Value;
            var menuItem = menuCategory.AddItem(Guid.NewGuid(), "Margherita", null, new Money(9.99m)).Value;

            var basket = new Basket(
                new UserId(Guid.NewGuid()),
                menu.RestaurantId);

            basket.AddItem(menuItem.Id, 1);

            await unitOfWorkSpy.Baskets.Add(basket);
            await unitOfWorkSpy.Menus.Add(menu);

            await authenticatorSpy.SignIn(basket.UserId);

            var command = new AddToBasketCommand()
            {
                RestaurantId = menu.RestaurantId,
                MenuItemId = menuItem.Id,
                Quantity = 3,
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeSuccessful();

            var basketItem = basket.Items.Single();

            basketItem.Quantity.ShouldBe(command.Quantity);
        }
    }
}
