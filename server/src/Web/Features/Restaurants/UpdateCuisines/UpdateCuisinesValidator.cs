using Web.Services.Validation;

namespace Web.Features.Restaurants.UpdateCuisines
{
    public class UpdateCuisinesValidator : RequestValidator<UpdateCuisinesCommand>
    {
        public UpdateCuisinesValidator()
        {
            CascadeRuleFor(x => x.RestaurantId)
                .Required();
        }
    }
}
