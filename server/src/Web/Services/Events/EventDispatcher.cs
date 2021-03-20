using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Web.Services.Jobs;

namespace Web.Services.Events
{
    public abstract class EventDispatcher<TEvent> : MediatR.IRequestHandler<TEvent> where TEvent : Event
    {
        private readonly IJobQueue queue;

        protected EventDispatcher(IJobQueue queue)
        {
            this.queue = queue;
        }

        public async Task<Unit> Handle(TEvent @event, CancellationToken cancellationToken)
        {
            await queue.Enqueue(await GetJobs(@event), cancellationToken);
            return Unit.Value;
        }

        protected abstract Task<IEnumerable<Job>> GetJobs(TEvent @event);
    }
}
