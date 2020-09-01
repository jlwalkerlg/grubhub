using System.Threading;
using System.Threading.Tasks;
using FoodSnap.Application;

namespace FoodSnap.ApplicationTests.Doubles
{
    public class DummyCommandHandlerSpy : IRequestHandler<DummyCommand>
    {
        public Result Result { get; set; } = Result.Ok();

        public Task<Result> Handle(DummyCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(Result);
        }
    }
}
