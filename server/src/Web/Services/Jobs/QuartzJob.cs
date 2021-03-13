using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Web.Services.Jobs
{
    public class QuartzJobProcessor : Quartz.IJob
    {
        private readonly ILogger<QuartzJobProcessor> logger;
        private readonly ISender sender;

        public QuartzJobProcessor(ILogger<QuartzJobProcessor> logger, ISender sender)
        {
            this.logger = logger;
            this.sender = sender;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var job = (Job)context.JobDetail.JobDataMap.Get("job");
            var options = job.Options ?? new EnqueueOptions();

            Result result = null;
            Exception exception = null;

            try
            {
                result = await sender.Send(job, context.CancellationToken);
            }
            catch (System.Exception ex)
            {
                exception = ex;
            }

            var isSuccess = exception is null && result.IsSuccess;

            if (isSuccess)
            {
                await context.Scheduler.DeleteJob(
                    context.JobDetail.Key,
                    context.CancellationToken);

                return;
            }

            var attempt = context.Trigger.JobDataMap.GetInt("attempt") + 1;
            context.Trigger.JobDataMap.Put("attempt", attempt);

            if (attempt < options.MaxAttempts)
            {
                var delayInSeconds = CalculateDelayInSeconds(options, attempt);
                var delay = TimeSpan.FromSeconds(delayInSeconds);

                LogFailure(LogLevel.Warning, job, result, exception);
                logger.LogInformation($"Retry {attempt} of {options.MaxAttempts - 1} will be performed in {delay}");

                await context.Scheduler.RescheduleJob(
                    context.Trigger.Key,
                    context.Trigger.GetTriggerBuilder()
                        .StartAt(DateTimeOffset.UtcNow.Add(delay))
                        .Build(),
                    context.CancellationToken);

                return;
            }

            LogFailure(LogLevel.Error, job, result, exception);

            var jobExecutionException = exception is null
                ? new JobExecutionException(result.Error.Message)
                : new JobExecutionException(exception);

            jobExecutionException.RefireImmediately = false;
            jobExecutionException.UnscheduleAllTriggers = true;

            throw jobExecutionException;
        }

        private int CalculateDelayInSeconds(EnqueueOptions options, int attempt)
        {
            if (options.DelaysInSeconds is null || options.DelaysInSeconds.Length == 0)
            {
                // https://github.com/HangfireIO/Hangfire/blob/master/src/Hangfire.Core/AutomaticRetryAttribute.cs
                var random = new Random();

                return (int)Math.Round(
                    Math.Pow(attempt - 1, 4) + 15 + random.Next(30) * attempt);
            }

            if (options.DelaysInSeconds.Length >= attempt)
            {
                return options.DelaysInSeconds[attempt - 1];
            }

            return options.DelaysInSeconds.Last();
        }

        private void LogFailure(LogLevel level, Job job, Result result, Exception exception)
        {
            var reason = exception is null
                ? " handler returned an error"
                : " an exception occured";

            var message = $"Failed to process the job '{job.Id}': {reason}.";

            if (level == LogLevel.Error)
            {
                message += " " + (exception?.ToString() ?? result.Error.Message) + ".";
            }

            logger.Log(level, message);
        }
    }
}
