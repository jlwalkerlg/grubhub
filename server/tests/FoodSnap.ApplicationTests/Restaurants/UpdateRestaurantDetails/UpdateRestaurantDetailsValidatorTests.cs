using System;
using System.Threading.Tasks;
using FoodSnap.Application.Restaurants.UpdateRestaurantDetails;
using Xunit;

namespace FoodSnap.ApplicationTests.Restaurants.UpdateRestaurantDetails
{
    public class UpdateRestaurantDetailsValidatorTests
    {
        private readonly UpdateRestaurantDetailsValidator validator;

        public UpdateRestaurantDetailsValidatorTests()
        {
            validator = new UpdateRestaurantDetailsValidator();
        }

        [Fact]
        public async Task Disallows_Empty_Ids()
        {
            var command = new UpdateRestaurantDetailsCommand
            {
                Id = Guid.Empty,
                Name = "Chow Main",
                PhoneNumber = "01234567890",
            };

            var result = await validator.Validate(command);

            Assert.False(result.IsSuccess);
            Assert.True(result.Error.Errors.ContainsKey(nameof(command.Id)));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task Disallows_Invalid_Names(string name)
        {
            var command = new UpdateRestaurantDetailsCommand
            {
                Id = Guid.NewGuid(),
                Name = name,
                PhoneNumber = "01234567890",
            };

            var result = await validator.Validate(command);

            Assert.False(result.IsSuccess);
            Assert.True(result.Error.Errors.ContainsKey(nameof(command.Name)));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("1352314")]
        public async Task Disallows_Invalid_Phone_Numbers(string number)
        {
            var command = new UpdateRestaurantDetailsCommand
            {
                Id = Guid.NewGuid(),
                Name = "Chow Main",
                PhoneNumber = number,
            };

            var result = await validator.Validate(command);

            Assert.False(result.IsSuccess);
            Assert.True(result.Error.Errors.ContainsKey(nameof(command.PhoneNumber)));
        }
    }
}
