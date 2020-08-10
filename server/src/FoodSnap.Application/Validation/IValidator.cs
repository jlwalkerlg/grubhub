namespace FoodSnap.Application.Validation
{
    public interface IValidator<TRequest> where TRequest : IRequest
    {
        Result Validate(TRequest request);
    }
}
