using System;
using Web.Services.Jobs;

namespace Web
{
    public abstract class Job : IRequest
    {
        public Guid Id { get; init; } = Guid.NewGuid();

        public virtual EnqueueOptions Options { get; } = null;
    }
}
