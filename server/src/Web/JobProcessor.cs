namespace Web
{
    public interface JobProcessor<T> : IRequestHandler<T> where T : Job
    {
    }
}
