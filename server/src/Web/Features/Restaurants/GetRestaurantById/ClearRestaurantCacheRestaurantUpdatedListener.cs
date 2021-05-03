using System.Threading.Tasks;
using DotNetCore.CAP;
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

        [CapSubscribe(nameof(RestaurantUpdatedEvent) + ":" + nameof(ClearRestaurantCacheRestaurantUpdatedListener))]
        public async Task Handle(RestaurantUpdatedEvent @event)
        {
            await cache.RemoveAsync($"restaurant:{@event.RestaurantId}");
        }
    }
}
