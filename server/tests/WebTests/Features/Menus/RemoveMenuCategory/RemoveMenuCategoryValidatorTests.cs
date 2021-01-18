using Shouldly;
using System;
using System.Threading.Tasks;
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

        [Fact]
        public async Task CategoryId_Cant_Be_Empty()
        {
            var command = new RemoveMenuCategoryCommand()
            {
                CategoryId = Guid.Empty,
            };

            var result = await validator.Validate(command);

            result.ShouldBeAnError();
            result.Errors.ShouldContainKey(nameof(command.CategoryId));
        }
    }
}
