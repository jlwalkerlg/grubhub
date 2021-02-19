using System.Threading;
using System.Threading.Tasks;

namespace Web.Services
{
    public interface IJobQueue
    {
        Task Enqueue(Job job, EnqueueOptions options = null, CancellationToken cancellationToken = default);
    }
}
