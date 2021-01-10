using System;
using System.Threading.Tasks;
using Shouldly;
using Web.Features.Menus.UpdateMenuItem;
using Xunit;

namespace WebTests.Features.Menus.UpdateMenuItem
{
    public class UpdateMenuItemValidatorTests
    {
        private readonly UpdateMenuItemValidator validator = new();

        [Fact]
        public async Task Disallows_Empty_Restaurant_Ids()
        {
            var restaurantId = Guid.Empty;

            var command = new UpdateMenuItemCommand()
            {
                RestaurantId = restaurantId,
            };

            var result = await validator.Validate(command);

            result.ShouldBeAnError();
            result.Errors.ShouldContainKey(nameof(command.RestaurantId));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task Disallows_Invalid_Categories(string category)
        {
            var command = new UpdateMenuItemCommand()
            {
                CategoryName = category,
            };

            var result = await validator.Validate(command);

            result.ShouldBeAnError();
            result.Errors.ShouldContainKey(nameof(command.RestaurantId));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task Disallows_Invalid_Items(string item)
        {
            var command = new UpdateMenuItemCommand()
            {
                OldItemName = item,
            };

            var result = await validator.Validate(command);

            result.ShouldBeAnError();
            result.Errors.ShouldContainKey(nameof(command.OldItemName));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task Disallows_Invalid_Names(string name)
        {
            var command = new UpdateMenuItemCommand()
            {
                NewItemName = name,
            };

            var result = await validator.Validate(command);

            result.ShouldBeAnError();
            result.Errors.ShouldContainKey(nameof(command.NewItemName));
        }

        [Fact]
        public async Task Disallows_Negative_Prices()
        {
            var command = new UpdateMenuItemCommand()
            {
                Price = -1m,
            };

            var result = await validator.Validate(command);

            result.ShouldBeAnError();
            result.Errors.ShouldContainKey(nameof(command.Price));
        }
    }
}
