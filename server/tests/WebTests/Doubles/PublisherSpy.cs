using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace WebTests.Doubles
{
    public class PublisherSpy : IPublisher
    {
        public List<object> Notifications { get; } = new();

        public Task Publish(object notification, CancellationToken cancellationToken = default)
        {
            Notifications.Add(notification);
            return Task.CompletedTask;
        }

        public Task Publish<TNotification>(
            TNotification notification,
            CancellationToken cancellationToken = default)
            where TNotification : INotification
        {
            return Publish(notification as object, cancellationToken);
        }
    }
}
