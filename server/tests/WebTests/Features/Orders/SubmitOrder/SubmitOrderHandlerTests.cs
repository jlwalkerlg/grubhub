using System;
using System.Threading.Tasks;
using Shouldly;
using Web;
using Web.Domain;
using Web.Domain.Menus;
using Web.Domain.Orders;
using Web.Domain.Restaurants;
using Web.Domain.Users;
using Web.Features.Orders.SubmitOrder;
using WebTests.Doubles;
using Xunit;

namespace WebTests.Features.Orders.SubmitOrder
{
    public class SubmitOrderHandlerTests
    {
        private readonly AuthenticatorSpy authenticatorSpy;
        private readonly UnitOfWorkSpy unitOfWorkSpy;
        private readonly ClockStub clockStub;
        private readonly SubmitOrderHandler handler;

        public SubmitOrderHandlerTests()
        {
            authenticatorSpy = new AuthenticatorSpy();

            unitOfWorkSpy = new UnitOfWorkSpy();

            clockStub = new ClockStub();

            handler = new SubmitOrderHandler(authenticatorSpy, unitOfWorkSpy, clockStub);
        }

        [Fact]
        public async Task It_Fails_If_The_Restaurant_Is_Not_Found()
        {
            authenticatorSpy.SignIn();

            var command = new SubmitOrderCommand()
            {
                RestaurantId = Guid.NewGuid(),
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.NotFound);
        }

        [Fact]
        public async Task It_Fails_If_The_Order_Is_Not_Found()
        {
            var restaurant = new Restaurant(
               new RestaurantId(Guid.NewGuid()),
               new UserId(Guid.NewGuid()),
               "Chow Main",
               new PhoneNumber("01234567890"),
               new Address("12 Maine Road, Manchester, UK"),
               new Coordinates(54, -2));

            await unitOfWorkSpy.Restaurants.Add(restaurant);

            authenticatorSpy.SignIn();

            var command = new SubmitOrderCommand()
            {
                RestaurantId = restaurant.Id,
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.NotFound);
        }

        [Fact]
        public async Task It_Fails_If_The_Restaurant_Is_Closed()
        {
            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                new UserId(Guid.NewGuid()),
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address("12 Maine Road, Manchester, UK"),
                new Coordinates(54, -2));

            restaurant.OpeningTimes = null;

            var menu = new Menu(restaurant.Id);
            var pizza = menu.AddCategory(Guid.NewGuid(), "Pizza").Value;
            var margherita = pizza.AddItem(Guid.NewGuid(), "Margherita", null, new Money(9.99m)).Value;

            var order = new Order(
                new OrderId(Guid.NewGuid()),
                new UserId(Guid.NewGuid()),
                restaurant.Id);

            order.AddItem(margherita.Id, 1);

            await unitOfWorkSpy.Restaurants.Add(restaurant);
            await unitOfWorkSpy.Menus.Add(menu);
            await unitOfWorkSpy.Orders.Add(order);

            authenticatorSpy.SignIn(order.UserId);

            var command = new SubmitOrderCommand()
            {
                RestaurantId = order.RestaurantId,
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.BadRequest);
        }

        [Fact]
        public async Task It_Fails_If_The_Order_Has_No_Items()
        {
            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                new UserId(Guid.NewGuid()),
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address("12 Maine Road, Manchester, UK"),
                new Coordinates(54, -2));

            restaurant.OpeningTimes = OpeningTimes.Always;

            var menu = new Menu(restaurant.Id);
            var pizza = menu.AddCategory(Guid.NewGuid(), "Pizza").Value;
            var margherita = pizza.AddItem(Guid.NewGuid(), "Margherita", null, new Money(9.99m)).Value;

            var order = new Order(
                new OrderId(Guid.NewGuid()),
                new UserId(Guid.NewGuid()),
                restaurant.Id);

            await unitOfWorkSpy.Restaurants.Add(restaurant);
            await unitOfWorkSpy.Menus.Add(menu);
            await unitOfWorkSpy.Orders.Add(order);

            authenticatorSpy.SignIn(order.UserId);

            var command = new SubmitOrderCommand()
            {
                RestaurantId = order.RestaurantId,
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.BadRequest);
        }
    }
}
