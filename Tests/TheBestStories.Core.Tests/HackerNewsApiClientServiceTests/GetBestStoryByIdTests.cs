using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using TheBestStories.Core.Interfaces;
using TheBestStories.Core.Models;
using TheBestStories.Core.Services;
using TheBestStories.Tests.Shared;

namespace TheBestStories.Core.Tests.HackerNewsApiClientServiceTests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class GetBestStoryByIdTests
    {
        [TestMethod]
        public async Task GetStoryByIdAsync_HappyScenario_ReturnsStory()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<HackerNewsApiClientService>>();
            var story = new HackerNewsStoryDetailsResponseBuilder().Build();

            var mockHttpHandler = new Mock<HttpMessageHandler>();
            var setupApiRequest = mockHttpHandler.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
                );

            setupApiRequest.ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(story))
            });

            var mockPolicyExecutor = new Mock<IHackerNewsApiClientPolicyExecutorService>();

            var mockHttpClient = new HttpClient(mockHttpHandler.Object) { BaseAddress = new Uri("https://hello-there") };

            var service = new HackerNewsApiClientService(mockLogger.Object, mockHttpClient, mockPolicyExecutor.Object);

            // Act
            var result = await service.GetStoryByIdAsync(story.Id);

            // Assert
            result.Should().BeEquivalentTo(story);
        }

        [TestMethod]
        public void GetStoryByIdAsync_WithApiException_ThrowsHttpRequestException()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<HackerNewsApiClientService>>();
            var story = new HackerNewsStoryDetailsResponseBuilder().Build();

            var mockHttpHandler = new Mock<HttpMessageHandler>();
            var setupApiRequest = mockHttpHandler.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
                );

            setupApiRequest.ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.InternalServerError
            });

            var mockPolicyExecutor = new Mock<IHackerNewsApiClientPolicyExecutorService>();

            var mockHttpClient = new HttpClient(mockHttpHandler.Object) { BaseAddress = new Uri("https://hello-there") };

            var service = new HackerNewsApiClientService(mockLogger.Object, mockHttpClient, mockPolicyExecutor.Object);

            // Act
            var result = async () => await service.GetStoryByIdAsync(story.Id);

            // Assert
            result.Should().ThrowAsync<HttpRequestException>();
        }

        [TestMethod]
        public async Task GetStoryByIdAsync_WithNoDataFromApi_ReturnsEmptyArray()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<HackerNewsApiClientService>>();
            HackerNewsStoryDetailsResponse? story = null;

            var mockHttpHandler = new Mock<HttpMessageHandler>();
            var setupApiRequest = mockHttpHandler.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
                );

            var hackerNewsResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(story))
            };

            setupApiRequest.ReturnsAsync(hackerNewsResponse);

            var mockPolicyExecutor = new Mock<IHackerNewsApiClientPolicyExecutorService>();
            mockPolicyExecutor
                .Setup(m => m.ExecuteAsync(It.IsAny<Func<Task<HttpResponseMessage>>>()))
                .ReturnsAsync(hackerNewsResponse);

            var mockHttpClient = new HttpClient(mockHttpHandler.Object) { BaseAddress = new Uri("https://hello-there") };

            var service = new HackerNewsApiClientService(mockLogger.Object, mockHttpClient, mockPolicyExecutor.Object);

            // Act
            var result = await service.GetBestStoryIdsAsync();

            // Assert
            result.Should().HaveCount(0);
        }
    }
}