using System;
using System.Threading.Tasks;
using Shouldly;
using Web;
using Web.Domain.Orders;
using Web.Domain.Restaurants;
using Web.Domain.Users;
using Web.Features.Orders.RemoveFromOrder;
using WebTests.Doubles;
using Xunit;

namespace WebTests.Features.Orders.RemoveFromOrder
{
    public class RemoveFromOrderHandlerTests
    {
        private readonly AuthenticatorSpy authenticatorSpy;
        private readonly UnitOfWorkSpy unitOfWorkSpy;
        private readonly RemoveFromOrderHandler handler;

        public RemoveFromOrderHandlerTests()
        {
            authenticatorSpy = new AuthenticatorSpy();

            unitOfWorkSpy = new UnitOfWorkSpy();

            handler = new RemoveFromOrderHandler(authenticatorSpy, unitOfWorkSpy);
        }

        [Fact]
        public async Task It_Fails_If_Order_Not_Found()
        {
            authenticatorSpy.SignIn();

            var command = new RemoveFromOrderCommand()
            {
                MenuItemId = Guid.NewGuid(),
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.NotFound);
        }

        [Fact]
        public async Task It_Fails_If_Order_Item_Not_Found()
        {
            var order = new Order(
                new OrderId(Guid.NewGuid()),
                new UserId(Guid.NewGuid()),
                new RestaurantId(Guid.NewGuid()));

            await unitOfWorkSpy.Orders.Add(order);

            authenticatorSpy.SignIn(order.UserId);

            var command = new RemoveFromOrderCommand()
            {
                MenuItemId = Guid.NewGuid(),
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.NotFound);
        }
    }
}
