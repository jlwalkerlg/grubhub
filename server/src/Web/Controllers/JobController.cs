using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web.Services;

namespace Web.Controllers
{
    public class JobController : Controller
    {
        private readonly IJobQueue queue;

        public JobController(IJobQueue queue)
        {
            this.queue = queue;
        }

        [HttpGet("/jobs/create")]
        public async Task<IActionResult> Create()
        {
            var job = new LogJob();
            await queue.Enqueue(job);
            return Ok();
        }
    }

    public class LogJob : Job { }

    public class LogJobProcessor : JobProcessor<LogJob>
    {
        public async Task<Result> Handle(LogJob job, CancellationToken cancellationToken)
        {
            await System.Console.Out.WriteLineAsync("LOG JOB");
            // return Result.Ok();
            return Error.NotFound("Order not found");
        }
    }
}
