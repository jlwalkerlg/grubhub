using System.Threading;
using System.Threading.Tasks;
using Web.Services.Authentication;
using Web.Services.Hashing;

namespace Web.Features.Users.ChangePassword
{
    public class ChangePasswordHandler : IRequestHandler<ChangePasswordCommand>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IAuthenticator authenticator;
        private readonly IHasher hasher;

        public ChangePasswordHandler(IUnitOfWork unitOfWork, IAuthenticator authenticator, IHasher hasher)
        {
            this.unitOfWork = unitOfWork;
            this.authenticator = authenticator;
            this.hasher = hasher;
        }

        public async Task<Result> Handle(ChangePasswordCommand command, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.Users.GetById(authenticator.UserId);

            if (user is null) return Error.NotFound("User not found.");

            if (!hasher.CheckMatch(command.CurrentPassword, user.Password))
            {
                return Error.ValidationError(nameof(command.CurrentPassword), "Incorrect password.");
            }

            user.Password = hasher.Hash(command.NewPassword);

            await unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
