using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Quartz;
using Web.Services;

namespace Web.Controllers
{
    public class QuartzController : Controller
    {
        private readonly IJobQueue queue;

        public QuartzController(IJobQueue queue)
        {
            this.queue = queue;
        }

        [HttpGet("/quartz/fire")]
        public async Task Quartz([FromQuery] bool succeed, [FromQuery] int maxAttempts = 3)
        {
            var job = new ExampleJob()
            {
                Succeed = succeed,
            };

            await queue.Enqueue(job, new EnqueueOptions()
            {
                MaxAttempts = maxAttempts,
                DelaysInSeconds = new int[] { 1, 2, 3 },
            });
        }
    }

    public class ExampleJob : Job
    {
        public bool Succeed { get; init; }
    }

    public class ExampleJobProcessor : JobProcessor<ExampleJob>
    {
        public async Task<Result> Handle(ExampleJob job, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            if (job.Succeed)
            {
                return Result.Ok();
            }

            return Error.BadRequest("Succeed was false.");
        }
    }
}
