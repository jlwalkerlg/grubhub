using System;
using System.Threading.Tasks;
using Shouldly;
using Web.Features.Restaurants.UpdateOpeningHours;
using Xunit;

namespace WebTests.Features.Restaurants.UpdateOpeningTimes
{
    public class UpdateOpeningHoursValidatorTests
    {
        private readonly UpdateOpeningHoursValidator validator = new();

        [Fact]
        public async Task Restaurant_Id_Is_Required()
        {
            var command = new UpdateOpeningHoursCommand();

            var result = await validator.Validate(command);

            result.ShouldBeAnError();
            result.Errors.ShouldContainKey(nameof(command.RestaurantId));
        }

        [Fact]
        public async Task Times_Are_Not_Required()
        {
            var command = new UpdateOpeningHoursCommand()
            {
                RestaurantId = Guid.NewGuid(),
            };

            var result = await validator.Validate(command);

            result.ShouldBeSuccessful();
        }

        [Fact]
        public async Task Closing_Times_Are_Not_Required_If_The_Opening_Time_Is_Provided()
        {
            var command = new UpdateOpeningHoursCommand()
            {
                RestaurantId = Guid.NewGuid(),
                MondayOpen = "12:00",
            };

            var result = await validator.Validate(command);

            result.ShouldBeSuccessful();
        }

        [Fact]
        public async Task Times_Must_Be_Valid_Format()
        {
            var command = new UpdateOpeningHoursCommand()
            {
                MondayOpen = "invalid",
                MondayClose = "invalid",
            };

            var result = await validator.Validate(command);

            result.ShouldBeAnError();
            result.Errors.ShouldContainKey(nameof(command.MondayOpen));
            result.Errors.ShouldContainKey(nameof(command.MondayClose));
        }

        [Theory]
        [InlineData("44:00")]
        [InlineData("24:00")]
        [InlineData("24:15")]
        public async Task Opening_Times_Must_Be_Within_Range(string time)
        {
            var command = new UpdateOpeningHoursCommand()
            {
                MondayOpen = time,
            };

            var result = await validator.Validate(command);

            result.ShouldBeAnError();
            result.Errors.ShouldContainKey(nameof(command.MondayOpen));
        }

        [Theory]
        [InlineData("00:00")]
        [InlineData("24:00")]
        public async Task Closing_Times_Must_Be_Within_Range(string time)
        {
            var command = new UpdateOpeningHoursCommand()
            {
                MondayOpen = "00:00",
                MondayClose = time,
            };

            var result = await validator.Validate(command);

            result.ShouldBeAnError();
            result.Errors.ShouldContainKey(nameof(command.MondayClose));
        }

        [Theory]
        [InlineData("12:00", "11:00")]
        [InlineData("12:00", "12:00")]
        public async Task Opening_Time_Must_Come_Earlier_Than_Closing_Time(string openingTime, string closingTime)
        {
            var command = new UpdateOpeningHoursCommand()
            {
                MondayOpen = openingTime,
                MondayClose = closingTime,
            };

            var result = await validator.Validate(command);

            result.ShouldBeAnError();
            result.Errors.ShouldNotContainKey(nameof(command.MondayOpen));
            result.Errors.ShouldContainKey(nameof(command.MondayClose));
        }
    }
}
