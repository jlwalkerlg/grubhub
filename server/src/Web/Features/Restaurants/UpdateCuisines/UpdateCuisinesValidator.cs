using Web.Services.Validation;

namespace Web.Features.Restaurants.UpdateCuisines
{
    public class UpdateCuisinesValidator : FluentValidator<UpdateCuisinesCommand>
    {
        public UpdateCuisinesValidator()
        {
            CascadeRuleFor(x => x.RestaurantId)
                .Required();
        }
    }
}
