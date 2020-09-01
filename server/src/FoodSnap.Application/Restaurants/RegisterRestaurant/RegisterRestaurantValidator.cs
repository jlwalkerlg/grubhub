using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FoodSnap.Application.Users;
using FoodSnap.Application.Validation;
using FoodSnap.Application.Validation.Failures;

namespace FoodSnap.Application.Restaurants.RegisterRestaurant
{
    public class RegisterRestaurantValidator : FluentValidator<RegisterRestaurantCommand>
    {
        private readonly IRestaurantManagerRepository restaurantManagerRepository;

        public RegisterRestaurantValidator(IRestaurantManagerRepository restaurantManagerRepository)
        {
            this.restaurantManagerRepository = restaurantManagerRepository;

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
        }

        private async Task<bool> EmailIsUnique(string email, CancellationToken cancellationToken)
        {
            return !(await restaurantManagerRepository.EmailExists(email));
        }
    }
}
