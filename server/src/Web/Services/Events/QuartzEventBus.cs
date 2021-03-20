using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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

                var jobDetail = JobBuilder.Create<QuartzEventDispatcher>()
                    .UsingJobData(new JobDataMap(data))
                    .StoreDurably()
                    .Build();

                var trigger = TriggerBuilder.Create()
                    .StartNow()
                    .WithSimpleSchedule(x => x.WithRepeatCount(0))
                    .Build();

                jobs.Add(jobDetail, new []{ trigger });
            }

            await scheduler.ScheduleJobs(jobs, false, cancellationToken);
        }
    }

    public class QuartzEventDispatcher : IJob
    {
        private readonly IServiceProvider services;
        private readonly ILogger<QuartzEventDispatcher> logger;

        public QuartzEventDispatcher(IServiceProvider services, ILogger<QuartzEventDispatcher> logger)
        {
            this.services = services;
            this.logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            // TODO: dispatch without dynamic
            dynamic @event = context.JobDetail.JobDataMap.Get("event");
            var listenerTypeName = (string)context.JobDetail.JobDataMap.Get("listenerTypeName");

            dynamic listener = services.GetRequiredService(Type.GetType(listenerTypeName)!);

            try
            {
                await listener.Handle(@event, CancellationToken.None);

                await context.Scheduler.DeleteJob(context.JobDetail.Key, context.CancellationToken);
            }
            catch (Exception exception)
            {
                await HandleFailure(context, @event.GetType(), listener.GetType(), exception);
            }
        }

        private async Task HandleFailure(IJobExecutionContext context, Type eventType, Type listenerType, Exception exception)
        {
            var attempt = context.Trigger.JobDataMap.GetInt("attempt") + 1;
            context.Trigger.JobDataMap.Put("attempt", attempt);

            var maxAttempts = Math.Max(1, context.Trigger.JobDataMap.GetInt("maxAttempts"));
            var delays = context.Trigger.JobDataMap.Get("delays") as int[] ?? new int[] { };

            if (attempt < maxAttempts)
            {
                var delayInSeconds = CalculateDelayInSeconds(delays, attempt);
                var delay = TimeSpan.FromSeconds(delayInSeconds);

                LogFailure(LogLevel.Warning, eventType, listenerType, exception);
                logger.LogInformation($"Retry {attempt} of {maxAttempts - 1} will be performed in {delay}");

                await context.Scheduler.RescheduleJob(
                    context.Trigger.Key,
                    context.Trigger.GetTriggerBuilder().StartAt(DateTimeOffset.UtcNow.Add(delay)).Build(),
                    context.CancellationToken);

                return;
            }

            LogFailure(LogLevel.Error, eventType, listenerType, exception);

            throw new JobExecutionException(exception)
            {
                RefireImmediately = false,
                UnscheduleAllTriggers = true,
            };
        }

        private int CalculateDelayInSeconds(int[] delays, int attempt)
        {
            if (delays is null || delays.Length == 0)
            {
                // https://github.com/HangfireIO/Hangfire/blob/master/src/Hangfire.Core/AutomaticRetryAttribute.cs
                var rng = new Random();

                return (int)Math.Round(Math.Pow(attempt - 1, 4) + 15 + rng.Next(30) * attempt);
            }

            return delays.Length >= attempt ? delays[attempt - 1] : delays.Last();
        }

        private void LogFailure(LogLevel level, Type eventType, Type listenerType, Exception exception)
        {
            var message = $"Listener '{listenerType}' failed to process the event '{eventType}': an exception occured.";

            if (level == LogLevel.Error)
            {
                message += " " + exception + ".";
            }

            logger.Log(level, message);
        }
    }
}
