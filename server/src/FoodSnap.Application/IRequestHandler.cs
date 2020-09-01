namespace FoodSnap.Application
{
    public interface IRequestHandler<TRequest> : MediatR.IRequestHandler<TRequest, Result>
        where TRequest : IRequest
    {
    }

    public interface IRequestHandler<TRequest, TResponse> : MediatR.IRequestHandler<TRequest, Result<TResponse>>
        where TRequest : IRequest<TResponse>
        where TResponse : Result
    {
    }
}
