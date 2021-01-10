using System;
using System.Threading.Tasks;
using Shouldly;
using Web.Features.Restaurants.UpdateCuisines;
using Xunit;

namespace WebTests.Features.Restaurants.UpdateCuisines
{
    public class UpdateCuisinesValidatorTests
    {
        private readonly UpdateCuisinesValidator validator = new();

        [Fact]
        public async Task RestaurantId_Is_Required()
        {
            var command = new UpdateCuisinesCommand()
            {
                RestaurantId = Guid.Empty,
            };

            var result = await validator.Validate(command);

            result.ShouldBeAnError();
            result.Errors.ShouldContainKey(nameof(command.RestaurantId));
        }
    }
}
