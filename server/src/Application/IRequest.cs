namespace Application
{
    public interface IRequest : MediatR.IRequest<Result>
    {
    }

    public interface IRequest<TResult> : MediatR.IRequest<Result<TResult>>
    {
    }
}
