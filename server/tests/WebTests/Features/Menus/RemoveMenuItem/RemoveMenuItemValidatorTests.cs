using Shouldly;
using System;
using System.Threading.Tasks;
using Web.Features.Menus.RemoveMenuItem;
using Xunit;

namespace WebTests.Features.Menus.RemoveMenuItem
{
    public class RemoveMenuItemValidatorTests
    {
        private readonly RemoveMenuItemValidator validator = new();

        [Fact]
        public async Task Disallows_Empty_RestaurantId()
        {
            var command = new RemoveMenuItemCommand()
            {
                RestaurantId = Guid.Empty,
            };

            var result = await validator.Validate(command);

            result.ShouldBeAnError();
            result.Errors.ShouldContainKey(nameof(command.RestaurantId));
        }

        [Fact]
        public async Task Disallows_Empty_CategoryId()
        {
            var command = new RemoveMenuItemCommand()
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
            var command = new RemoveMenuItemCommand()
            {
                ItemId = Guid.Empty,
            };

            var result = await validator.Validate(command);

            result.ShouldBeAnError();
            result.Errors.ShouldContainKey(nameof(command.ItemId));
        }
    }
}
