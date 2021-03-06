using Shouldly;
using System;
using System.Threading.Tasks;
using Web.Features.Menus.RenameMenuCategory;
using Xunit;

namespace WebTests.Features.Menus.RenameMenuCategory
{
    public class RenameMenuCategoryValidatorTests
    {
        private readonly RenameMenuCategoryValidator validator = new();

        [Fact]
        public async Task RestaurantId_Cant_Be_Empty()
        {
            var command = new RenameMenuCategoryCommand()
            {
                RestaurantId = Guid.Empty,
            };

            var result = await validator.Validate(command);

            result.ShouldBeAnError();
            result.Errors.ShouldContainKey(nameof(command.RestaurantId));
        }

        [Fact]
        public async Task CategoryId_Cant_Be_Empty()
        {
            var command = new RenameMenuCategoryCommand()
            {
                CategoryId = Guid.Empty,
            };

            var result = await validator.Validate(command);

            result.ShouldBeAnError();
            result.Errors.ShouldContainKey(nameof(command.CategoryId));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task NewName_Cant_Be_Empty(string newName)
        {
            var command = new RenameMenuCategoryCommand()
            {
                NewName = newName,
            };

            var result = await validator.Validate(command);

            result.ShouldBeAnError();
            result.Errors.ShouldContainKey(nameof(command.NewName));
        }
    }
}
