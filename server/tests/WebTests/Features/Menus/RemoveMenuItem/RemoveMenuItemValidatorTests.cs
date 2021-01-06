using System;
using System.Threading.Tasks;
using Web.Features.Menus.RemoveMenuItem;
using Xunit;

namespace WebTests.Features.Menus.RemoveMenuItem
{
    public class RemoveMenuItemValidatorTests
    {
        private readonly RemoveMenuItemValidator validator;

        public RemoveMenuItemValidatorTests()
        {
            validator = new RemoveMenuItemValidator();
        }

        [Fact]
        public async Task Disallows_Empty_Restaurant_Ids()
        {
            var restaurantId = Guid.Empty;
            var category = "Pizza";
            var item = "Margherita";

            var command = new RemoveMenuItemCommand
            {
                RestaurantId = restaurantId,
                CategoryName = category,
                ItemName = item,
            };

            var result = await validator.Validate(command);

            Assert.False(result);
            Assert.True(result.Error.Errors.ContainsKey(nameof(command.RestaurantId)));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task Disallows_Invalid_Categories(string category)
        {
            var restaurantId = Guid.NewGuid();
            var item = "Margherita";

            var command = new RemoveMenuItemCommand
            {
                RestaurantId = restaurantId,
                CategoryName = category,
                ItemName = item,
            };

            var result = await validator.Validate(command);

            Assert.False(result);
            Assert.True(result.Error.Errors.ContainsKey(nameof(command.CategoryName)));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task Disallows_Invalid_Items(string item)
        {
            var restaurantId = Guid.NewGuid();
            var category = "Pizza";

            var command = new RemoveMenuItemCommand
            {
                RestaurantId = restaurantId,
                CategoryName = category,
                ItemName = item,
            };

            var result = await validator.Validate(command);

            Assert.False(result);
            Assert.True(result.Error.Errors.ContainsKey(nameof(command.ItemName)));
        }
    }
}
