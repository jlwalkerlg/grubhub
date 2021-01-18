using Shouldly;
using System;
using System.Threading.Tasks;
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

        [Fact]
        public async Task Disallows_Empty_CategoryId()
        {
            var command = new UpdateMenuItemCommand()
            {
                CategoryId = Guid.Empty,
            };

            var result = await validator.Validate(command);

            result.ShouldBeAnError();
            result.Errors.ShouldContainKey(nameof(command.CategoryId));
        }

        [Fact]
        public async Task Disallows_Empty_ItemId()
        {
            var command = new UpdateMenuItemCommand()
            {
                ItemId = Guid.Empty,
            };

            var result = await validator.Validate(command);

            result.ShouldBeAnError();
            result.Errors.ShouldContainKey(nameof(command.ItemId));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task Disallows_Invalid_Names(string name)
        {
            var command = new UpdateMenuItemCommand()
            {
                Name = name,
            };

            var result = await validator.Validate(command);

            result.ShouldBeAnError();
            result.Errors.ShouldContainKey(nameof(command.Name));
        }

        [Fact]
        public async Task Disallows_Descriptions_That_Are_Too_Long()
        {
            var command = new UpdateMenuItemCommand()
            {
                Description = new string('c', 281),
            };

            var result = await validator.Validate(command);

            result.ShouldBeAnError();
            result.Errors.ShouldContainKey(nameof(command.Description));
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
