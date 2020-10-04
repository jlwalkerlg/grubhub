using System.Linq;
using System;
using FoodSnap.Domain;
using FoodSnap.Domain.Menus;
using Xunit;
using FoodSnap.Domain.Restaurants;

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
        public void It_Adds_A_Category()
        {
            var menu = new Menu(new MenuId(Guid.NewGuid()), new RestaurantId(Guid.NewGuid()));
            menu.AddCategory("Pizza");

            Assert.Single(menu.Categories);
            Assert.Equal("Pizza", menu.Categories.Single().Name);
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

        [Fact]
        public void It_Cant_Add_Two_Categories_With_The_Same_Name()
        {
            var menu = new Menu(new MenuId(Guid.NewGuid()), new RestaurantId(Guid.NewGuid()));
            menu.AddCategory("Pizza");

            Assert.Throws<InvalidOperationException>(() => menu.AddCategory("Pizza"));
        }

        [Fact]
        public void It_Adds_A_Menu_Item()
        {
            var menu = new Menu(new MenuId(Guid.NewGuid()), new RestaurantId(Guid.NewGuid()));
            menu.AddCategory("Pizza");

            var category = menu.Categories.Single();
            menu.AddItem(category.Id, "Margherita", "Cheese & tomato", new Money(10));

            var item = category.Items.Single();
            Assert.Equal("Margherita", item.Name);
            Assert.Equal("Cheese & tomato", item.Description);
            Assert.Equal(new Money(10), item.Price);
        }

        [Fact]
        public void It_Cant_Add_An_Item_If_The_Category_Doesnt_Exist()
        {
            var menu = new Menu(new MenuId(Guid.NewGuid()), new RestaurantId(Guid.NewGuid()));

            var categoryId = Guid.NewGuid();

            Assert.Throws<InvalidOperationException>(() =>
            {
                menu.AddItem(
                    categoryId,
                    "Margherita Pizza",
                    "Cheese, tomatoes, thin crust",
                    new Money(10));
            });
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Item_Name_Cant_Be_Empty(string itemName)
        {
            var menu = new Menu(new MenuId(Guid.NewGuid()), new RestaurantId(Guid.NewGuid()));
            menu.AddCategory("Pizza");

            var category = menu.Categories.Single();

            Assert.Throws<ArgumentException>(() =>
            {
                menu.AddItem(category.Id, itemName, "", new Money(10));
            });
        }

        [Fact]
        public void Item_Price_Cant_Be_Null()
        {
            var menu = new Menu(new MenuId(Guid.NewGuid()), new RestaurantId(Guid.NewGuid()));
            menu.AddCategory("Pizza");

            var category = menu.Categories.Single();

            Assert.Throws<ArgumentNullException>(() =>
            {
                menu.AddItem(category.Id, "Margherita Pizza", "", null);
            });
        }
    }
}
