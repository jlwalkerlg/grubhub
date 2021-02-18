using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Web;

namespace WebTests
{
    public class BadRequestBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TResponse : Result, new()
    {
        public Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            var response = new TResponse();
            response.Error = Error.BadRequest("Bad request.");

            return Task.FromResult(response);
        }
    }
}
