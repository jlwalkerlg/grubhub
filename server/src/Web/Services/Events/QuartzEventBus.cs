using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Web.Services.Events
{
    public class QuartzEventBus : IEventBus
    {
        private readonly IScheduler scheduler;
        private readonly IServiceProvider services;

        public QuartzEventBus(IScheduler scheduler, IServiceProvider services)
        {
            this.scheduler = scheduler;
            this.services = services;
        }

        public async Task Publish<TEvent>(TEvent @event, CancellationToken cancellationToken) where TEvent : Event
        {
            var listenerInterfaceType = typeof(IEventListener<>).MakeGenericType(@event.GetType());
            var listeners = services.GetServices(listenerInterfaceType).Where(x => x != null).ToList();

            var jobs = new Dictionary<IJobDetail, IReadOnlyCollection<ITrigger>>();

            foreach (var listener in listeners)
            {
                IDictionary data = new Dictionary<string, object>()
                {
                    { "event", @event },
                    { "listenerTypeName", listener.GetType().AssemblyQualifiedName },
                };

                var attribute = (RetryAttribute)Attribute.GetCustomAttribute(
                    listener.GetType(),
                    typeof(RetryAttribute));

                if (attribute is not null)
                {
                    data.Add("maxAttempts", attribute.MaxAttempts);
                }

                var job = JobBuilder.Create<QuartzEventDispatcher>()
                    .UsingJobData(new JobDataMap(data))
                    .StoreDurably()
                    .Build();

                var trigger = TriggerBuilder.Create()
                    .StartNow()
                    .WithSimpleSchedule(x => x.WithRepeatCount(0))
                    .Build();

                jobs.Add(job, new []{ trigger });
            }

            await scheduler.ScheduleJobs(jobs, false, cancellationToken);
        }
    }
}
