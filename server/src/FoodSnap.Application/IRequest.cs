using FoodSnap.Domain;

namespace FoodSnap.Application
{
    public interface IRequest : MediatR.IRequest<Result>
    {
    }

    public interface IRequest<TResult> : MediatR.IRequest<Result<TResult>>
    {
    }
}
