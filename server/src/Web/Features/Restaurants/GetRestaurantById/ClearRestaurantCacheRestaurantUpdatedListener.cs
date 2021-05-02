using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Web.Services.Events;

namespace Web.Features.Restaurants.GetRestaurantById
{
    public class ClearRestaurantCacheRestaurantUpdatedListener : IEventListener<RestaurantUpdatedEvent>
    {
        private readonly IDistributedCache cache;

        public ClearRestaurantCacheRestaurantUpdatedListener(IDistributedCache cache)
        {
            this.cache = cache;
        }

        public async Task Handle(RestaurantUpdatedEvent @event, CancellationToken cancellationToken)
        {
            await cache.RemoveAsync($"restaurant:{@event.RestaurantId}", cancellationToken);
        }
    }
}
