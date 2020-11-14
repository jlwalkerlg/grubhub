using System.Threading;
using System.Threading.Tasks;
using FoodSnap.Application;
using MediatR;

namespace FoodSnap.WebTests.Actions
{
    public abstract class FailMiddlewareStub
    {
        public static string Message { get; } = "Fail middleware!";
    }

    public class FailMiddlewareStub<TRequest, TResponse>
        : FailMiddlewareStub,
        IPipelineBehavior<TRequest, TResponse>
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
