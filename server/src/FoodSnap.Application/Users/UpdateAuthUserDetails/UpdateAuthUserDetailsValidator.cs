using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FoodSnap.Application.Services.Authentication;
using FoodSnap.Application.Validation;

namespace FoodSnap.Application.Users.UpdateAuthUserDetails
{
    public class UpdateAuthUserDetailsValidator : FluentValidator<UpdateAuthUserDetailsCommand>
    {
        private readonly IAuthenticator authenticator;
        private readonly IUnitOfWork unitOfWork;

        public UpdateAuthUserDetailsValidator(IAuthenticator authenticator, IUnitOfWork unitOfWork)
        {
            this.authenticator = authenticator;
            this.unitOfWork = unitOfWork;

            CascadeRuleFor(x => x.Name)
                .Required();

            CascadeRuleFor(x => x.Email)
                .Required()
                .Email()
                .MustAsync(EmailIsUnique)
                .WithMessage("Email taken.");
        }

        private async Task<bool> EmailIsUnique(string email, CancellationToken cancellationToken)
        {
            var existingUser = await unitOfWork.Users.GetByEmail(email);
            return existingUser == null || existingUser.Id == authenticator.UserId;
        }
    }
}
