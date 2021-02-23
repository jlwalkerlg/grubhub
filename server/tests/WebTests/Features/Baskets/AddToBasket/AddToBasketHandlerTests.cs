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
    }
}
