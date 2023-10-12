using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;
using TheBestStories.Core.Interfaces;
using TheBestStories.Core.Models;

namespace TheBestStories.Core.Services
{
    public class HackerNewsCacheService : IHackerNewsCacheService
    {
        private readonly ILogger<HackerNewsCacheService> _logger;
        private readonly IOptions<AppOptions> _options;
        private readonly IMemoryCache _memoryCache;

        public HackerNewsCacheService(
            ILogger<HackerNewsCacheService> logger,
            IMemoryCache memoryCache,
            IOptions<AppOptions> options)
        {
            _logger = logger;
            _options = options;
            _memoryCache = memoryCache;
        }

        public void AddStoriesToCache(IEnumerable<HackerNewsStoryDetailsResponse> hackerNewsStories)
        {
            if (!hackerNewsStories.Any())
            {
                _logger.LogInformation($"{nameof(AddStoriesToCache)} - no stories received to add to the cache");

                return;
            }

            var cacheExpiry = DateTimeOffset.UtcNow.Add(TimeSpan.FromSeconds(_options.Value.MemoryCache.DurationInSec));
            var storiesAsBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(hackerNewsStories));

            _logger.LogInformation($"{nameof(AddStoriesToCache)} - caching {hackerNewsStories.Count()} number of stories as bytes");

            _memoryCache.Set(
                _options.Value.MemoryCache.BestStoriesKey,
                storiesAsBytes,
                cacheExpiry);
        }

        public IEnumerable<HackerNewsStoryDetailsResponse> GetStoriesFromCache()
        {
            _logger.LogInformation($"{nameof(GetStoriesFromCache)} - fetching stories from Cache");

            var storiesAsBytes = _memoryCache.Get<byte[]>(_options.Value.MemoryCache.BestStoriesKey);

            if (storiesAsBytes == null || !storiesAsBytes.Any())
            {
                return Enumerable.Empty<HackerNewsStoryDetailsResponse>();
            }

            var storiesFromCache = 
                JsonSerializer.Deserialize<IEnumerable<HackerNewsStoryDetailsResponse?>>(storiesAsBytes) ?? 
                Enumerable.Empty<HackerNewsStoryDetailsResponse>();

            _logger.LogInformation($"{nameof(GetStoriesFromCache)} - returning {storiesFromCache.Count()} number of stories");

            return storiesFromCache;
        }
    }
}