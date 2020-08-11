using System.Threading.Tasks;

namespace FoodSnap.Application.Validation
{
    public interface IValidator<TRequest> where TRequest : IRequest
    {
        Task<Result> Validate(TRequest request);
    }
}
