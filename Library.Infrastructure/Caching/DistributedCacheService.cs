using Library.Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace Library.Infrastructure.Caching
{
    public class DistributedCacheService : ICacheService
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger<DistributedCacheService> _logger;
        public DistributedCacheService(IDistributedCache cache, ILogger<DistributedCacheService> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken)
        {
            var bytes = await _cache.GetAsync(key, cancellationToken);

            if(bytes == null || bytes.Length == 0)
            {
                _logger.LogInformation($"Log miss: {key}");
                return default;
            }

            _logger.LogInformation($"Log hit: {key}");
            var json = Encoding.UTF8.GetString(bytes);
            return JsonSerializer.Deserialize<T>(json);
        }

        public async Task RemoveAsync(string key, CancellationToken cancellationToken)
        {
            await _cache.RemoveAsync(key, cancellationToken);
            _logger.LogInformation($"Cache removed: {key}");
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan ttl, CancellationToken cancellationToken)
        {
            var json = JsonSerializer.Serialize(value);
            var bytes = Encoding.UTF8.GetBytes(json);

            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = ttl,
            };

            await _cache.SetAsync(key, bytes, options, cancellationToken);
            _logger.LogInformation($"Cache set: {key}, ttl = {ttl}");
        }
    }
}
