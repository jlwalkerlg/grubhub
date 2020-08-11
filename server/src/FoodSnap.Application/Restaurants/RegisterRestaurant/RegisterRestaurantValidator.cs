using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FoodSnap.Application.Validation;
using FoodSnap.Application.Validation.Failures;

namespace FoodSnap.Application.Restaurants.RegisterRestaurant
{
    public class RegisterRestaurantValidator : FluentValidator<RegisterRestaurantCommand>
    {
        private readonly IRestaurantManagerRepository restaurantManagerRepository;

        public RegisterRestaurantValidator(IRestaurantManagerRepository restaurantManagerRepository)
        {
            RuleFor(x => x.ManagerName)
                .Required();

            RuleFor(x => x.ManagerEmail)
                .Required()
                .Email()
                .MustAsync(EmailIsUnique).WithState(x => new EmailTakenFailure());

            RuleFor(x => x.ManagerPassword)
                .Required()
                .MinLength(8);

            RuleFor(x => x.RestaurantName)
                .Required();

            RuleFor(x => x.RestaurantPhoneNumber)
                .Required()
                .PhoneNumber();

            RuleFor(x => x.AddressLine1)
                .Required();

            RuleFor(x => x.Town)
                .Required();

            RuleFor(x => x.Postcode)
                .Required()
                .Postcode();
            this.restaurantManagerRepository = restaurantManagerRepository;
        }

        private async Task<bool> EmailIsUnique(string email, CancellationToken cancellationToken)
        {
            var exists = await restaurantManagerRepository.EmailExists(email);
            return !exists;
        }
    }
}
