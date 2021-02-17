using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Web;
using Web.Data.EF;
using Web.Services.Jobs;
using Xunit;
using Shouldly;
using System;

namespace WebTests.Services.Jobs
{
    public class EFJobQueueTests : IntegrationTestBase, IDisposable
    {
        private readonly IServiceScope scope;
        private readonly AppDbContext db;
        private readonly EFJobQueue queue;

        public EFJobQueueTests(IntegrationTestFixture fixture) : base(fixture)
        {
            scope = fixture.Services.CreateScope();
            db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            queue = new EFJobQueue(db);
        }

        public void Dispose()
        {
            scope.Dispose();
        }

        [Fact]
        public async Task It_Enqueues_Jobs_And_Gets_The_Next_Job()
        {
            var dummy = new DummyJob()
            {
                Value = "Hello.",
            };

            await queue.Enqueue(dummy);

            var job = await queue.GetNextJob() as DummyJob;

            job.Retries.ShouldBe(dummy.Retries);
            job.Value.ShouldBe(dummy.Value);
        }

        [Fact]
        public async Task It_Ignores_Jobs_That_Have_Failed_More_Than_The_Max_Retries()
        {
            var dummy = new DummyJob()
            {
                Value = "Hello.",
            };

            await queue.Enqueue(dummy);

            var job = await queue.GetNextJob() as DummyJob;

            await queue.RegisterFailedAttempt(job);

            (await queue.GetNextJob() as DummyJob).ShouldNotBeNull();

            await queue.RegisterFailedAttempt(job);

            (await queue.GetNextJob() as DummyJob).ShouldBeNull();
        }

        [Fact]
        public async Task It_Removes_Jobs_That_Have_Been_Marked_As_Complete()
        {
            var dummy = new DummyJob()
            {
                Value = "Hello.",
            };

            await queue.Enqueue(dummy);

            var job = await queue.GetNextJob() as DummyJob;

            await queue.MarkComplete(job);

            (await queue.GetNextJob() as DummyJob).ShouldBeNull();
        }
    }

    public class DummyJob : Job
    {
        public override int Retries => 2;
        public string Value { get; set; }
    }
}
