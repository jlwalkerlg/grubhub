using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace Web.Extensions
{
    public static class DistributedCacheExtensions
    {
        public static async Task SetAsync<T>(this IDistributedCache cache,
            string key,
            T data,
            CancellationToken cancellationToken = default)
        {
            var json = JsonSerializer.Serialize(data);
            await cache.SetStringAsync(key, json, cancellationToken);
        }

        public static async Task SetAsync<T>(this IDistributedCache cache,
            string key,
            T data,
            DistributedCacheEntryOptions options,
            CancellationToken cancellationToken = default)
        {
            var json = JsonSerializer.Serialize(data);
            await cache.SetStringAsync(key, json, options, cancellationToken);
        }

        public static async Task<T> GetAsync<T>(this IDistributedCache cache,
            string key,
            CancellationToken cancellationToken = default)
        {
            var json = await cache.GetAsync(key, cancellationToken);
            if (json is null) return default;

            return JsonSerializer.Deserialize<T>(json);
        }
    }
}
