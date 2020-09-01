using System.Threading.Tasks;
using FoodSnap.Application;
using FoodSnap.Application.Validation;

namespace FoodSnap.ApplicationTests.Doubles
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
