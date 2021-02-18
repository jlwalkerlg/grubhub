namespace Web
{
    public abstract class Job : IRequest
    {
        public long Id { get; set; }
        public virtual int Retries { get; } = 1;
    }
}
