using System.Threading.Tasks;
using DotNetCore.CAP;
using Microsoft.Extensions.Caching.Distributed;
using Web.Features.Menus;
using Web.Services.Events;

namespace Web.Features.Restaurants.GetRestaurantById
{
    [CapSubscribe(nameof(ClearRestaurantCacheMenuUpdatedListener))]
    public class ClearRestaurantCacheMenuUpdatedListener : IEventListener<MenuUpdatedEvent>
    {
        private readonly IDistributedCache cache;

        public ClearRestaurantCacheMenuUpdatedListener(IDistributedCache cache)
        {
            this.cache = cache;
        }

        [CapSubscribe(nameof(MenuUpdatedEvent), isPartial: true)]
        public async Task Handle(MenuUpdatedEvent @event)
        {
            await cache.RemoveAsync($"restaurant:{@event.RestaurantId}");
        }
    }
}
