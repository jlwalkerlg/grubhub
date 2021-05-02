using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Web.Features.Menus;
using Web.Services.Events;

namespace Web.Features.Restaurants.GetRestaurantById
{
    [Retry(MaxAttempts = 5)]
    public class ClearRestaurantCacheMenuUpdatedListener : IEventListener<MenuUpdatedEvent>
    {
        private readonly IDistributedCache cache;

        public ClearRestaurantCacheMenuUpdatedListener(IDistributedCache cache)
        {
            this.cache = cache;
        }

        public async Task Handle(MenuUpdatedEvent @event, CancellationToken cancellationToken)
        {
            await cache.RemoveAsync($"restaurant:{@event.RestaurantId}", cancellationToken);
        }
    }
}
