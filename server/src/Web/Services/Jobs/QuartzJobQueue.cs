using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Quartz;

namespace Web.Services.Jobs
{
    public class QuartzJobQueue : IJobQueue
    {
        private readonly IScheduler scheduler;

        public QuartzJobQueue(IScheduler scheduler)
        {
            this.scheduler = scheduler;
        }

        public async Task Enqueue(
            Job job,
            EnqueueOptions options = null,
            CancellationToken cancellationToken = default)
        {
            IDictionary data = new Dictionary<string, object>()
            {
                { "job", job },
                { "options", options ?? new EnqueueOptions() },
            };

            var jobDetail = JobBuilder.Create<QuartzJobProcessor>()
                .UsingJobData(new JobDataMap(data))
                .WithIdentity(job.Id.ToString())
                .StoreDurably()
                .Build();

            var trigger = TriggerBuilder.Create()
                .StartNow()
                .WithSimpleSchedule(x => x.WithRepeatCount(0))
                .Build();

            await scheduler.ScheduleJob(jobDetail, trigger, cancellationToken);
        }

        public async Task Enqueue(IDictionary<Job, EnqueueOptions> jobs, CancellationToken cancellationToken = default)
        {
            var dict = new Dictionary<IJobDetail, IReadOnlyCollection<ITrigger>>();

            foreach (var (job, options) in jobs)
            {
                IDictionary data = new Dictionary<string, object>()
                {
                    { "job", job },
                    { "options", options ?? new EnqueueOptions() },
                };

                var jobDetail = JobBuilder.Create<QuartzJobProcessor>()
                    .UsingJobData(new JobDataMap(data))
                    .WithIdentity(job.Id.ToString())
                    .StoreDurably()
                    .Build();

                var trigger = TriggerBuilder.Create()
                    .StartNow()
                    .WithSimpleSchedule(x => x.WithRepeatCount(0))
                    .Build();

                dict.Add(jobDetail, new []{ trigger });
            }

            await scheduler.ScheduleJobs(dict, false, cancellationToken);
        }
    }
}
