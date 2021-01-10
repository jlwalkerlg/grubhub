using System;
using System.Threading.Tasks;
using Shouldly;
using Web.Features.Menus.RemoveMenuCategory;
using Xunit;

namespace WebTests.Features.Menus.RemoveMenuCategory
{
    public class RemoveMenuCategoryValidatorTests
    {
        private readonly RemoveMenuCategoryValidator validator = new();

        [Fact]
        public async Task RestaurantId_Cant_Be_Empty()
        {
            var command = new RemoveMenuCategoryCommand()
            {
                RestaurantId = Guid.Empty,
            };

            var result = await validator.Validate(command);

            result.ShouldBeAnError();
            result.Errors.ShouldContainKey(nameof(command.RestaurantId));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task CategoryName_Cant_Be_Empty(string name)
        {
            var command = new RemoveMenuCategoryCommand()
            {
                CategoryName = name,
            };

            var result = await validator.Validate(command);

            result.ShouldBeAnError();
            result.Errors.ShouldContainKey(nameof(command.CategoryName));
        }
    }
}
