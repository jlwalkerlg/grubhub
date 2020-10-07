using System;
using System.Threading.Tasks;
using FoodSnap.Application.Menus.RemoveMenuItem;
using Xunit;

namespace FoodSnap.ApplicationTests.Menus.RemoveMenuItem
{
    public class RemoveMenuItemValidatorTests
    {
        private readonly RemoveMenuItemValidator validator;

        public RemoveMenuItemValidatorTests()
        {
            validator = new RemoveMenuItemValidator();
        }

        [Fact]
        public async Task Disallows_Empty_Menu_Ids()
        {
            var menuId = Guid.Empty;
            var category = "Pizza";
            var item = "Margherita";

            var command = new RemoveMenuItemCommand
            {
                MenuId = menuId,
                CategoryName = category,
                ItemName = item,
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

            var command = new RemoveMenuItemCommand
            {
                MenuId = menuId,
                CategoryName = category,
                ItemName = item,
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

            var command = new RemoveMenuItemCommand
            {
                MenuId = menuId,
                CategoryName = category,
                ItemName = item,
            };

            var result = await validator.Validate(command);

            Assert.False(result.IsSuccess);
            Assert.True(result.Error.Errors.ContainsKey(nameof(command.ItemName)));
        }
    }
}
