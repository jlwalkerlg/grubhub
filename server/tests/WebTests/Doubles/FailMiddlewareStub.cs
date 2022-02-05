using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Web;

namespace WebTests.Doubles
{
    public abstract class FailMiddlewareStub
    {
        public static string Message { get; } = "Fail middleware!";
    }

    public class FailMiddlewareStub<TRequest, TResponse>
        : FailMiddlewareStub,
        IPipelineBehavior<TRequest, TResponse>
        where TRequest : MediatR.IRequest<TResponse>
        where TResponse : Result, new()
    {
        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var result = new TResponse();
            result.Error = Error.BadRequest(Message);
            return Task.FromResult(result);
        }
    }
}
