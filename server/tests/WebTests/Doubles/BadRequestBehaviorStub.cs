using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Web;

namespace WebTests.Doubles
{
    public class BadRequestBehaviorStub<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : MediatR.IRequest<TResponse>
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
