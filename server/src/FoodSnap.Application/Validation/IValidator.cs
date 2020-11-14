using System.Threading.Tasks;
using FoodSnap.Application;

namespace FoodSnap.Application.Validation
{
    public interface IValidator<TRequest>
    {
        Task<Result> Validate(TRequest request);
    }
}
