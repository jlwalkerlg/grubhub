using System;
using System.Threading.Tasks;
using Shouldly;
using Web.Features.Menus.RemoveMenuItem;
using Xunit;

namespace WebTests.Features.Menus.RemoveMenuItem
{
    public class RemoveMenuItemValidatorTests
    {
        private readonly RemoveMenuItemValidator validator = new();

        [Fact]
        public async Task Disallows_Empty_Restaurant_Ids()
        {
            var command = new RemoveMenuItemCommand()
            {
                RestaurantId = Guid.Empty,
            };

            var result = await validator.Validate(command);

            result.ShouldBeAnError();
            result.Error.Errors.ShouldContainKey(nameof(command.RestaurantId));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task Disallows_Invalid_Categories(string category)
        {
            var command = new RemoveMenuItemCommand()
            {
                CategoryName = category,
            };

            var result = await validator.Validate(command);

            result.ShouldBeAnError();
            result.Error.Errors.ShouldContainKey(nameof(command.CategoryName));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task Disallows_Invalid_Items(string item)
        {
            var command = new RemoveMenuItemCommand()
            {
                ItemName = item,
            };

            var result = await validator.Validate(command);

            result.ShouldBeAnError();
            result.Error.Errors.ShouldContainKey(nameof(command.ItemName));
        }
    }
}
