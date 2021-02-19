using System;

namespace Web
{
    public abstract class Job : IRequest
    {
        public Guid Id { get; init; } = Guid.NewGuid();
    }
}
