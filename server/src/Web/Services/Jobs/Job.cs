using System;

namespace Web.Services.Jobs
{
    public abstract class Job : IRequest
    {
        public Guid Id { get; init; } = Guid.NewGuid();

        public virtual EnqueueOptions Options { get; } = new()
        {
            MaxAttempts = 3,
        };
    }
}
