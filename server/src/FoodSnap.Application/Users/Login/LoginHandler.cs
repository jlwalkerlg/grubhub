using System.Threading;
using System.Threading.Tasks;
using FoodSnap.Application.Services.Authentication;
using FoodSnap.Application.Services.Hashing;

namespace FoodSnap.Application.Users.Login
{
    public class LoginHandler : IRequestHandler<LoginCommand>
    {
        private readonly IUserRepository repository;
        private readonly IAuthenticator authenticator;
        private readonly IHasher hasher;

        public LoginHandler(
            IUserRepository repository,
            IAuthenticator authenticator,
            IHasher hasher)
        {
            this.repository = repository;
            this.authenticator = authenticator;
            this.hasher = hasher;
        }

        public async Task<Result> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            var user = await repository.GetByEmail(command.Email);

            if (user == null)
            {
                return Result.Fail(Error.BadRequest("Invalid credentials."));
            }

            if (!hasher.CheckMatch(command.Password, user.Password))
            {
                return Result.Fail(Error.BadRequest("Invalid credentials."));
            }

            authenticator.SignIn(user);

            return Result.Ok();
        }
    }
}
