using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace FoodSnap.WebTests
{
    public class MediatorSpy : IMediator
    {
        public object Request { get; private set; }
        public object Result { get; set; }

        public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            Request = request;
            return Task.FromResult((TResponse)Result);
        }

        public Task<object> Send(object request, CancellationToken cancellationToken = default)
        {
            Request = request;
            return Task.FromResult(Result);
        }

        public Task Publish(object notification, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification
        {
            throw new System.NotImplementedException();
        }
    }
}
