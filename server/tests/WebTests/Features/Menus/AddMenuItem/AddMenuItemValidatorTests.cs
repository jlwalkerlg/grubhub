using System;
using System.Threading.Tasks;
using Shouldly;
using Web.Features.Menus.AddMenuItem;
using Xunit;

namespace WebTests.Features.Menus.AddMenuItem
{
    public class AddMenuItemValidatorTests
    {
        private readonly AddMenuItemValidator validator = new();

        [Fact]
        public async Task Disallows_Empty_Restaurant_Ids()
        {
            var command = new AddMenuItemCommand()
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
            var command = new AddMenuItemCommand()
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
        public async Task Disallows_Invalid_Names(string name)
        {
            var command = new AddMenuItemCommand()
            {
                ItemName = name,
            };

            var result = await validator.Validate(command);

            result.ShouldBeAnError();
            result.Error.Errors.ShouldContainKey(nameof(command.ItemName));
        }

        [Fact]
        public async Task Disallows_Negative_Prices()
        {
            var command = new AddMenuItemCommand()
            {
                Price = -1m,
            };

            var result = await validator.Validate(command);

            result.ShouldBeAnError();
            result.Error.Errors.ShouldContainKey(nameof(command.Price));
        }
    }
}
