using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Web;
using Web.Data.EF;
using Web.Services.Jobs;
using Xunit;
using Shouldly;
using System.Linq;
using Web.Services;

namespace WebTests.Services.Jobs
{
    public class EFJobQueueTests : IntegrationTestBase
    {
        private readonly IServiceScope scope;
        private readonly AppDbContext db;
        private readonly EFJobQueue queue;

        public EFJobQueueTests(IntegrationTestFixture fixture) : base(fixture)
        {
            scope = factory.Services.CreateScope();
            db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            queue = new EFJobQueue(db);
        }

        [Fact]
        public async Task It_Enqueues_Jobs_And_Gets_The_Next_Job()
        {
            var dummy = new DummyJob()
            {
                Value = "Hello.",
            };

            await queue.Enqueue(dummy, new EnqueueOptions() { MaxAttempts = int.MaxValue });

            var jobs = await queue.GetNextNJobs(1);

            jobs.ShouldHaveSingleItem();

            var job = jobs.First() as DummyJob;

            job.Id.ShouldBe(dummy.Id);
            job.Value.ShouldBe(dummy.Value);
        }

        [Fact]
        public async Task It_Ignores_Jobs_That_Have_Been_Attempted_More_Than_The_Max()
        {
            var dummy = new DummyJob()
            {
                Value = "Hello.",
            };

            await queue.Enqueue(dummy, new EnqueueOptions() { MaxAttempts = 2 });

            var job = (await queue.GetNextNJobs(1)).Single();

            await queue.RegisterFailedAttempt(job);

            (await queue.GetNextNJobs(1)).ShouldNotBeEmpty();

            await queue.RegisterFailedAttempt(job);

            (await queue.GetNextNJobs(1)).ShouldBeEmpty();
        }

        [Fact]
        public async Task It_Removes_Jobs_That_Have_Been_Marked_As_Complete()
        {
            var dummy = new DummyJob()
            {
                Value = "Hello.",
            };

            await queue.Enqueue(dummy, new EnqueueOptions() { MaxAttempts = int.MaxValue });

            var job = (await queue.GetNextNJobs(1)).Single();

            await queue.MarkComplete(job);

            (await queue.GetNextNJobs(1)).ShouldBeEmpty();
        }
    }

    public class DummyJob : Job
    {
        public string Value { get; set; }
    }
}
