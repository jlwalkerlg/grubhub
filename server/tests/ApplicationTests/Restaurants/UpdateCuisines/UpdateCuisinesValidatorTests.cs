using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Restaurants.UpdateCuisines;
using Xunit;

namespace ApplicationTests.Restaurants.UpdateCuisines
{
    public class UpdateCuisinesValidatorTests
    {
        private readonly UpdateCuisinesValidator validator;

        public UpdateCuisinesValidatorTests()
        {
            validator = new UpdateCuisinesValidator();
        }

        [Fact]
        public async Task RestaurantId_Is_Required()
        {
            var command = new UpdateCuisinesCommand()
            {
                RestaurantId = default,
                Cuisines = new List<string>(),
            };

            var result = await validator.Validate(command);

            Assert.False(result.IsSuccess);
            Assert.True(result.Error.Errors.ContainsKey(nameof(command.RestaurantId)));
        }
    }
}
