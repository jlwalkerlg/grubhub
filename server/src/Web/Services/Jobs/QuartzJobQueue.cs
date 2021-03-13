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

        public async Task Enqueue(IEnumerable<Job> jobs, CancellationToken cancellationToken = default)
        {
            var dict = new Dictionary<IJobDetail, IReadOnlyCollection<ITrigger>>();

            foreach (var job in jobs)
            {
                IDictionary data = new Dictionary<string, object>()
                {
                    { "job", job },
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
