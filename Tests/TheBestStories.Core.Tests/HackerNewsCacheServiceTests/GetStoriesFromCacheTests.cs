using FluentAssertions;
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
    public class GetStoriesFromCache
    {
        [TestMethod]
        public void GetStoriesFromCache_WithDataInCache_ReturnsData()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<HackerNewsCacheService>>();
            var options = Options.Create(new AppOptions
            {
                MemoryCache = new AppOptions.MemoryCacheOptions
                {
                    BestStoriesKey = "some-key"
                }
            });

            var stories = new List<HackerNewsStoryDetailsResponse>
            {
                new HackerNewsStoryDetailsResponseBuilder().Build()
            };

            var storiesAsBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(stories));

            var service = new HackerNewsCacheService(mockLogger.Object, GetMemoryCache(storiesAsBytes), options);

            // Act
            var result = service.GetStoriesFromCache();

            // Assert
            result.Should().BeEquivalentTo(stories);
        }

        [TestMethod]
        public void GetStoriesFromCache_WithNoDataInCache_ReturnsEmptyArray()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<HackerNewsCacheService>>();
            var options = Options.Create(new AppOptions
            {
                MemoryCache = new AppOptions.MemoryCacheOptions
                {
                    BestStoriesKey = "some-key"
                }
            });

            var stories = Enumerable.Empty<HackerNewsStoryDetailsResponse>();

            var storiesAsBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(stories));

            var service = new HackerNewsCacheService(mockLogger.Object, GetMemoryCache(storiesAsBytes), options);

            // Act
            var result = service.GetStoriesFromCache();

            // Assert
            result.Should().BeEquivalentTo(stories);
        }

        [TestMethod]
        public void GetStoriesFromCache_WithNullDataInCache_ReturnsEmptyArray()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<HackerNewsCacheService>>();
            var options = Options.Create(new AppOptions
            {
                MemoryCache = new AppOptions.MemoryCacheOptions
                {
                    BestStoriesKey = "some-key"
                }
            });

            var stories = Enumerable.Empty<HackerNewsStoryDetailsResponse>();

            var service = new HackerNewsCacheService(mockLogger.Object, GetMemoryCache(null), options);

            // Act
            var result = service.GetStoriesFromCache();

            // Assert
            result.Should().BeEquivalentTo(stories);
        }

        // ---- helpers

        private static IMemoryCache GetMemoryCache(object? expectedValue)
        {
            var mockMemoryCache = new Mock<IMemoryCache>();
            mockMemoryCache
                .Setup(x => x.TryGetValue(It.IsAny<object>(), out expectedValue))
                .Returns(true);

            return mockMemoryCache.Object;
        }
    }
}
