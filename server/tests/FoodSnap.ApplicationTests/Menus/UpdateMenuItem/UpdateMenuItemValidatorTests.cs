using System;
using System.Threading.Tasks;
using FoodSnap.Application.Menus.UpdateMenuItem;
using Xunit;

namespace FoodSnap.ApplicationTests.Menus.UpdateMenuItem
{
    public class UpdateMenuItemValidatorTests
    {
        private readonly UpdateMenuItemValidator validator;

        public UpdateMenuItemValidatorTests()
        {
            validator = new UpdateMenuItemValidator();
        }

        [Fact]
        public async Task Disallows_Empty_Menu_Ids()
        {
            var menuId = Guid.Empty;
            var category = "Pizza";
            var item = "Margherita";
            var name = "Margherita";
            var description = "Cheese & tomato";
            var price = 9.99m;

            var command = new UpdateMenuItemCommand
            {
                MenuId = menuId,
                CategoryName = category,
                OldItemName = item,
                NewItemName = name,
                Description = description,
                Price = price,
            };

            var result = await validator.Validate(command);

            Assert.False(result.IsSuccess);
            Assert.True(result.Error.Errors.ContainsKey(nameof(command.MenuId)));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task Disallows_Invalid_Categories(string category)
        {
            var menuId = Guid.NewGuid();
            var item = "Margherita";
            var name = "Margherita";
            var description = "Cheese & tomato";
            var price = 9.99m;

            var command = new UpdateMenuItemCommand
            {
                MenuId = menuId,
                CategoryName = category,
                OldItemName = item,
                NewItemName = name,
                Description = description,
                Price = price,
            };

            var result = await validator.Validate(command);

            Assert.False(result.IsSuccess);
            Assert.True(result.Error.Errors.ContainsKey(nameof(command.CategoryName)));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task Disallows_Invalid_Items(string item)
        {
            var menuId = Guid.NewGuid();
            var category = "Pizza";
            var name = "Margherita";
            var description = "Cheese & tomato";
            var price = 9.99m;

            var command = new UpdateMenuItemCommand
            {
                MenuId = menuId,
                CategoryName = category,
                OldItemName = item,
                NewItemName = name,
                Description = description,
                Price = price,
            };

            var result = await validator.Validate(command);

            Assert.False(result.IsSuccess);
            Assert.True(result.Error.Errors.ContainsKey(nameof(command.OldItemName)));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task Disallows_Invalid_Names(string name)
        {
            var menuId = Guid.NewGuid();
            var category = "Pizza";
            var item = "Margherita";
            var description = "Cheese & tomato";
            var price = 9.99m;

            var command = new UpdateMenuItemCommand
            {
                MenuId = menuId,
                CategoryName = category,
                OldItemName = item,
                NewItemName = name,
                Description = description,
                Price = price,
            };

            var result = await validator.Validate(command);

            Assert.False(result.IsSuccess);
            Assert.True(result.Error.Errors.ContainsKey(nameof(command.NewItemName)));
        }

        [Fact]
        public async Task Disallows_Negative_Prices()
        {
            var menuId = Guid.NewGuid();
            var category = "Pizza";
            var item = "Margherita";
            var name = "Margherita";
            var description = "Cheese & tomato";
            var price = -1m;

            var command = new UpdateMenuItemCommand
            {
                MenuId = menuId,
                CategoryName = category,
                OldItemName = item,
                NewItemName = name,
                Description = description,
                Price = price,
            };

            var result = await validator.Validate(command);

            Assert.False(result.IsSuccess);
            Assert.True(result.Error.Errors.ContainsKey(nameof(command.Price)));
        }
    }
}
