using Shouldly;
using System;
using System.Threading.Tasks;
using Web.Features.Menus.AddMenuItem;
using Xunit;

namespace WebTests.Features.Menus.AddMenuItem
{
    public class AddMenuItemValidatorTests
    {
        private readonly AddMenuItemValidator validator = new();

        [Fact]
        public async Task Disallows_Empty_RestaurantId()
        {
            var command = new AddMenuItemCommand()
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
            var command = new AddMenuItemCommand()
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
        public async Task Disallows_Invalid_Names(string name)
        {
            var command = new AddMenuItemCommand()
            {
                Name = name,
            };

            var result = await validator.Validate(command);

            result.ShouldBeAnError();
            result.Errors.ShouldContainKey(nameof(command.Name));
        }

        [Fact]
        public async Task Disallows_Description_That_Are_Too_Long()
        {
            var command = new AddMenuItemCommand()
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
            var command = new AddMenuItemCommand()
            {
                Price = -1m,
            };

            var result = await validator.Validate(command);

            result.ShouldBeAnError();
            result.Errors.ShouldContainKey(nameof(command.Price));
        }
    }
}
