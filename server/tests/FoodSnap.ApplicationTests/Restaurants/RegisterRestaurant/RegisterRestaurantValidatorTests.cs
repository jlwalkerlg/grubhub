using System;
using System.Threading.Tasks;
using FoodSnap.Application.Restaurants.RegisterRestaurant;
using FoodSnap.Application.Validation;
using FoodSnap.Application.Validation.Failures;
using FoodSnap.ApplicationTests.Users;
using FoodSnap.Domain;
using FoodSnap.Domain.Users;
using Xunit;

namespace FoodSnap.ApplicationTests.Restaurants.RegisterRestaurant
{
    public class RegisterRestaurantValidatorTests
    {
        private readonly RestaurantManagerRepositorySpy restaurantManagerRepositorySpy;
        private readonly RegisterRestaurantValidator validator;

        public RegisterRestaurantValidatorTests()
        {
            restaurantManagerRepositorySpy = new RestaurantManagerRepositorySpy();

            validator = new RegisterRestaurantValidator(restaurantManagerRepositorySpy);
        }

        [Theory]
        [InlineData(null, typeof(RequiredFailure))]
        [InlineData("", typeof(RequiredFailure))]
        [InlineData(" ", typeof(RequiredFailure))]
        public async Task Disallows_Invalid_Manager_Names(string name, Type failureType)
        {
            var command = new RegisterRestaurantCommandBuilder()
                .SetManagerName(name)
                .Build();

            var result = await validator.Validate(command);

            Assert.False(result.IsSuccess);

            var error = result.Error as ValidationError;
            var failures = error.Failures;

            Assert.IsType(failureType, failures[nameof(command.ManagerName)]);
        }

        [Theory]
        [InlineData(null, typeof(RequiredFailure))]
        [InlineData("", typeof(RequiredFailure))]
        [InlineData(" ", typeof(RequiredFailure))]
        [InlineData("blahblahblah", typeof(EmailFailure))]
        public async Task Disallows_Invalid_Manager_Emails(string email, Type failureType)
        {
            var command = new RegisterRestaurantCommandBuilder()
                .SetManagerEmail(email)
                .Build();

            var result = await validator.Validate(command);

            Assert.False(result.IsSuccess);

            var error = result.Error as ValidationError;
            var failures = error.Failures;

            Assert.IsType(failureType, failures[nameof(command.ManagerEmail)]);
        }

        [Fact]
        public async Task Disallows_Emails_That_Are_Already_Taken()
        {
            var manager = new RestaurantManager(
                "Mr Wong",
                new Email("wong@test.com"),
                "password123",
                Guid.NewGuid());

            await restaurantManagerRepositorySpy.Add(manager);

            var command = new RegisterRestaurantCommandBuilder()
                .SetManagerEmail(manager.Email.Address)
                .Build();

            var result = await validator.Validate(command);

            Assert.False(result.IsSuccess);

            var error = result.Error as ValidationError;
            var failures = error.Failures;

            Assert.IsType<EmailTakenFailure>(failures[nameof(command.ManagerEmail)]);
        }

        [Theory]
        [InlineData(null, typeof(RequiredFailure))]
        [InlineData("", typeof(RequiredFailure))]
        [InlineData(" ", typeof(RequiredFailure))]
        [InlineData("1234567", typeof(MinLengthFailure))]
        public async Task Disallows_Invalid_Manager_Passwords(string password, Type failureType)
        {
            var command = new RegisterRestaurantCommandBuilder()
                .SetManagerPassword(password)
                .Build();

            var result = await validator.Validate(command);

            Assert.False(result.IsSuccess);

            var error = result.Error as ValidationError;
            var failures = error.Failures;

            Assert.IsType(failureType, failures[nameof(command.ManagerPassword)]);
        }

        [Theory]
        [InlineData(null, typeof(RequiredFailure))]
        [InlineData("", typeof(RequiredFailure))]
        [InlineData(" ", typeof(RequiredFailure))]
        public async Task Disallows_Invalid_Restaurant_Names(string name, Type failureType)
        {
            var command = new RegisterRestaurantCommandBuilder()
                .SetRestaurantName(name)
                .Build();

            var result = await validator.Validate(command);

            Assert.False(result.IsSuccess);

            var error = result.Error as ValidationError;
            var failures = error.Failures;

            Assert.IsType(failureType, failures[nameof(command.RestaurantName)]);
        }

        [Theory]
        [InlineData(null, typeof(RequiredFailure))]
        [InlineData("", typeof(RequiredFailure))]
        [InlineData(" ", typeof(RequiredFailure))]
        [InlineData("1352314", typeof(PhoneNumberFailure))]
        public async Task Disallows_Invalid_Restaurant_Phone_Numbers(string number, Type failureType)
        {
            var command = new RegisterRestaurantCommandBuilder()
                .SetRestaurantPhoneNumber(number)
                .Build();

            var result = await validator.Validate(command);

            Assert.False(result.IsSuccess);

            var error = result.Error as ValidationError;
            var failures = error.Failures;

            Assert.IsType(failureType, failures[nameof(command.RestaurantPhoneNumber)]);
        }
    }
}
