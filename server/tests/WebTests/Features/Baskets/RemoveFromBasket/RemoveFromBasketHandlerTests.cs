using System;
using System.Threading.Tasks;
using Shouldly;
using Web;
using Web.Domain.Baskets;
using Web.Domain.Restaurants;
using Web.Domain.Users;
using Web.Features.Baskets.RemoveFromBasket;
using WebTests.Doubles;
using Xunit;

namespace WebTests.Features.Baskets.RemoveFromBasket
{
    public class RemoveFromBasketHandlerTests
    {
        private readonly AuthenticatorSpy authenticatorSpy;
        private readonly UnitOfWorkSpy unitOfWorkSpy;
        private readonly RemoveFromBasketHandler handler;

        public RemoveFromBasketHandlerTests()
        {
            authenticatorSpy = new AuthenticatorSpy();

            unitOfWorkSpy = new UnitOfWorkSpy();

            handler = new RemoveFromBasketHandler(authenticatorSpy, unitOfWorkSpy);
        }

        [Fact]
        public async Task It_Fails_If_The_Basket_Is_Not_Found()
        {
            await authenticatorSpy.SignIn();

            var command = new RemoveFromBasketCommand()
            {
                RestaurantId = Guid.NewGuid(),
                MenuItemId = Guid.NewGuid(),
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.NotFound);
        }

        [Fact]
        public async Task It_Fails_If_The_Basket_Item_Is_Not_Found()
        {
            var basket = new Basket(
                new UserId(Guid.NewGuid()),
                new RestaurantId(Guid.NewGuid()));

            await unitOfWorkSpy.Baskets.Add(basket);

            await authenticatorSpy.SignIn(basket.UserId);

            var command = new RemoveFromBasketCommand()
            {
                RestaurantId = Guid.NewGuid(),
                MenuItemId = Guid.NewGuid(),
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.NotFound);
        }
    }
}
