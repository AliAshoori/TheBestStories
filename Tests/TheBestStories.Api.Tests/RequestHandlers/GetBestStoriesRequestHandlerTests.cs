using FluentAssertions;
using Moq;
using System.Diagnostics.CodeAnalysis;
using TheBestStories.Api.Mappers;
using TheBestStories.Api.RequestHandlers;
using TheBestStories.Api.Responses;
using TheBestStories.Core.Interfaces;
using TheBestStories.Core.Models;
using TheBestStories.Tests.Shared;

namespace TheBestStories.Api.Tests.RequestHandlers
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class GetBestStoriesRequestHandlerTests
    {
        [TestMethod]
        public async Task Handle_WithNothingInCache_ReadsFromApiService()
        {
            // Arrange
            var hackerNewsStoryDetail = new HackerNewsStoryDetailsResponseBuilder().Build();
            var hackerNewsResponse =
                new List<HackerNewsStoryDetailsResponse>
                {
                    hackerNewsStoryDetail
                };

            var mockStoryService = new Mock<IHackerNewsStoryService>();
            mockStoryService
                .Setup(m => m.GetBestNStoriesAsync(It.IsAny<int>()))
                .ReturnsAsync(hackerNewsResponse);

            var mockCacheService = new Mock<IHackerNewsCacheService>();
            mockCacheService
                .Setup(m => m.GetStoriesFromCache())
                .Returns(Enumerable.Empty<HackerNewsStoryDetailsResponse>());
            mockCacheService.Setup(m => m.AddStoriesToCache(hackerNewsResponse));

            var expectedResult = new List<GetStoryDetailResponse>
            {
                new GetStoryDetailResponseBuilder()
                        .WithTitle(hackerNewsStoryDetail.Title)
                        .WithUrl(hackerNewsStoryDetail.Url)
                        .WithCommentCount(hackerNewsStoryDetail.Descendants)
                        .WithPostedBy(hackerNewsStoryDetail.By)
                        .WithScore(hackerNewsStoryDetail.Score)
                        .WithTime(DateTimeOffset.FromUnixTimeSeconds(hackerNewsStoryDetail.Time).DateTime)
                        .Build()
            };

            var mockMapper = new Mock<IObjectMapper<GetStoryDetailResponse, HackerNewsStoryDetailsResponse>>();
            mockMapper.Setup(m => m.Map(hackerNewsStoryDetail)).Returns(expectedResult.First());

            var handler = new GetBestStoriesRequestHandler(
                mockCacheService.Object,
                mockStoryService.Object,
                mockMapper.Object);

            var request = new GetBestStoriesRequestBuilder()
                .WithRequestedNumberOfStories(1)
                .Build();

            // Act
            var result = await handler.Handle(request, default);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [TestMethod]
        public async Task Handle_WithDataAvailableInCache_ReadsFromCache()
        {
            // Arrange
            var hackerNewsStoryDetail = new HackerNewsStoryDetailsResponseBuilder().Build();
            var dataFromCache =
                new List<HackerNewsStoryDetailsResponse>
                {
                    hackerNewsStoryDetail
                };

            var mockStoryService = new Mock<IHackerNewsStoryService>();

            var request = new GetBestStoriesRequestBuilder()
                .WithRequestedNumberOfStories(1)
                .Build();

            var mockCacheService = new Mock<IHackerNewsCacheService>();
            mockCacheService
                .Setup(m => m.GetStoriesFromCache())
                .Returns(dataFromCache);

            var expectedResult = new List<GetStoryDetailResponse>
            {
                new GetStoryDetailResponseBuilder()
                        .WithTitle(hackerNewsStoryDetail.Title)
                        .WithUrl(hackerNewsStoryDetail.Url)
                        .WithCommentCount(hackerNewsStoryDetail.Descendants)
                        .WithPostedBy(hackerNewsStoryDetail.By)
                        .WithScore(hackerNewsStoryDetail.Score)
                        .WithTime(DateTimeOffset.FromUnixTimeSeconds(hackerNewsStoryDetail.Time).DateTime)
                        .Build()
            };

            var mockMapper = new Mock<IObjectMapper<GetStoryDetailResponse, HackerNewsStoryDetailsResponse>>();
            mockMapper.Setup(m => m.Map(hackerNewsStoryDetail)).Returns(expectedResult.First());

            var handler = new GetBestStoriesRequestHandler(
                mockCacheService.Object,
                mockStoryService.Object,
                mockMapper.Object);

            // Act
            var result = await handler.Handle(request, default);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
            mockStoryService.Verify(m => m.GetBestNStoriesAsync(request.RequestedNumberOfStories), Times.Never);
        }
    }
}
