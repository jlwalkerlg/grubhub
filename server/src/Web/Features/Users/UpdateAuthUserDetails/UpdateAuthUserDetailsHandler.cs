using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Web.Domain;
using Web.Services.Authentication;

namespace Web.Features.Users.UpdateAuthUserDetails
{
    public class UpdateAuthUserDetailsHandler : IRequestHandler<UpdateAuthUserDetailsCommand>
    {
        private readonly IAuthenticator authenticator;
        private readonly IUnitOfWork unitOfWork;

        public UpdateAuthUserDetailsHandler(IAuthenticator authenticator, IUnitOfWork unitOfWork)
        {
            this.authenticator = authenticator;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateAuthUserDetailsCommand command, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.Users.GetById(authenticator.UserId);

            if (command.Email != user.Email.Address)
            {
                if (await unitOfWork.Users.EmailExists(command.Email))
                {
                    return Error.ValidationError(new Dictionary<string, string>()
                    {
                        { nameof(command.Email), "Email already taken." }
                    });
                }
            }

            user.Rename(command.FirstName, command.LastName);
            user.Email = new Email(command.Email);

            await unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
