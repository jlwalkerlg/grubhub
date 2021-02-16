using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Web
{
    public abstract class JobProcessor<T> : INotificationHandler<T> where T : Job
    {
        public async Task Handle(T job, CancellationToken cancellationToken)
        {
            await Process(job, cancellationToken);
        }

        protected abstract Task Process(T job, CancellationToken cancellationToken);
    }
}
