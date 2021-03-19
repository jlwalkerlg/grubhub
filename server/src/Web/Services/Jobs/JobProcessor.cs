namespace Web.Services.Jobs
{
    public interface IJobProcessor<T> : IRequestHandler<T> where T : Job
    {
    }
}
