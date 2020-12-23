using System.Threading;
using System.Threading.Tasks;
using Application.Services.Authentication;
namespace Application.Users.Logout
{
    public class LogoutHandler : IRequestHandler<LogoutCommand>
    {
        private readonly IAuthenticator authenticator;

        public LogoutHandler(IAuthenticator authenticator)
        {
            this.authenticator = authenticator;
        }

        public Task<Result> Handle(LogoutCommand command, CancellationToken cancellationToken)
        {
            authenticator.SignOut();

            return Task.FromResult(Result.Ok());
        }
    }
}
