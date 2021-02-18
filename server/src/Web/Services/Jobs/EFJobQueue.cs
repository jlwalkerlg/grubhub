using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
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

        public Task Enqueue(Job job, CancellationToken cancellationToken = default)
        {
            return Enqueue(new[] { job }, cancellationToken);
        }

        public async Task Enqueue(
            IEnumerable<Job> jobs, CancellationToken cancellationToken = default)
        {
            foreach (var job in jobs)
            {
                var type = job.GetType().AssemblyQualifiedName;
                var json = JsonSerializer.Serialize(job, job.GetType());

                var serialised = new SerialisedJob()
                {
                    Retries = job.Retries,
                    Type = type,
                    Json = json,
                };

                await context.Jobs.AddAsync(serialised, cancellationToken);
            }

            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<Job>> GetNextNJobs(
            int n, CancellationToken cancellationToken = default)
        {
            var serialisedJobs = await context.Jobs
                .Where(x => x.Attempts < x.Retries)
                .OrderBy(x => x.Id)
                .Take(n)
                .ToListAsync(cancellationToken);

            return serialisedJobs.Select(x =>
            {
                var job = (Job)JsonSerializer.Deserialize(
                    x.Json,
                    Type.GetType(x.Type));

                job.Id = x.Id;

                return job;
            });
        }

        public async Task RegisterFailedAttempt(
            Job job, CancellationToken cancellationToken = default)
        {
            var serialised = await context.Jobs.FindAsync(job.Id);

            if (serialised is null) return;

            serialised.Attempts++;

            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task MarkComplete(Job job, CancellationToken cancellationToken = default)
        {
            var serialised = await context.Jobs.FindAsync(job.Id);

            if (serialised is null) return;

            context.Jobs.Remove(serialised);

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
