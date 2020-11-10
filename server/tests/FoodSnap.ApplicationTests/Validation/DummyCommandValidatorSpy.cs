using System.Threading.Tasks;
using FoodSnap.Application.Validation;
using FoodSnap.Shared;

namespace FoodSnap.ApplicationTests.Validation
{
    public class DummyCommandValidatorSpy : IValidator<DummyCommand>
    {
        public Result Result { get; set; } = Result.Ok();

        public Task<Result> Validate(DummyCommand request)
        {
            return Task.FromResult(Result);
        }
    }
}
