using System.Threading;
using System.Threading.Tasks;
using Application;

namespace ApplicationTests
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
