using System.Threading.Tasks;
namespace Web.Services.Validation
{
    public interface IValidator<TRequest>
    {
        Task<Result> Validate(TRequest request);
    }
}
