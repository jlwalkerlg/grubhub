using System;
using System.Threading.Tasks;
using FoodSnap.Application.Menus.RenameMenuCategory;
using Xunit;

namespace FoodSnap.ApplicationTests.Menus.RenameMenuCategory
{
    public class RenameMenuCategoryValidatorTests
    {
        private readonly RenameMenuCategoryValidator validator;

        public RenameMenuCategoryValidatorTests()
        {
            validator = new RenameMenuCategoryValidator();
        }

        [Fact]
        public async Task RestaurantId_Cant_Be_Empty()
        {
            var command = new RenameMenuCategoryCommand
            {
                RestaurantId = Guid.Empty,
                OldName = "Pizza",
                NewName = "Curry",
            };

            var result = await validator.Validate(command);

            Assert.False(result.IsSuccess);
            Assert.True(result.Error.Errors.ContainsKey(nameof(command.RestaurantId)));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task OldName_Cant_Be_Empty(string oldName)
        {
            var command = new RenameMenuCategoryCommand
            {
                RestaurantId = Guid.NewGuid(),
                OldName = oldName,
                NewName = "Curry",
            };

            var result = await validator.Validate(command);

            Assert.False(result.IsSuccess);
            Assert.True(result.Error.Errors.ContainsKey(nameof(command.OldName)));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task NewName_Cant_Be_Empty(string newName)
        {
            var command = new RenameMenuCategoryCommand
            {
                RestaurantId = Guid.NewGuid(),
                OldName = "Pizza",
                NewName = newName,
            };

            var result = await validator.Validate(command);

            Assert.False(result.IsSuccess);
            Assert.True(result.Error.Errors.ContainsKey(nameof(command.NewName)));
        }
    }
}
