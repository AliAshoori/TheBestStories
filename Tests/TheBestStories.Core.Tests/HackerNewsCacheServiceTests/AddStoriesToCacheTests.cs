using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json;
using TheBestStories.Core.Models;
using TheBestStories.Core.Services;
using TheBestStories.Tests.Shared;

namespace TheBestStories.Core.Tests.HackerNewsCacheServiceTests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class AddStoriesToCacheTests
    {
        [TestMethod]
        public void AddStoriesToCache_HappyScenario_AddsDataToCache()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<HackerNewsCacheService>>();
            var options = Options.Create(new AppOptions
            {
                MemoryCache = new AppOptions.MemoryCacheOptions
                {
                    BestStoriesKey = "some-key",
                    DurationInSec = 30
                }
            });

            var stories = new List<HackerNewsStoryDetailsResponse>
            {
                new HackerNewsStoryDetailsResponseBuilder().Build()
            };

            var cacheExpiry = DateTimeOffset.UtcNow.Add(TimeSpan.FromSeconds(options.Value.MemoryCache.DurationInSec));
            var storiesAsBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(stories));

            var memoryCache = Mock.Of<IMemoryCache>();
            var cachEntry = Mock.Of<ICacheEntry>();

            var mockMemoryCache = Mock.Get(memoryCache);
            mockMemoryCache
                .Setup(m => m.CreateEntry(It.IsAny<object>()))
                .Returns(cachEntry);

            var service = new HackerNewsCacheService(mockLogger.Object, mockMemoryCache.Object, options);

            // Act
            service.AddStoriesToCache(stories);

            // Assert
            mockMemoryCache.Verify(m => m.CreateEntry(options.Value.MemoryCache.BestStoriesKey), Times.Once);
        }

        [TestMethod]
        public void AddStoriesToCache_WithNoData_DoesNotAddAnyDataToCache()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<HackerNewsCacheService>>();
            var options = Options.Create(new AppOptions
            {
                MemoryCache = new AppOptions.MemoryCacheOptions
                {
                    BestStoriesKey = "some-key",
                    DurationInSec = 30
                }
            });

            var stories = Enumerable.Empty<HackerNewsStoryDetailsResponse>();

            var cacheExpiry = DateTimeOffset.UtcNow.Add(TimeSpan.FromSeconds(options.Value.MemoryCache.DurationInSec));
            var storiesAsBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(stories));

            var memoryCache = Mock.Of<IMemoryCache>();
            var cachEntry = Mock.Of<ICacheEntry>();

            var mockMemoryCache = Mock.Get(memoryCache);
            mockMemoryCache
                .Setup(m => m.CreateEntry(It.IsAny<object>()))
                .Returns(cachEntry);

            var service = new HackerNewsCacheService(mockLogger.Object, mockMemoryCache.Object, options);

            // Act
            service.AddStoriesToCache(stories);

            // Assert
            mockMemoryCache.Verify(m => m.CreateEntry(options.Value.MemoryCache.BestStoriesKey), Times.Never);
        }
    }
}
