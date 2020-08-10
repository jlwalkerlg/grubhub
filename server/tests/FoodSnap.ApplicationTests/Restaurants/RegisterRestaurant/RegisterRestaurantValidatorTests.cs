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
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Disallows_Empty_Names(string name)
        {
            var command = new RegisterRestaurantCommandBuilder()
                .SetManagerName(name)
                .Build();

            var result = validator.Validate(command);

            Assert.False(result.IsSuccess);

            var error = result.Error as ValidationError;
            var errors = error.Errors;

            Assert.True(errors.ContainsKey(nameof(command.ManagerName)));
        }
    }
}
