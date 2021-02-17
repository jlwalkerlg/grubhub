using MediatR;

namespace Web
{
    public abstract class Job : INotification
    {
        public long Id { get; set; }
        public virtual int Retries { get; } = 1;
    }
}
