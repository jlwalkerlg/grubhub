using System.Threading;
using System.Threading.Tasks;
using Web.Services.Authentication;
using Web.Services.Hashing;

namespace Web.Features.Users.Login
{
    public class LoginHandler : IRequestHandler<LoginCommand>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IAuthenticator authenticator;
        private readonly IHasher hasher;

        public LoginHandler(
            IUnitOfWork unitOfWork,
            IAuthenticator authenticator,
            IHasher hasher)
        {
            this.unitOfWork = unitOfWork;
            this.authenticator = authenticator;
            this.hasher = hasher;
        }

        public async Task<Result> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.Users.GetByEmail(command.Email);

            if (user == null)
            {
                return Error.BadRequest("Invalid credentials.");
            }

            if (!hasher.CheckMatch(command.Password, user.Password))
            {
                return Error.BadRequest("Invalid credentials.");
            }

            authenticator.SignIn(user);

            return Result.Ok();
        }
    }
}
