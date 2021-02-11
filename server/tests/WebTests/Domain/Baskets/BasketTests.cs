using System;
using Shouldly;
using Web.Domain;
using Web.Domain.Baskets;
using Web.Domain.Menus;
using Web.Domain.Restaurants;
using Web.Domain.Users;
using Xunit;

namespace WebTests.Domain.Baskets
{
    public class BasketTests
    {
        [Fact]
        public void CalculateSubtotal_Calculates_The_Subtotal()
        {
            var menu = new Menu(
                new RestaurantId(Guid.NewGuid())
            );

            var pizza = menu.AddCategory(
                Guid.NewGuid(),
                "Pizza"
            ).Value;

            var curry = menu.AddCategory(
                Guid.NewGuid(),
                "Curry"
            ).Value;

            var margherita = pizza.AddItem(
                Guid.NewGuid(),
                "Margherita",
                null,
                new Money(9.99m)
            ).Value;

            var hawaiian = pizza.AddItem(
                Guid.NewGuid(),
                "Hawaiian",
                null,
                new Money(12.99m)
            ).Value;

            var masala = curry.AddItem(
                Guid.NewGuid(),
                "Masala",
                null,
                new Money(11.99m)
            ).Value;

            var basket = new Basket(
                new UserId(Guid.NewGuid()),
                menu.RestaurantId
            );

            basket.AddItem(margherita.Id, 2);
            basket.AddItem(hawaiian.Id, 1);
            basket.AddItem(masala.Id, 3);

            basket.CalculateSubtotal(menu).ShouldBe(new Money(68.94m));
        }

        [Fact]
        public void CalculateSubtotal_Throws_If_The_Menu_Item_Is_Not_Found()
        {
            var menu = new Menu(
                new RestaurantId(Guid.NewGuid())
            );

            var basket = new Basket(
                new UserId(Guid.NewGuid()),
                menu.RestaurantId
            );

            basket.AddItem(Guid.NewGuid(), 1);

            Should.Throw<InvalidOperationException>(
                () => basket.CalculateSubtotal(menu));
        }

        [Fact]
        public void CalculateSubtotal_Returns_Zero_If_The_Basket_Has_No_Items()
        {
            var menu = new Menu(
                new RestaurantId(Guid.NewGuid())
            );

            var basket = new Basket(
                new UserId(Guid.NewGuid()),
                menu.RestaurantId
            );

            basket.CalculateSubtotal(menu).ShouldBe(Money.Zero);
        }
    }
}
