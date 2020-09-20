using System;
using FoodSnap.Domain;
using FoodSnap.Domain.Menus;
using Xunit;

namespace FoodSnap.DomainTests.Menus
{
    public class MenuTests
    {
        [Fact]
        public void Restaurant_Id_Cant_Be_Empty()
        {
            var menu = new Menu(Guid.Empty);

            menu.AddCategory("Pizza");
            Assert.Throws<InvalidOperationException>(() => menu.AddCategory("Pizza"));
        }

        [Fact]
        public void It_Cant_Add_Two_Categories_With_The_Same_Name()
        {
            var menu = new Menu(Guid.NewGuid());

            menu.AddCategory("Pizza");
            Assert.Throws<InvalidOperationException>(() => menu.AddCategory("Pizza"));
        }

        [Fact]
        public void It_Cant_Add_An_Item_If_The_Category_Doesnt_Exist()
        {
            var menu = new Menu(Guid.NewGuid());

            Assert.Throws<InvalidOperationException>(() =>
            {
                var category = "Pizza";
                menu.AddItem(
                    category,
                    "Margherita Pizza",
                    "Cheese, tomatoes, thin crust",
                    new Money(10));
            });
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Category_Name_Cant_Be_Empty(string categoryName)
        {
            var menu = new Menu(Guid.NewGuid());

            Assert.Throws<ArgumentException>(() => menu.AddCategory(categoryName));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Item_Name_Cant_Be_Empty(string itemName)
        {
            var menu = new Menu(Guid.NewGuid());
            menu.AddCategory("Pizza");

            Assert.Throws<ArgumentException>(() =>
            {
                menu.AddItem("Pizza", itemName, "", new Money(10));
            });
        }

        [Fact]
        public void Item_Price_Cant_Be_Null()
        {
            var menu = new Menu(Guid.NewGuid());
            menu.AddCategory("Pizza");

            Assert.Throws<ArgumentNullException>(() =>
            {
                menu.AddItem("Pizza", "Margherita Pizza", "", null);
            });
        }
    }
}
