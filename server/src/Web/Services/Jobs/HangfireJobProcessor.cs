using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.Server;
using Hangfire.States;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Web.Services.Jobs
{
    public class HangfireJobProcessor
    {
        private readonly ISender sender;
        private readonly IBackgroundJobClient client;
        private readonly ILogger<HangfireJobProcessor> logger;

        public HangfireJobProcessor(ISender sender, IBackgroundJobClient client, ILogger<HangfireJobProcessor> logger)
        {
            this.sender = sender;
            this.client = client;
            this.logger = logger;
        }

        public async Task Process(Job job, CancellationToken cancellationToken, PerformContext context)
        {
            var options = job.Options ?? new EnqueueOptions();

            var attempt = context.GetJobParameter<int>("Attempt") + 1;
            context.SetJobParameter("Attempt", attempt);

            Result result = Result.Ok();
            Exception exception = null;

            try
            {
                result = await sender.Send(job, cancellationToken);

                if (!result)
                {
                    exception = new HangfireJobResultException(result.Error);
                }
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            if (exception is null) return; // success

            client.ChangeState(
                context.BackgroundJob.Id,
                new FailedState(exception));

            TimeSpan? delay = null;

            if (attempt < options.MaxAttempts)
            {
                var delayInSeconds = CalculateDelayInSeconds(attempt, options);
                delay = TimeSpan.FromSeconds(delayInSeconds);

                client.ChangeState(
                    context.BackgroundJob.Id,
                    new ScheduledState(delay.Value));
            }

            LogFailure(attempt, options, delay, result, exception, context);
        }

        private void LogFailure(int attempt, EnqueueOptions options, TimeSpan? delay, Result result, Exception exception, PerformContext context)
        {
            var reason = !result ? "handler returned an error" : " an exception occured";
            var message = $"Failed to process the job '{context.BackgroundJob.Id}': {reason}. ";

            if (attempt < options.MaxAttempts)
            {
                message += $"Attempt {attempt + 1} of {options.MaxAttempts} will be performed in {delay}. ";
            }

            message += !result ? result.Error : exception;

            var logLevel = attempt < options.MaxAttempts ? LogLevel.Warning : LogLevel.Error;

            logger.Log(logLevel, message);
        }

        private static int CalculateDelayInSeconds(int attempt, EnqueueOptions options)
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
    }
}
