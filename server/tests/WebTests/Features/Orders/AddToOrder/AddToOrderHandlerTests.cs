using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Web;
using Web.Domain;
using Web.Domain.Menus;
using Web.Domain.Orders;
using Web.Domain.Restaurants;
using Web.Domain.Users;
using Web.Features.Orders.AddToOrder;
using WebTests.Doubles;
using Xunit;

namespace WebTests.Features.Orders.AddToOrder
{
    public class AddToOrderHandlerTests
    {
        private readonly AuthenticatorSpy authenticatorSpy;
        private readonly UnitOfWorkSpy unitOfWorkSpy;
        private readonly AddToOrderHandler handler;

        public AddToOrderHandlerTests()
        {
            authenticatorSpy = new AuthenticatorSpy();

            unitOfWorkSpy = new UnitOfWorkSpy();

            handler = new AddToOrderHandler(
                authenticatorSpy,
                unitOfWorkSpy);
        }

        [Fact]
        public async Task It_Fails_The_Menu_Is_Not_Found()
        {
            authenticatorSpy.SignIn();

            var command = new AddToOrderCommand()
            {
                RestaurantId = Guid.NewGuid(),
                MenuItemId = Guid.NewGuid(),
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.NotFound);
        }

        [Fact]
        public async Task It_Fails_The_Menu_Item_Is_Not_Found()
        {
            var menu = new Menu(new RestaurantId(Guid.NewGuid()));

            await unitOfWorkSpy.Menus.Add(menu);

            authenticatorSpy.SignIn();

            var command = new AddToOrderCommand()
            {
                RestaurantId = menu.RestaurantId,
                MenuItemId = Guid.NewGuid(),
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.NotFound);
        }

        [Fact]
        public async Task It_Increments_The_Quantity_If_The_Order_Item_Already_Exists()
        {
            var menu = new Menu(new RestaurantId(Guid.NewGuid()));
            var menuCategory = menu.AddCategory(Guid.NewGuid(), "Pizza").Value;
            var menuItem = menuCategory.AddItem(Guid.NewGuid(), "Margherita", null, new Money(9.99m)).Value;

            var order = new Order(
                new OrderId(Guid.NewGuid()),
                new UserId(Guid.NewGuid()),
                menu.RestaurantId);

            order.AddItem(menuItem.Id);

            await unitOfWorkSpy.Orders.Add(order);
            await unitOfWorkSpy.Menus.Add(menu);

            authenticatorSpy.SignIn(order.UserId);

            var command = new AddToOrderCommand()
            {
                RestaurantId = menu.RestaurantId,
                MenuItemId = menuItem.Id,
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeSuccessful();

            var orderItem = order.Items.Single();

            orderItem.Quantity.ShouldBe(2);
        }

        [Fact]
        public async Task It_Starts_A_New_Order_If_An_Active_Order_At_Another_Restaurant_Already_Exists()
        {
            var existingOrder = new Order(
                new OrderId(Guid.NewGuid()),
                new UserId(Guid.NewGuid()),
                new RestaurantId(Guid.NewGuid()));

            var menu = new Menu(new RestaurantId(Guid.NewGuid()));
            var menuCategory = menu.AddCategory(Guid.NewGuid(), "Pizza").Value;
            var menuItem = menuCategory.AddItem(Guid.NewGuid(), "Margherita", null, new Money(9.99m)).Value;

            await unitOfWorkSpy.Orders.Add(existingOrder);
            await unitOfWorkSpy.Menus.Add(menu);

            authenticatorSpy.SignIn(existingOrder.UserId);

            var command = new AddToOrderCommand()
            {
                RestaurantId = menu.RestaurantId,
                MenuItemId = menuItem.Id,
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeSuccessful();

            unitOfWorkSpy.OrderRepositorySpy.Orders.Count.ShouldBe(2);

            existingOrder.Status.ShouldBe(OrderStatus.Cancelled);

            var newOrder = unitOfWorkSpy.OrderRepositorySpy.Orders
                .Single(x => x.RestaurantId == menu.RestaurantId);

            newOrder.Items.ShouldHaveSingleItem();

            var orderItem = newOrder.Items.Single();

            orderItem.Quantity.ShouldBe(1);
            orderItem.MenuItemId.ShouldBe(menuItem.Id);
        }
    }
}
