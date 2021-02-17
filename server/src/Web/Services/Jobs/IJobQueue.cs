using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Web.Services
{
    public interface IJobQueue
    {
        Task Enqueue(Job job, CancellationToken cancellationToken = default);
        Task Enqueue(IEnumerable<Job> jobs, CancellationToken cancellationToken = default);
        Task<Job> GetNextJob(CancellationToken cancellationToken = default);
        Task RegisterFailedAttempt(Job job, CancellationToken cancellationToken = default);
        Task MarkComplete(Job job, CancellationToken cancellationToken = default);
    }
}
