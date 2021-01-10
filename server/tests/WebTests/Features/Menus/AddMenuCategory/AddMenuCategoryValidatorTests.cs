using System;
using System.Threading.Tasks;
using Shouldly;
using Web.Features.Menus.AddMenuCategory;
using Xunit;

namespace WebTests.Features.Menus.AddMenuCategory
{
    public class AddMenuCategoryValidatorTests
    {
        private readonly AddMenuCategoryValidator validator = new();

        [Fact]
        public async Task Disallows_Empty_Restaurant_Ids()
        {
            var command = new AddMenuCategoryCommand()
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
        public async Task Disallows_Invalid_Names(string name)
        {
            var command = new AddMenuCategoryCommand()
            {
                Name = name,
            };

            var result = await validator.Validate(command);

            result.ShouldBeAnError();
            result.Errors.ShouldContainKey(nameof(command.Name));
        }
    }
}
