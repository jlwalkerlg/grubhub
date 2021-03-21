using System;
using System.Text.RegularExpressions;
using Web.Services.Validation;

namespace Web.Features.Restaurants.UpdateRestaurantDetails
{
    public class UpdateRestaurantDetailsValidator : RequestValidator<UpdateRestaurantDetailsCommand>
    {
        private static readonly Regex regex = new(
            @"^\d{2}:\d{2}$",
            RegexOptions.Compiled,
            TimeSpan.FromMilliseconds(250));

        public UpdateRestaurantDetailsValidator()
        {
            CascadeRuleFor(x => x.RestaurantId)
                .Required();

            CascadeRuleFor(x => x.Name)
                .Required();

            CascadeRuleFor(x => x.Description)
                .MaxLength(400);

            CascadeRuleFor(x => x.PhoneNumber)
                .Required()
                .PhoneNumber();

            CascadeRuleFor(x => x.DeliveryFee)
                .Min(0);

            CascadeRuleFor(x => x.MinimumDeliverySpend)
                .Min(0);

            CascadeRuleFor(x => x.MaxDeliveryDistanceInKm)
                .Min(0);

            CascadeRuleFor(x => x.EstimatedDeliveryTimeInMinutes)
                .Min(5);
        }
    }
}
