using FluentValidation;
using System.Threading;
using System.Threading.Tasks;
using Web.Services.Validation;

namespace Web.Features.Restaurants.RegisterRestaurant
{
    public class RegisterRestaurantValidator : FluentValidator<RegisterRestaurantCommand>
    {
        private readonly IUnitOfWork unitOfWork;

        public RegisterRestaurantValidator(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;

            CascadeRuleFor(x => x.ManagerName)
                .Required();

            CascadeRuleFor(x => x.ManagerEmail)
                .Required()
                .Email()
                .MustAsync(EmailIsUnique)
                .WithMessage("User is already registered.");

            CascadeRuleFor(x => x.ManagerPassword)
                .Required()
                .MinLength(8);

            CascadeRuleFor(x => x.RestaurantName)
                .Required();

            CascadeRuleFor(x => x.RestaurantPhoneNumber)
                .Required()
                .PhoneNumber();

            CascadeRuleFor(x => x.Address)
                .Required();
        }

        private async Task<bool> EmailIsUnique(string email, CancellationToken cancellationToken)
        {
            return !(await unitOfWork.Users.EmailExists(email));
        }
    }
}
