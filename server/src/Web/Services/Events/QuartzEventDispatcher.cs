using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Web.Services.Events
{
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
            var @event = context.JobDetail.JobDataMap.Get("event");
            var listenerTypeName = (string)context.JobDetail.JobDataMap.Get("listenerTypeName");

            var listener = services.GetRequiredService(Type.GetType(listenerTypeName));

            try
            {
                await Dispatch(@event, listener);

                await context.Scheduler.DeleteJob(context.JobDetail.Key, context.CancellationToken);
            }
            catch (Exception exception)
            {
                await HandleFailure(context, @event.GetType(), listener.GetType(), exception);
            }
        }

        private static async Task Dispatch(object @event, object listener)
        {
            var wrapperType = typeof(ListenerWrapper<>).MakeGenericType(@event.GetType());
            var wrapper = (ListenerWrapperBase) Activator.CreateInstance(wrapperType);

            await wrapper.Dispatch(@event, listener, default);
        }

        private async Task HandleFailure(IJobExecutionContext context, Type eventType, Type listenerType, Exception exception)
        {
            var attempt = context.Trigger.JobDataMap.GetInt("attempt") + 1;
            context.Trigger.JobDataMap.Put("attempt", attempt);

            var maxAttempts = Math.Max(1, context.JobDetail.JobDataMap.GetInt("maxAttempts"));

            if (attempt < maxAttempts)
            {
                var delayInSeconds = CalculateDelayInSeconds(attempt);
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

        private int CalculateDelayInSeconds(int attempt)
        {
            // https://github.com/HangfireIO/Hangfire/blob/master/src/Hangfire.Core/AutomaticRetryAttribute.cs
            var rng = new Random();
            return (int)Math.Round(Math.Pow(attempt - 1, 4) + 15 + rng.Next(30) * attempt);
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
