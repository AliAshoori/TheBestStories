using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Diagnostics.CodeAnalysis;
using TheBestStories.Core.Interfaces;
using TheBestStories.Core.Services;
using TheBestStories.Tests.Shared;

namespace TheBestStories.Core.Tests.HackerNewsStoryServiceTests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class GetBestNStoriesAsyncTests
    {
        [TestMethod]
        public async Task GetBestNStoriesAsync_HappyScenario_ReturnsStories()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<HackerNewsStoryService>>();

            var storyIds = new List<long> { 1, 2 };
            IEnumerable<long> GetStoryIds() { return storyIds; }

            var mockApiService = new Mock<IHackerNewsApiClientService>();
            mockApiService.Setup(m => m.GetBestStoryIdsAsync()).ReturnsAsync(GetStoryIds);

            var storyOne =
                new HackerNewsStoryDetailsResponseBuilder()
                        .WithScore(100)
                        .WithStoryId(storyIds[0])
                        .Build();

            var storyTwo =
                new HackerNewsStoryDetailsResponseBuilder()
                        .WithScore(90)
                        .WithStoryId(storyIds[1])
                        .Build();

            mockApiService.Setup(m => m.GetStoryByIdAsync(storyOne.Id)).ReturnsAsync(storyOne);
            mockApiService.Setup(m => m.GetStoryByIdAsync(storyTwo.Id)).ReturnsAsync(storyTwo);

            var service = new HackerNewsStoryService(mockLogger.Object, mockApiService.Object);

            // Act
            var result = await service.GetBestNStoriesAsync(storyIds.Count);

            // Assert
            result.Should().HaveCount(storyIds.Count);
            result.First().Should().BeEquivalentTo(storyOne);
            result.Last().Should().BeEquivalentTo(storyTwo);
        }

        [TestMethod]
        public async Task GetBestNStoriesAsync_WithRequestedNumberLessThanApiResult_ReturnsAsPerRequestedNumber()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<HackerNewsStoryService>>();

            var storyIds = new List<long> { 1, 2 };
            IEnumerable<long> GetStoryIds() { return storyIds; }

            var mockApiService = new Mock<IHackerNewsApiClientService>();
            mockApiService.Setup(m => m.GetBestStoryIdsAsync()).ReturnsAsync(GetStoryIds);

            var storyOne =
                new HackerNewsStoryDetailsResponseBuilder()
                        .WithScore(100)
                        .WithStoryId(storyIds[0])
                        .Build();

            var storyTwo =
                new HackerNewsStoryDetailsResponseBuilder()
                        .WithScore(90)
                        .WithStoryId(storyIds[1])
                        .Build();

            mockApiService.Setup(m => m.GetStoryByIdAsync(storyOne.Id)).ReturnsAsync(storyOne);
            mockApiService.Setup(m => m.GetStoryByIdAsync(storyTwo.Id)).ReturnsAsync(storyTwo);

            const int requestedNumberAsOne = 1;

            var service = new HackerNewsStoryService(mockLogger.Object, mockApiService.Object);

            // Act
            var result = await service.GetBestNStoriesAsync(requestedNumberAsOne);

            // Assert
            result.Should().HaveCount(requestedNumberAsOne);
            result.First().Should().BeEquivalentTo(storyOne);
        }

        [TestMethod]
        public async Task GetBestNStoriesAsync_WithRequestedNumberGreaterThanApiResult_ReturnsAll()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<HackerNewsStoryService>>();

            var storyIds = new List<long> { 1, 2 };
            IEnumerable<long> GetStoryIds() { return storyIds; }

            var mockApiService = new Mock<IHackerNewsApiClientService>();
            mockApiService.Setup(m => m.GetBestStoryIdsAsync()).ReturnsAsync(GetStoryIds);

            var storyOne =
                new HackerNewsStoryDetailsResponseBuilder()
                        .WithScore(100)
                        .WithStoryId(storyIds[0])
                        .Build();

            var storyTwo =
                new HackerNewsStoryDetailsResponseBuilder()
                        .WithScore(90)
                        .WithStoryId(storyIds[1])
                        .Build();

            mockApiService.Setup(m => m.GetStoryByIdAsync(storyOne.Id)).ReturnsAsync(storyOne);
            mockApiService.Setup(m => m.GetStoryByIdAsync(storyTwo.Id)).ReturnsAsync(storyTwo);

            var requestedNumber = storyIds.Count + 1;

            var service = new HackerNewsStoryService(mockLogger.Object, mockApiService.Object);

            // Act
            var result = await service.GetBestNStoriesAsync(requestedNumber);

            // Assert
            result.Should().HaveCount(storyIds.Count);
        }

        [TestMethod]
        public void GetBestNStoriesAsync_WithApiThrowsException_ReturnsException()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<HackerNewsStoryService>>();

            var storyIds = new List<long> { 1, 2 };
            IEnumerable<long> GetStoryIds() { return storyIds; }

            var mockApiService = new Mock<IHackerNewsApiClientService>();
            mockApiService.Setup(m => m.GetBestStoryIdsAsync()).ReturnsAsync(GetStoryIds);

            var storyOne =
                new HackerNewsStoryDetailsResponseBuilder()
                        .WithScore(100)
                        .WithStoryId(storyIds[0])
                        .Build();

            var storyTwo =
                new HackerNewsStoryDetailsResponseBuilder()
                        .WithScore(90)
                        .WithStoryId(storyIds[1])
                        .Build();

            mockApiService.Setup(m => m.GetStoryByIdAsync(storyOne.Id)).Throws<HttpRequestException>();
            mockApiService.Setup(m => m.GetStoryByIdAsync(storyTwo.Id)).ReturnsAsync(storyTwo);

            var service = new HackerNewsStoryService(mockLogger.Object, mockApiService.Object);

            // Act
            var result = async () => await service.GetBestNStoriesAsync(storyIds.Count);

            // Assert
            result.Should().ThrowAsync<HttpRequestException>();
        }
    }
}
