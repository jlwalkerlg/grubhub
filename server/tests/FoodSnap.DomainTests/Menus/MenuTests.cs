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

        [Fact]
        public void It_Cant_Add_Two_Categories_With_The_Same_Name()
        {
            var menu = new Menu(new MenuId(Guid.NewGuid()), new RestaurantId(Guid.NewGuid()));
            menu.AddCategory("Pizza");

            Assert.Throws<InvalidOperationException>(() =>
            {
                menu.AddCategory("Pizza");
            });
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
        public void It_Adds_An_Item_To_A_Category()
        {
            var menu = new Menu(new MenuId(Guid.NewGuid()), new RestaurantId(Guid.NewGuid()));
            menu.AddCategory("Pizza");

            var category = menu.Categories.Single();

            category.AddItem("Margherita", "Cheese & tomato", new Money(9.99m));

            Assert.Single(category.Items);

            var item = category.Items.Single();
            Assert.Equal("Margherita", item.Name);
            Assert.Equal("Cheese & tomato", item.Description);
            Assert.Equal(new Money(9.99m), item.Price);
        }

        [Fact]
        public void It_Can_Rename_An_Item()
        {
            var menu = new Menu(new MenuId(Guid.NewGuid()), new RestaurantId(Guid.NewGuid()));
            menu.AddCategory("Pizza");

            var category = menu.Categories.Single();

            category.AddItem("Margherita", "Ham & pinapple", new Money(11.99m));
            category.RenameItem("Margherita", "Hawaiian");

            Assert.Single(category.Items);

            var item = category.Items.Single();
            Assert.Equal("Hawaiian", item.Name);
        }

        [Fact]
        public void Renaming_An_Item_With_The_Same_Name_Is_Ay_Ok()
        {
            var menu = new Menu(new MenuId(Guid.NewGuid()), new RestaurantId(Guid.NewGuid()));
            menu.AddCategory("Pizza");

            var category = menu.Categories.Single();
            category.AddItem("Hawaiian", "Ham & pinapple", new Money(11.99m));

            category.RenameItem("Hawaiian", "Hawaiian");

            var item = category.Items.Single();
            Assert.Equal("Hawaiian", item.Name);
        }

        [Fact]
        public void It_Cant_Rename_An_Item_If_An_Item_With_That_Name_Already_Exists()
        {
            var menu = new Menu(new MenuId(Guid.NewGuid()), new RestaurantId(Guid.NewGuid()));
            menu.AddCategory("Pizza");

            var category = menu.Categories.Single();
            category.AddItem("Margherita", "Cheese & tomato", new Money(9.99m));

            category.AddItem("Hawaiian", "Ham & pinapple", new Money(11.99m));

            Assert.Throws<InvalidOperationException>(() =>
            {
                category.RenameItem("Hawaiian", "Margherita");
            });
        }

        [Fact]
        public void It_Cant_Add_Two_Items_With_The_Same_Name_To_The_Same_Category()
        {
            var menu = new Menu(new MenuId(Guid.NewGuid()), new RestaurantId(Guid.NewGuid()));
            menu.AddCategory("Pizza");

            var category = menu.Categories.Single();

            category.AddItem("Margherita", "Cheese & tomato", new Money(9.99m));

            Assert.Throws<InvalidOperationException>(() =>
            {
                category.AddItem("Margherita", "Cheese & tomato", new Money(9.99m));
            });
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Item_Name_Cant_Be_Empty(string name)
        {
            var menu = new Menu(new MenuId(Guid.NewGuid()), new RestaurantId(Guid.NewGuid()));
            menu.AddCategory("Pizza");

            var category = menu.Categories.Single();

            Assert.Throws<ArgumentException>(() =>
            {
                category.AddItem(name, "", new Money(10));
            });

            category.AddItem("Margherita", "Cheese & tomato", new Money(9.99m));

            Assert.Throws<ArgumentException>(() =>
            {
                category.RenameItem("Margherita", name);
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
                category.AddItem("Margherita", "", null);
            });

            category.AddItem("Margherita", "Cheese & tomato", new Money(9.99m));
            var item = category.Items.Single();

            Assert.Throws<ArgumentNullException>(() =>
            {
                item.Price = null;
            });
        }

        [Fact]
        public void It_Can_Remove_An_Item()
        {
            var menu = new Menu(new MenuId(Guid.NewGuid()), new RestaurantId(Guid.NewGuid()));
            menu.AddCategory("Pizza");

            var category = menu.Categories.Single();
            category.AddItem("Margherita", "Cheese & tomato", new Money(9.99m));

            category.RemoveItem("Margherita");

            Assert.Empty(category.Items);
        }

        [Fact]
        public void It_Cant_Remove_An_Item_That_Doesnt_Exist()
        {
            var menu = new Menu(new MenuId(Guid.NewGuid()), new RestaurantId(Guid.NewGuid()));
            menu.AddCategory("Pizza");

            var category = menu.Categories.Single();

            Assert.Throws<InvalidOperationException>(() =>
            {
                category.RemoveItem("Margherita");
            });
        }
    }
}
