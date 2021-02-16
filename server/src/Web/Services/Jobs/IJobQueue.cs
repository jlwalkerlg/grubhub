using System.Threading.Tasks;

namespace Web.Services
{
    public interface IJobQueue
    {
        Task Enqueue(Job job);
        Task<Job> Dequeue();
    }
}
