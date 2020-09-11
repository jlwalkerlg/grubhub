using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FoodSnap.Application.Users;
using FoodSnap.Application.Validation;

namespace FoodSnap.Application.Restaurants.RegisterRestaurant
{
    public class RegisterRestaurantValidator : FluentValidator<RegisterRestaurantCommand>
    {
        private readonly IRestaurantManagerRepository restaurantManagerRepository;

        public RegisterRestaurantValidator(IRestaurantManagerRepository restaurantManagerRepository)
        {
            this.restaurantManagerRepository = restaurantManagerRepository;

            CascadeRuleFor(x => x.ManagerName)
                .Required();

            CascadeRuleFor(x => x.ManagerEmail)
                .Required()
                .Email()
                .MustAsync(EmailIsUnique)
                .WithMessage("Restaurant manager is already registered.");

            CascadeRuleFor(x => x.ManagerPassword)
                .Required()
                .MinLength(8);

            CascadeRuleFor(x => x.RestaurantName)
                .Required();

            CascadeRuleFor(x => x.RestaurantPhoneNumber)
                .Required()
                .PhoneNumber();

            CascadeRuleFor(x => x.AddressLine1)
                .Required();

            CascadeRuleFor(x => x.Town)
                .Required();

            CascadeRuleFor(x => x.Postcode)
                .Required()
                .Postcode();
        }

        private async Task<bool> EmailIsUnique(string email, CancellationToken cancellationToken)
        {
            return !(await restaurantManagerRepository.EmailExists(email));
        }
    }
}
