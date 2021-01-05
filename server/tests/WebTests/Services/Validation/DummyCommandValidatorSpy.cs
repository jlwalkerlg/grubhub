using System.Threading.Tasks;
using Web.Services.Validation;
using Web;
using WebTests.Doubles;

namespace WebTests.Services.Validation
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
