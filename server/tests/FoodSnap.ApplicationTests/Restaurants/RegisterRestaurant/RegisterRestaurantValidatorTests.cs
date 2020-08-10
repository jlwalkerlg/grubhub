using System;
using FoodSnap.Application.Restaurants.RegisterRestaurant;
using FoodSnap.Application.Validation;
using Xunit;

namespace FoodSnap.ApplicationTests.Restaurants.RegisterRestaurant
{
    public class RegisterRestaurantValidatorTests
    {
        private readonly RegisterRestaurantValidator validator;

        public RegisterRestaurantValidatorTests()
        {
            validator = new RegisterRestaurantValidator();
        }

        [Theory]
        [InlineData(null, typeof(RequiredFailure))]
        [InlineData("", typeof(RequiredFailure))]
        [InlineData(" ", typeof(RequiredFailure))]
        public void Disallows_Invalid_Manager_Names(string name, Type failureType)
        {
            var command = new RegisterRestaurantCommandBuilder()
                .SetManagerName(name)
                .Build();

            var result = validator.Validate(command);

            Assert.False(result.IsSuccess);

            var error = result.Error as ValidationError;
            var errors = error.Errors;

            Assert.IsType(failureType, errors[nameof(command.ManagerName)]);
        }

        [Theory]
        [InlineData(null, typeof(RequiredFailure))]
        [InlineData("", typeof(RequiredFailure))]
        [InlineData(" ", typeof(RequiredFailure))]
        [InlineData("blahblahblah", typeof(InvaildEmailFailure))]
        public void Disallows_Invalid_Manager_Emails(string email, Type failureType)
        {
            var command = new RegisterRestaurantCommandBuilder()
                .SetManagerEmail(email)
                .Build();

            var result = validator.Validate(command);

            Assert.False(result.IsSuccess);

            var error = result.Error as ValidationError;
            var errors = error.Errors;

            Assert.IsType(failureType, errors[nameof(command.ManagerEmail)]);
        }

        [Theory]
        [InlineData(null, typeof(RequiredFailure))]
        [InlineData("", typeof(RequiredFailure))]
        [InlineData(" ", typeof(RequiredFailure))]
        [InlineData("1234567", typeof(MinLengthFailure))]
        public void Disallows_Invalid_Manager_Passwords(string password, Type failureType)
        {
            var command = new RegisterRestaurantCommandBuilder()
                .SetManagerPassword(password)
                .Build();

            var result = validator.Validate(command);

            Assert.False(result.IsSuccess);

            var error = result.Error as ValidationError;
            var errors = error.Errors;

            Assert.IsType(failureType, errors[nameof(command.ManagerPassword)]);
        }

        [Theory]
        [InlineData(null, typeof(RequiredFailure))]
        [InlineData("", typeof(RequiredFailure))]
        [InlineData(" ", typeof(RequiredFailure))]
        public void Disallows_Invalid_Restaurant_Names(string name, Type failureType)
        {
            var command = new RegisterRestaurantCommandBuilder()
                .SetRestaurantName(name)
                .Build();

            var result = validator.Validate(command);

            Assert.False(result.IsSuccess);

            var error = result.Error as ValidationError;
            var errors = error.Errors;

            Assert.IsType(failureType, errors[nameof(command.RestaurantName)]);
        }

        [Theory]
        [InlineData(null, typeof(RequiredFailure))]
        [InlineData("", typeof(RequiredFailure))]
        [InlineData(" ", typeof(RequiredFailure))]
        [InlineData("1352314", typeof(PhoneNumberFailure))]
        public void Disallows_Invalid_Restaurant_Phone_Numbers(string number, Type failureType)
        {
            var command = new RegisterRestaurantCommandBuilder()
                .SetRestaurantPhoneNumber(number)
                .Build();

            var result = validator.Validate(command);

            Assert.False(result.IsSuccess);

            var error = result.Error as ValidationError;
            var errors = error.Errors;

            Assert.IsType(failureType, errors[nameof(command.RestaurantPhoneNumber)]);
        }
    }
}
