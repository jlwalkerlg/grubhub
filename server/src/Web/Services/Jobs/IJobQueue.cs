using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Web.Services.Jobs
{
    public interface IJobQueue
    {
        Task Enqueue(Job job, EnqueueOptions options = null, CancellationToken cancellationToken = default);
        Task Enqueue(IDictionary<Job, EnqueueOptions> jobs, CancellationToken cancellationToken = default);
    }
}
