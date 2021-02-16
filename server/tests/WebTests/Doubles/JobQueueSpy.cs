using System.Collections.Generic;
using System.Threading.Tasks;
using Web;
using Web.Services;

namespace WebTests.Doubles
{
    public class JobQueueSpy : IJobQueue
    {
        public List<Job> Jobs { get; } = new();

        public Task<Job> Dequeue()
        {
            throw new System.NotImplementedException();
        }

        public Task Enqueue(Job job)
        {
            Jobs.Add(job);
            return Task.CompletedTask;
        }
    }
}
