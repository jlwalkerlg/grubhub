using System;
using FoodSnap.Domain;
using FoodSnap.Domain.Menus;
using Xunit;
using FoodSnap.Domain.Restaurants;
using static FoodSnap.Domain.Error;

namespace FoodSnap.DomainTests.Menus
{
    public class MenuTests
    {
        [Fact]
        public void Test_Equality()
        {
            var m1 = new Menu(new MenuId(Guid.NewGuid()), new RestaurantId(Guid.NewGuid()));
            var m2 = new Menu(new MenuId(Guid.NewGuid()), new RestaurantId(Guid.NewGuid()));
            var m3 = new Menu(m1.Id, new RestaurantId(Guid.NewGuid()));

            Assert.NotEqual(m1, m2);
            Assert.False(m1 == m2);
            Assert.False(m1.Equals(m2));

            Assert.Equal(m1, m3);
            Assert.True(m1 == m3);
            Assert.True(m1.Equals(m3));
        }

        [Fact]
        public void Id_Cant_Be_Null()
        {
            var restaurantId = new RestaurantId(Guid.NewGuid());

            Assert.Throws<ArgumentNullException>(() => new Menu(null, restaurantId));
        }

        [Fact]
        public void Restaurant_Id_Cant_Be_Null()
        {
            var id = new MenuId(Guid.NewGuid());

            Assert.Throws<ArgumentNullException>(() => new Menu(id, null));
        }

        [Fact]
        public void It_Cant_Add_Two_Categories_With_The_Same_Name()
        {
            var menu = new Menu(new MenuId(Guid.NewGuid()), new RestaurantId(Guid.NewGuid()));
            menu.AddCategory("Pizza");

            var result = menu.AddCategory("Pizza");

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorType.BadRequest, result.Error.Type);
        }

        [Fact]
        public void It_Cant_Add_Two_Items_To_The_Same_Category_With_The_Same_Name()
        {
            var menu = new Menu(new MenuId(Guid.NewGuid()), new RestaurantId(Guid.NewGuid()));
            menu.AddCategory("Pizza");
            menu.AddItem("Pizza", "Margherita", "Cheese & tomato", new Money(9.99m));

            var result = menu.AddItem("Pizza", "Margherita", "Cheese & tomato", new Money(9.99m));

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorType.BadRequest, result.Error.Type);
        }

        [Fact]
        public void It_Cant_Add_An_Item_If_The_Category_Doesnt_Exist()
        {
            var menu = new Menu(new MenuId(Guid.NewGuid()), new RestaurantId(Guid.NewGuid()));

            var result = menu.AddItem("Pizza", "Margherita", "Cheese & tomatoes", new Money(9.99m));

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorType.BadRequest, result.Error.Type);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Category_Name_Cant_Be_Empty(string categoryName)
        {
            var menu = new Menu(new MenuId(Guid.NewGuid()), new RestaurantId(Guid.NewGuid()));

            Assert.Throws<ArgumentException>(() => menu.AddCategory(categoryName));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Item_Name_Cant_Be_Empty(string itemName)
        {
            var menu = new Menu(new MenuId(Guid.NewGuid()), new RestaurantId(Guid.NewGuid()));
            menu.AddCategory("Pizza");

            Assert.Throws<ArgumentException>(() =>
            {
                menu.AddItem("Pizza", itemName, "", new Money(10));
            });
        }

        [Fact]
        public void Item_Price_Cant_Be_Null()
        {
            var menu = new Menu(new MenuId(Guid.NewGuid()), new RestaurantId(Guid.NewGuid()));
            menu.AddCategory("Pizza");

            Assert.Throws<ArgumentNullException>(() =>
            {
                menu.AddItem("Pizza", "Margherita", "", null);
            });
        }
    }
}
