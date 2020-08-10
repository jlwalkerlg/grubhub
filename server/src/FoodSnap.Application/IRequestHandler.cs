namespace FoodSnap.Application
{
    public interface IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest
        where TResponse : Result
    {
        TResponse Handle(TRequest request);
    }

    public interface IRequestHandler<TRequest> : IRequestHandler<TRequest, Result>
        where TRequest : IRequest
    {
    }
}
