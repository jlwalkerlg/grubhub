using System.Linq;
using System.Collections.Generic;
using FoodSnap.Application.Validation;

namespace FoodSnap.Application.Restaurants.RegisterRestaurant
{
    public class RegisterRestaurantValidator : IValidator<RegisterRestaurantCommand>
    {
        public Result Validate(RegisterRestaurantCommand command)
        {
            var errors = new Dictionary<string, IValidationFailure>();

            if (string.IsNullOrWhiteSpace(command.ManagerName))
            {
                errors.Add(nameof(command.ManagerName), new RequiredFailure());
            }

            if (errors.Any())
            {
                return Result.Fail(new ValidationError(errors));
            }

            return Result.Ok();
        }
    }
}
