using System.Threading.Tasks;

namespace FoodSnap.Application.Validation
{
    public interface IValidator<TRequest>
    {
        Task<Result> Validate(TRequest request);
    }
}
