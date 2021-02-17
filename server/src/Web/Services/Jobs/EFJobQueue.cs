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
            var serialised = await context.Jobs
                .OrderBy(x => x.Id)
                .FirstOrDefaultAsync();

            if (serialised is null) return null;

            context.Jobs.Remove(serialised);

            var job = (Job)JsonSerializer.Deserialize(
                serialised.Json,
                Type.GetType(serialised.Type));

            return job;
        }

        public async Task Enqueue(Job job)
        {
            var type = job.GetType().AssemblyQualifiedName;
            var json = JsonSerializer.Serialize(job, job.GetType());

            var serialised = new SerialisedJob()
            {
                Retries = job.Retries,
                Attempts = job.Attempts,
                IsComplete = job.IsComplete,
                Type = type,
                Json = json,
            };

            await context.Jobs.AddAsync(serialised);
        }

        public Task Save()
        {
            return context.SaveChangesAsync();
        }
    }
}
