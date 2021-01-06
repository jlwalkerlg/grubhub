using System;
using System.Threading.Tasks;
using Web.Features.Menus.UpdateMenuItem;
using Xunit;

namespace WebTests.Features.Menus.UpdateMenuItem
{
    public class UpdateMenuItemValidatorTests
    {
        private readonly UpdateMenuItemValidator validator;

        public UpdateMenuItemValidatorTests()
        {
            validator = new UpdateMenuItemValidator();
        }

        [Fact]
        public async Task Disallows_Empty_Restaurant_Ids()
        {
            var restaurantId = Guid.Empty;
            var category = "Pizza";
            var item = "Margherita";
            var name = "Margherita";
            var description = "Cheese & tomato";
            var price = 9.99m;

            var command = new UpdateMenuItemCommand
            {
                RestaurantId = restaurantId,
                CategoryName = category,
                OldItemName = item,
                NewItemName = name,
                Description = description,
                Price = price,
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
            var name = "Margherita";
            var description = "Cheese & tomato";
            var price = 9.99m;

            var command = new UpdateMenuItemCommand
            {
                RestaurantId = restaurantId,
                CategoryName = category,
                OldItemName = item,
                NewItemName = name,
                Description = description,
                Price = price,
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
            var name = "Margherita";
            var description = "Cheese & tomato";
            var price = 9.99m;

            var command = new UpdateMenuItemCommand
            {
                RestaurantId = restaurantId,
                CategoryName = category,
                OldItemName = item,
                NewItemName = name,
                Description = description,
                Price = price,
            };

            var result = await validator.Validate(command);

            Assert.False(result);
            Assert.True(result.Error.Errors.ContainsKey(nameof(command.OldItemName)));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task Disallows_Invalid_Names(string name)
        {
            var restaurantId = Guid.NewGuid();
            var category = "Pizza";
            var item = "Margherita";
            var description = "Cheese & tomato";
            var price = 9.99m;

            var command = new UpdateMenuItemCommand
            {
                RestaurantId = restaurantId,
                CategoryName = category,
                OldItemName = item,
                NewItemName = name,
                Description = description,
                Price = price,
            };

            var result = await validator.Validate(command);

            Assert.False(result);
            Assert.True(result.Error.Errors.ContainsKey(nameof(command.NewItemName)));
        }

        [Fact]
        public async Task Disallows_Negative_Prices()
        {
            var restaurantId = Guid.NewGuid();
            var category = "Pizza";
            var item = "Margherita";
            var name = "Margherita";
            var description = "Cheese & tomato";
            var price = -1m;

            var command = new UpdateMenuItemCommand
            {
                RestaurantId = restaurantId,
                CategoryName = category,
                OldItemName = item,
                NewItemName = name,
                Description = description,
                Price = price,
            };

            var result = await validator.Validate(command);

            Assert.False(result);
            Assert.True(result.Error.Errors.ContainsKey(nameof(command.Price)));
        }
    }
}
