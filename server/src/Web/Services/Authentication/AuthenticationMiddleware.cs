using MediatR;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Web.Services.Authentication
{
    public class AuthenticationMiddleware<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TResponse : Result, new()
    {
        private readonly IAuthenticator authenticator;

        public AuthenticationMiddleware(IAuthenticator authenticator)
        {
            this.authenticator = authenticator;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            if (!RequestRequiresAuthentication(request))
            {
                return await next();
            }

            if (!authenticator.IsAuthenticated)
            {
                return new TResponse()
                {
                    Error = Error.Unauthenticated()
                };
            }

            return await next();
        }

        private bool RequestRequiresAuthentication(TRequest request)
        {
            return request.GetType()
                .GetCustomAttributes()
                .OfType<AuthenticateAttribute>()
                .Any();
        }
    }
}