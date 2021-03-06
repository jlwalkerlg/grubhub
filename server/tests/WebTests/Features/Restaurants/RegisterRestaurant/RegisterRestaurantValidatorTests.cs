using Shouldly;
using System.Threading.Tasks;
using Web.Features.Restaurants.RegisterRestaurant;
using Xunit;

namespace WebTests.Features.Restaurants.RegisterRestaurant
{
    public class RegisterRestaurantValidatorTests
    {
        private readonly RegisterRestaurantValidator validator = new();

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task Disallows_Invalid_Manager_Names(string name)
        {
            var command = new RegisterRestaurantCommand()
            {
                ManagerName = name,
            };

            var result = await validator.Validate(command);

            result.ShouldBeAnError();
            result.Errors.ShouldContainKey(nameof(command.ManagerName));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("blahblahblah")]
        public async Task Disallows_Invalid_Manager_Emails(string email)
        {
            var command = new RegisterRestaurantCommand()
            {
                ManagerEmail = email,
            };

            var result = await validator.Validate(command);

            result.ShouldBeAnError();
            result.Errors.ShouldContainKey(nameof(command.ManagerEmail));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("1234567")]
        public async Task Disallows_Invalid_Manager_Passwords(string password)
        {
            var command = new RegisterRestaurantCommand()
            {
                ManagerPassword = password,
            };

            var result = await validator.Validate(command);

            result.ShouldBeAnError();
            result.Errors.ShouldContainKey(nameof(command.ManagerPassword));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task Disallows_Invalid_Restaurant_Names(string name)
        {
            var command = new RegisterRestaurantCommand()
            {
                RestaurantName = name,
            };

            var result = await validator.Validate(command);

            result.ShouldBeAnError();
            result.Errors.ShouldContainKey(nameof(command.RestaurantName));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("1352314")]
        public async Task Disallows_Invalid_Restaurant_Phone_Numbers(string number)
        {
            var command = new RegisterRestaurantCommand()
            {
                RestaurantPhoneNumber = number,
            };

            var result = await validator.Validate(command);

            result.ShouldBeAnError();
            result.Errors.ShouldContainKey(nameof(command.RestaurantPhoneNumber));
        }

        [Theory]
        [InlineData(null, null, null)]
        [InlineData("", "", "")]
        [InlineData(" ", " ", " ")]
        public async Task Disallows_Invalid_Restaurant_Addresses(string line1, string city, string postcode)
        {
            var command = new RegisterRestaurantCommand()
            {
                AddressLine1 = line1,
                AddressLine2 = null,
                City = city,
                Postcode = postcode,
            };

            var result = await validator.Validate(command);

            result.ShouldBeAnError();
            result.Errors.ShouldContainKey(nameof(command.AddressLine1));
            result.Errors.ShouldContainKey(nameof(command.City));
            result.Errors.ShouldContainKey(nameof(command.Postcode));
        }
    }
}
