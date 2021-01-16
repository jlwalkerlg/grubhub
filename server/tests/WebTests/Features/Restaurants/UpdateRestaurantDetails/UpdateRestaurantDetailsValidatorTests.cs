using Shouldly;
using System;
using System.Threading.Tasks;
using Web.Features.Restaurants.UpdateRestaurantDetails;
using Xunit;

namespace WebTests.Features.Restaurants.UpdateRestaurantDetails
{
    public class UpdateRestaurantDetailsValidatorTests
    {
        private readonly UpdateRestaurantDetailsValidator validator = new();

        [Fact]
        public async Task Disallows_Empty_Ids()
        {
            var command = new UpdateRestaurantDetailsCommand()
            {
                RestaurantId = Guid.Empty,
            };

            var result = await validator.Validate(command);

            result.ShouldBeAnError();
            result.Errors.ShouldContainKey(nameof(command.RestaurantId));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task Disallows_Invalid_Names(string name)
        {
            var command = new UpdateRestaurantDetailsCommand()
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
            var command = new UpdateRestaurantDetailsCommand()
            {
                Description = new string('c', 401),
            };

            var result = await validator.Validate(command);

            result.ShouldBeAnError();
            result.Errors.ShouldContainKey(nameof(command.Description));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("1352314")]
        public async Task Disallows_Invalid_Phone_Numbers(string number)
        {
            var command = new UpdateRestaurantDetailsCommand()
            {
                PhoneNumber = number,
            };

            var result = await validator.Validate(command);

            result.ShouldBeAnError();
            result.Errors.ShouldContainKey(nameof(command.PhoneNumber));
        }

        [Theory]
        [InlineData(-1)]
        public async Task Disallows_Invalid_Delivery_Fees(decimal fee)
        {
            var command = new UpdateRestaurantDetailsCommand()
            {
                DeliveryFee = fee,
            };

            var result = await validator.Validate(command);

            result.ShouldBeAnError();
            result.Errors.ShouldContainKey(nameof(command.DeliveryFee));
        }

        [Theory]
        [InlineData(-1)]
        public async Task Disallows_Invalid_Minimum_Delivery_Spends(decimal spend)
        {
            var command = new UpdateRestaurantDetailsCommand()
            {
                MinimumDeliverySpend = spend,
            };

            var result = await validator.Validate(command);

            result.ShouldBeAnError();
            result.Errors.ShouldContainKey(nameof(command.MinimumDeliverySpend));
        }

        [Theory]
        [InlineData(-1)]
        public async Task Disallows_Invalid_Max_Delivery_Distance_In_Km(int distance)
        {
            var command = new UpdateRestaurantDetailsCommand()
            {
                MaxDeliveryDistanceInKm = distance,
            };

            var result = await validator.Validate(command);

            result.ShouldBeAnError();
            result.Errors.ShouldContainKey(nameof(command.MaxDeliveryDistanceInKm));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(4)]
        public async Task Disallows_Invalid_Estimated_Delivery_Times(int time)
        {
            var command = new UpdateRestaurantDetailsCommand()
            {
                EstimatedDeliveryTimeInMinutes = time,
            };

            var result = await validator.Validate(command);

            result.ShouldBeAnError();
            result.Errors.ShouldContainKey(nameof(command.EstimatedDeliveryTimeInMinutes));
        }
    }
}
