using System;
using System.Threading.Tasks;
using FoodSnap.Application.Menus.AddMenuCategory;
using Xunit;

namespace FoodSnap.ApplicationTests.Menus.AddMenuCategory
{
    public class AddMenuCategoryValidatorTests
    {
        private readonly AddMenuCategoryValidator validator;

        public AddMenuCategoryValidatorTests()
        {
            validator = new AddMenuCategoryValidator();
        }

        [Fact]
        public async Task Disallows_Empty_Menu_Ids()
        {
            var menuId = Guid.Empty;
            var name = "Pizza";

            var command = new AddMenuCategoryCommand
            {
                MenuId = menuId,
                Name = name,
            };

            var result = await validator.Validate(command);

            Assert.False(result.IsSuccess);
            Assert.True(result.Error.Errors.ContainsKey(nameof(command.MenuId)));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task Disallows_Invalid_Names(string name)
        {
            var menuId = Guid.NewGuid();

            var command = new AddMenuCategoryCommand
            {
                MenuId = menuId,
                Name = name,
            };

            var result = await validator.Validate(command);

            Assert.False(result.IsSuccess);
            Assert.True(result.Error.Errors.ContainsKey(nameof(command.Name)));
        }
    }
}
