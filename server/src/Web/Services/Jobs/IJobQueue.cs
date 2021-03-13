using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Web.Services.Jobs
{
    public interface IJobQueue
    {
        Task Enqueue(IEnumerable<Job> jobs, CancellationToken cancellationToken = default);
    }
}
