using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Web.Data.EF;

namespace Web.Services.Jobs
{
    public class EFJobQueue : IJobQueue
    {
        private readonly AppDbContext context;

        public EFJobQueue(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<Job> Dequeue()
        {
            var serialized = await context.Jobs
                .OrderBy(x => x.Id)
                .FirstOrDefaultAsync();

            if (serialized is null) return null;

            context.Jobs.Remove(serialized);

            var job = (Job)JsonSerializer.Deserialize(
                serialized.Json,
                Type.GetType(serialized.Type));

            return job;
        }

        public async Task Enqueue(Job job)
        {
            var json = JsonSerializer.Serialize(job, job.GetType());

            var serialized = new SerialisedJob()
            {
                Retries = job.Retries,
                Attempts = job.Attempts,
                IsComplete = job.IsComplete,
                Type = job.GetType().AssemblyQualifiedName,
                Json = json,
            };

            await context.Jobs.AddAsync(serialized);
        }

        public Task Save()
        {
            return context.SaveChangesAsync();
        }
    }
}
