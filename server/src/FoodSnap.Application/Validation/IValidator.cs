using System.Threading.Tasks;
using FoodSnap.Domain;

namespace FoodSnap.Application.Validation
{
    public interface IValidator<TRequest>
    {
        Task<Result> Validate(TRequest request);
    }
}
