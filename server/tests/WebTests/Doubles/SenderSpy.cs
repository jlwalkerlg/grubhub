using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace WebTests.Doubles
{
    public class SenderSpy : ISender
    {
        public object Response { get; set; }

        public List<object> Requests { get; } = new();

        public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            Requests.Add(request);
            return Task.FromResult((TResponse)Response);
        }

        public Task<object> Send(object request, CancellationToken cancellationToken = default)
        {
            Requests.Add(request);
            return Task.FromResult(Response);
        }

        public IAsyncEnumerable<TResponse> CreateStream<TResponse>(IStreamRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public IAsyncEnumerable<object> CreateStream(object request, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }
    }
}
