using FoodSnap.Application.Validation;

namespace FoodSnap.Application.Restaurants.UpdateRestaurantDetails
{
    public class UpdateRestaurantDetailsValidator : FluentValidator<UpdateRestaurantDetailsCommand>
    {
        public UpdateRestaurantDetailsValidator()
        {
            CascadeRuleFor(x => x.Id)
                .Required();

            CascadeRuleFor(x => x.Name)
                .Required();

            CascadeRuleFor(x => x.PhoneNumber)
                .Required()
                .PhoneNumber();
        }
    }
}
