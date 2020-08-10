namespace FoodSnap.Application
{
    public interface IValidator<TRequest> where TRequest : IRequest
    {
        Result Validate(TRequest request);
    }
}
