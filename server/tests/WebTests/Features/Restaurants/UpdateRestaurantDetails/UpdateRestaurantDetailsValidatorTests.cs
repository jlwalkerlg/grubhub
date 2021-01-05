using System;
using System.Threading.Tasks;
using Web.Features.Restaurants.UpdateRestaurantDetails;
using Xunit;

namespace WebTests.Features.Restaurants.UpdateRestaurantDetails
{
    public class UpdateRestaurantDetailsValidatorTests
    {
        private readonly UpdateRestaurantDetailsValidator validator;
        private readonly UpdateRestaurantDetailsCommand validCommand;

        public UpdateRestaurantDetailsValidatorTests()
        {
            validator = new UpdateRestaurantDetailsValidator();

            validCommand = new UpdateRestaurantDetailsCommand
            {
                RestaurantId = Guid.NewGuid(),
                Name = "Chow Main",
                PhoneNumber = "01234567890",
                MinimumDeliverySpend = 0,
                DeliveryFee = 0,
                MaxDeliveryDistanceInKm = 10,
                EstimatedDeliveryTimeInMinutes = 5,
            };
        }

        [Fact]
        public async Task It_Passes()
        {
            var result = await validator.Validate(validCommand);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task Disallows_Empty_Ids()
        {
            var command = validCommand with
            {
                RestaurantId = Guid.Empty,
            };

            var result = await validator.Validate(command);

            Assert.False(result.IsSuccess);
            Assert.True(result.Error.Errors.ContainsKey(nameof(command.RestaurantId)));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task Disallows_Invalid_Names(string name)
        {
            var command = validCommand with
            {
                Name = name,
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
            var command = validCommand with
            {
                PhoneNumber = number,
            };

            var result = await validator.Validate(command);

            Assert.False(result.IsSuccess);
            Assert.True(result.Error.Errors.ContainsKey(nameof(command.PhoneNumber)));
        }

        [Theory]
        [InlineData(-1)]
        public async Task Disallows_Invalid_Delivery_Fees(decimal fee)
        {
            var command = validCommand with
            {
                DeliveryFee = fee,
            };

            var result = await validator.Validate(command);

            Assert.False(result.IsSuccess);
            Assert.True(result.Error.Errors.ContainsKey(nameof(command.DeliveryFee)));
        }

        [Theory]
        [InlineData(-1)]
        public async Task Disallows_Invalid_Minimum_Delivery_Spends(decimal spend)
        {
            var command = validCommand with
            {
                MinimumDeliverySpend = spend,
            };

            var result = await validator.Validate(command);

            Assert.False(result.IsSuccess);
            Assert.True(result.Error.Errors.ContainsKey(nameof(command.MinimumDeliverySpend)));
        }

        [Theory]
        [InlineData(-1)]
        public async Task Disallows_Invalid_Max_Delivery_Distance_In_Km(int distance)
        {
            var command = validCommand with
            {
                MaxDeliveryDistanceInKm = distance,
            };

            var result = await validator.Validate(command);

            Assert.False(result.IsSuccess);
            Assert.True(result.Error.Errors.ContainsKey(nameof(command.MaxDeliveryDistanceInKm)));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(4)]
        public async Task Disallows_Invalid_Estimated_Delivery_Times(int time)
        {
            var command = validCommand with
            {
                EstimatedDeliveryTimeInMinutes = time,
            };

            var result = await validator.Validate(command);

            Assert.False(result.IsSuccess);
            Assert.True(result.Error.Errors.ContainsKey(nameof(command.EstimatedDeliveryTimeInMinutes)));
        }
    }
}
