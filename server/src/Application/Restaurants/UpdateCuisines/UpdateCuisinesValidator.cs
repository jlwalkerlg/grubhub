using Application.Validation;

namespace Application.Restaurants.UpdateCuisines
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
